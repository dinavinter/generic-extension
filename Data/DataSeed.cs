using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace GenericExtesion
{
    public static class DataSeed
    {
        public static void EnsureSeed(this ExtensionDbContext context)
        {
            foreach (var extensionModel in GetSeed())
            {
                context.UpsertJsonAsync(extensionModel, extensionModel.Id);
            }

            context.SaveChanges();
        }
        internal class FirstLower : JsonNamingPolicy
        {
            public override string ConvertName(string name) => name.Substring(0, 1).ToLower()+name.Substring(1, name.Length-1);
        }
        public static IEnumerable<ExtensionModel> GetSeed()
        {
            return JsonSerializer.Deserialize<ExtensionModel[]>(File.ReadAllText(".seed/extensions.json"), new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = false,
                PropertyNamingPolicy = new FirstLower(),
                AllowTrailingCommas = true
            });
            
            
            // var model = new ExtensionModel
            // {
            //     Id = "set_require_password_change",
            //     Result = new ExpandoObject(),
            //     DelayMs = 1000,
            //     HttpCalls = new[]
            //     {
            //         new HttpCall()
            //         {
            //             RequestParameters = new ExpandoObject(),
            //             RequestHeaders = new ExpandoObject()
            //         }
            //     },
            // };
            //
            // model.Result.Status = "FAIL";
            // model.Result.Data = new {userFacingErrorMessage = "You Suck!"};
            // model.HttpCalls[0].RequestParameters.uid = "${data.accountInfo.uid}";
            // model.HttpCalls[0].RequestParameters.requirePasswordChange = true;
            // model.HttpCalls[0].RequestParameters.client_id = "${apiKey}";
            // model.HttpCalls[0].RequestParameters.oauth_token =
            //     "${st2.s.AcbHpnRbvw.w6EnvRmwGWfH102lvUx_dc2NwrHikBJS4EJ7h2_PQ-uN2PiCi4UKwKdcGOTGRFHKe-eGr1I3Ih-W3IdC01Iixg.9t3Yff8nzk5ivDDTW7EQMTBBi_4PWnondFvxf4uz0OHTnrsdq_RiQTRpwD7oVuQyfX7n6YFtDEk1enzlKq2fzw.sc3}";
            // model.HttpCalls[0].RequestHeaders.XGigyaTestName = "requirePasswordChange on login extension";
            // yield return model;
        }
    }
}