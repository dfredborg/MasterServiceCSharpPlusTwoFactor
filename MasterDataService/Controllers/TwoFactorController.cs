using MasterDataService.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MasterDataService.Controllers
{
    public class TwoFactorController : ApiController
    {
        public HttpResponseMessage GetToken(string token)
        {
            TwoFactorService.TwoFactor_Service service = new TwoFactorService.TwoFactor_Service();
            service.Url = @"http://localhost:7047/DynamicsNAV110/WS/CRONUS%20Danmark%20A%2FS/Page/TwoFactor";
            NetworkCredential networkCredential = new NetworkCredential(@"DESKTOP-AOPOAEF\DENNIS", "070886Denz");
            //service.UseDefaultCredentials = true;
            service.Credentials = networkCredential;
            List<TwoFactorService.TwoFactor_Filter> filterArray = new List<TwoFactorService.TwoFactor_Filter>();
            TwoFactorService.TwoFactor_Filter tokenFilter = new TwoFactorService.TwoFactor_Filter();
            tokenFilter.Field = TwoFactorService.TwoFactor_Fields.Secret_Token;
            tokenFilter.Criteria = token;
            filterArray.Add(tokenFilter);

            TwoFactorService.TwoFactor[] list = service.ReadMultiple(filterArray.ToArray(), null, 1);
            if (list.Count() == 0)
            {
                HttpResponseMessage response2 = Request.CreateResponse(HttpStatusCode.NotFound);
                return response2;
            }
            TwoFactor twoFactor = new TwoFactor();
            Random random = new Random();
            int tempToken = random.Next(10000,88888) + DateTime.Now.Second;
            string secretToken = tempToken.ToString();
            list[0].Login_Token = secretToken;
            service.Update(ref list[0]);
            twoFactor.UserName = list[0].User_ID;
            twoFactor.SecretToken = list[0].Secret_Token;
            twoFactor.LoginToken = list[0].Login_Token;
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(twoFactor), Encoding.UTF8, "application/json");
            return response;
        }
    }
}
