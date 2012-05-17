using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;

namespace CloudElite.ServiceModel.Protocols
{
    /// <summary>
    /// Custom SOAP Header class. We will use this class in classic ASMX Client to inject 
    /// our custom WRAP authentication header in the form of SWT token. This class is used with the
    /// WSDL clients.
    /// </summary>
    public class SwtSoapHeader : SoapHeader
    {

        /// <summary>
        /// Gets or sets the SWT token.
        /// </summary>
        /// <value>
        /// The SWT token.
        /// </value>
        public string SwtToken { get; set; }
    }
}
