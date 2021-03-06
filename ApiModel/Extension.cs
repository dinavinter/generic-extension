using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using HttpMethod = Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpMethod;

namespace GenericExtesion
{
    public class Extension
    {
        /// <example>H4sIAAAAAAAAChXKuwqEQAxG4VdZ%2F9rC2m7xggvaaGUZJIgYVCZJITLvvmN5Ps6DzuyqSERRHi6So2ahe0hZ5BhZXQzlg8nIPCHa769HusjodVcOLS3bsTYhnGFgVVo5ffPpn8mXPUOM8Q%2BFTF%2BMZwAAAA%3D%3D</example> 
        public string ExtensionId;
    }

    /// <remarks>
    /// Sample request:
    ///
    ///     POST /ExtensionModel
    ///     {
    ///        "Result": { "Status": "FAIL",  "Data": { "userFacingErrorMessage" : "You Suck!" } },
    ///        "DelayMs" : 1000
    ///     }
    ///
    /// </remarks>
    public class ExtensionModel
    { 
        public string? Id { get; set; }

        public HttpCall[] HttpCalls { get; set; } = new HttpCall[0];

        public int? DelayMs { get; set; }
        public dynamic Result { get; set; } = new {status = "OK"};


        public Task Delay(HttpContext context)
        {
            if (DelayMs != null)
            {
                context.Response.Headers["X-DelayFor"] = TimeSpan.FromMilliseconds(DelayMs.Value).ToString();

                return Task.Delay(DelayMs.Value);
                
            }
            return Task.CompletedTask;
        }
    }


    public class HttpCall
    {
        public string BaseUrl { get; set; }

        ///<remarks>
        /// Sample REQUEST PARAMETERS:
        /// {
        ///    "uid" : "${data.accountInfo.uid}",
        ///    "requirePasswordChange" : "true",
        ///    "client_id": "${apiKey}",
        ///    "oauth_token": "st2.s.AcbHpnRbvw.w6EnvRmwGWfH102lvUx_dc2NwrHikBJS4EJ7h2_PQ-uN2PiCi4UKwKdcGOTGRFHKe-eGr1I3Ih-W3IdC01Iixg.9t3Yff8nzk5ivDDTW7EQMTBBi_4PWnondFvxf4uz0OHTnrsdq_RiQTRpwD7oVuQyfX7n6YFtDEk1enzlKq2fzw.sc3"
        /// }	 
        /// </remarks>
        public ExpandoObject RequestParameters { get; set; }

        public ExpandoObject RequestHeaders { get; set; }

        public HttpMethod HttpMethod = HttpMethod.Get;

        public Dictionary<string, object> Predicates { get; set; } = new Dictionary<string, object>();
        
        public HttpRequestMessage GetRequest(ExtensionPayload payload) =>
            new HttpRequestMessage()
            {
                RequestUri = new UriBuilder(BaseUrl)
                {
                    Query = string.Join('&', RequestParameters.Select(e =>
                        $"{e.Key}={payload.GetValue(e.Value?.ToString())}"))
                }.Uri,
            };

        public bool Enabled(ExtensionPayload payload)
        {
            return Predicates.All(e => payload.GetValue(e.Key) == e.Value);
        }

    }

    public class TypedDynamicJson : DynamicObject
    {
        private readonly IDictionary<string, object> _typedProperty;

        public TypedDynamicJson()
        {
            _typedProperty = new Dictionary<string, object>();
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            object typedObj;

            if (_typedProperty.TryGetValue(binder.Name, out typedObj))
            {
                result = typedObj;

                return true;
            }

            result = null;

            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _typedProperty[binder.Name] = value;

            return true;
        }


        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _typedProperty.Keys;
        }
    }
}