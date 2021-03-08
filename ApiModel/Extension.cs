using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        public string Id { get; set; }
        
         public HttpCall[] HttpCalls { get; set; }
        
         public int DelayMs { get; set; }

          public dynamic Result { get; set; }
        
        
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
        public  ExpandoObject RequestParameters { get; set; }

        public dynamic RequestHeaders { get; set; }

    }
    
    public class TypedDynamicJson  : DynamicObject
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
            _typedProperty[binder.Name] =  value;
 
            return true;
        }

         

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _typedProperty.Keys;
        }
        
        
    }
}

