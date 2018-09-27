using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterDataService.Models
{
    public class TwoFactor
    {
        public string UserName { get; set; }
        public string LoginToken { get; set; }
        public string SecretToken { get; set; }
    }
}