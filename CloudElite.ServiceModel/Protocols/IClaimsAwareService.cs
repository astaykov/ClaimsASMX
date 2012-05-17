using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Claims;

namespace CloudElite.ServiceModel.Protocols
{
    /// <summary>
    /// This is base Interface for ClaimsAwareService
    /// Use Implement this interface in all your ASMX Web Service, to get the full power
    /// of Claims. The <see cref="T:CloudElite.ServiceModel.Protocols.TokenExtension"/> will inject ClaimsPricipal property into the Service
    /// for every service call.
    /// </summary>
    public interface IClaimsAwareService
    {
        IClaimsPrincipal CurrentPricipal { get; }
        void SetClaimsPricipal(IClaimsPrincipal principal);
    }
}
