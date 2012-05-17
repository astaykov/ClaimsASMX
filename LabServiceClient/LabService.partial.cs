using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudElite.ServiceModel.Protocols;
using Two10.Swt;
using System.Configuration;


public partial class LabService : System.Web.Services.Protocols.SoapHttpClientProtocol
{
    public SwtSoapHeader CustomHeader;
    protected CloudElite.SwtHelper _tokenHelper
        = new CloudElite.SwtHelper(ConfigurationManager.AppSettings["AcsSignKey"],
            ConfigurationManager.AppSettings["WrapEndpoint"]);

    /// <remarks/>
    public LabService()
    {
        this.Url = "http://localhost:41210/LabService.asmx";
        SimpleWebToken token = _tokenHelper.GetToken(
            ConfigurationManager.AppSettings["SiUsername"],
            ConfigurationManager.AppSettings["SiPassword"],
            ConfigurationManager.AppSettings["SiAudience"]);
        SwtSoapHeader hdr = new SwtSoapHeader();
        hdr.SwtToken = string.Format("WRAP access_token=\"{0}\"", token.ToUrlEncodedString());
        this.CustomHeader = hdr;
    }
}
