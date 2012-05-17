using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using Two10.Swt;
using System.Configuration;

namespace CloudElite.ServiceModel.Dispatcher
{
    /// <summary>
    /// Custom ClientMessageInspecotr for WCF Client
    /// This message inspector will inject custom SOAP Header to  
    /// the outgoing messages. This header will contain a WRAP SWT token
    /// </summary>
    public class TokenHeaderInspector : IClientMessageInspector 
    {
        private SwtHelper _tokenHelper = 
            new SwtHelper(ConfigurationManager.AppSettings["AcsSignKey"],
                ConfigurationManager.AppSettings["WrapEndpoint"]);
        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            Console.WriteLine("IClientMessageInspector.AfterReceiveReply called.");
            Console.WriteLine("Message: {0}", reply.ToString());
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            Console.WriteLine("IClientMessageInspector.BeforeSendRequest called.");
            SimpleWebToken token = _tokenHelper.GetToken(
            ConfigurationManager.AppSettings["SiUsername"],
            ConfigurationManager.AppSettings["SiPassword"],
            ConfigurationManager.AppSettings["SiAudience"]);
            request.Headers.Add(
                MessageHeader.CreateHeader(
                    "Token", "http://servicelab.org/tokenheader",
                    string.Format("WRAP access_token=\"{0}\"", token.ToUrlEncodedString())
                    )
                );
            return null;
        }

    }


    /// <summary>
    /// Custom Endpoint Behavior, which will add our TokenHeaderInspector to the MessageInspectors
    /// collection of the client runtime. 
    /// </summary>
    public class MessageInspectBehavior : IEndpointBehavior
    {
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            return;
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new TokenHeaderInspector());
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            return;
        }

        public void Validate(ServiceEndpoint endpoint) 
        { 
            return; 
        }

    }
}
