using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using CloudElite.ServiceModel.Protocols;
using Microsoft.IdentityModel.Claims;

namespace ServiceApp
{
    /// <summary>
    /// Demo ASMX WebService, which implements IClaimsAwareService, so will have
    /// an IClaimsPricipal injected for every service call. Use the IClaimsPrincipal
    /// to perform additional checks during each method call
    /// </summary>
    [WebService(Namespace = "http://labservice.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class LabService : System.Web.Services.WebService, IClaimsAwareService
    {
        public LabService()
        {
            System.Diagnostics.Debug.WriteLine("Constructor called");
        }

        [WebMethod]
        public string HelloWorld()
        {
            System.Diagnostics.Debug.WriteLine("Calling HelloWorld");
            return "Hello " + User.Identity.Name;
        }

        [WebMethod]
        public void DummyCall()
        {
            System.Diagnostics.Debug.WriteLine("Calling DummyCall");
        }

        #region IClaimsAwareService implementation
        public IClaimsPrincipal CurrentPricipal
        {
            get;
            private set;
        }

        public void SetClaimsPricipal(IClaimsPrincipal principal)
        {
            this.CurrentPricipal = principal;
        }
        #endregion


    }
}
