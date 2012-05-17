using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using Two10.Swt;
using System.ServiceModel.Channels;
using System.Configuration;

namespace CloudElite.ServiceModel.Protocols
{
    /// <summary>
    /// This is custom SOAP Extension class that processes messages on the Server only!
    /// You need to have the following AppSetting configured in your web.config to make this work correctly:
    ///  - AcsSignKey: this will be the value of the Symmetric Key used in Azure ACS to sign the SWT tokens
    ///  - SiAudience: this will be the Relying party application configured for the used Service Identities,
    ///  and of course matching the currently running application
    /// </summary>
    public class TokenExtension : SoapExtension
    {
        private SwtHelper _tokenHelper =
            new SwtHelper(ConfigurationManager.AppSettings["AcsSignKey"],
                ConfigurationManager.AppSettings["WrapEndpoint"]);

        #region Initializers - currently not used
        public override object GetInitializer(Type serviceType)
        {
            return null;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {
            return;
        }
        #endregion

        /// <summary>
        /// Process the soap message and try validate calling party authentication
        /// by examing the message headers. We are searching for custom message header 
        /// containing a WRAP authentication token.
        /// This method is automatically called by the ASMX WebService infrastructure
        /// </summary>
        /// <param name="message">The <see cref="T:System.Web.Services.Protocols.SoapMessage"/> to process.</param>
        public override void ProcessMessage(SoapMessage message)
        {
            if (message.Stage == SoapMessageStage.AfterDeserialize && message is SoapServerMessage)
            {
                // we need to catch the message after it has come from the client
                // and has been deserialized
                if (message.Headers.Count < 1)
                {
                    // the number of message headers is less than 1, for sure there is no authentication
                    throw new ApplicationException("You are not authorized to use this resource");
                }
                
                foreach (SoapHeader header in message.Headers)
                {
                    if (header is SoapUnknownHeader)
                    {
                        // The custom header sent will be unknown header.
                        // Get the token value from the InnerText proprety of the header
                        string tokenHeaderValue = ((SoapUnknownHeader)header).Element.InnerText;
                        
                        // Parse the token string to real token instance
                        SimpleWebToken token = SimpleWebToken.Parse(tokenHeaderValue);

                        // Check token validity - checks token Signature, Expire date
                        // and, because included as parameter - the token Audience
                        SimpleWebTokenValidationResult result = 
                            SimpleWebToken.Validate(token, this._tokenHelper.ACSSigningKey,
                            expectedAudience: ConfigurationManager.AppSettings["SiAudience"]);
                        if (result == SimpleWebTokenValidationResult.Valid)
                        {
                            var server = ((SoapServerMessage)message).Server;
                            if (server is IClaimsAwareService)
                            {
                                // inject the ClaimsPrincipal to the instance of the service
                                // thus the method can perform additional checks
                                ((IClaimsAwareService)server).SetClaimsPricipal(token.ToPrinciple());
                            }

                            foreach (var claim in token.Claims.AllKeys)
                            {
                                // debugging purposes only - printout the claims collection
                                System.Diagnostics.Debug.WriteLine(string.Format("{0} => {1} ", claim, token.Claims[claim]));
                            }
                        }
                        else
                        {
                            throw new ApplicationException(string.Format("Authentication failed: {0}",result.ToString()));
                        }
                        
                    }
                }
            }

        }
    }
}
