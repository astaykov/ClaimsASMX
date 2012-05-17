using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Two10.Swt;

namespace CloudElite
{
    public class SwtHelper
    {
        public SwtHelper(string acsSignKey, string wrapEndPoint)
        {
            ACSSigningKey = acsSignKey;
            WrapEndpoint = wrapEndPoint;
        }

        public SimpleWebToken GetToken(string userName, string password, string scope)
        {
            var swt = SimpleWebToken.GetToken(this.WrapEndpoint, userName, password, scope);
            if(!swt.CheckSignature(this.ACSSigningKey))
            {
                throw new ApplicationException("The token was not valid!");
            }
            return swt;
        }

        public string ACSSigningKey { get; set; }
        public string WrapEndpoint { get; set; }
    }
}
