This is sample project that shows how to secure plain old ASMX WebServices with Claims,
more specifically with Simple Web Tokens.
There are 3 projects in the solution:
1. ServiceApp - this is a WebApplication that exposes ASMX WebService, which is secured based on SWT
2. CloudElite.ServiceModel - s class library that has all neccessary helper classes and extensions
to help securing the server, and geting authentication token on the client
3. LabServiceClient - a WinForms application, which uses both WCF client and WSDL (old WebServices) client
to demonstrate how can you call SWT secured WebService with either clients.

The security infrastructure relies on Two10.Swt implementaiton for SimpleWebTokens. You can check the full
SWT implmentation at Rob Blackwell's GitHub account: https://github.com/RobBlackwell/webtokens

All security related stuff is configured in web.config for the webapp and app.config for the WinForms app.
Required changes in app.config for WinForms are following:
  <appSettings>
    <add key="AcsSignKey" value="[YOUR ACS SIGNING KEY HERE]" />
    <add key="SiUsername" value="[YOUR Service Identity user name here]" />
    <add key="SiPassword" value="[YOUR Service Identity password here]" />
    <add key="SiAudience" value="[YOUR Relying party application URL here]"/>
  </appSettings>

Required changes for web.config on the server:
  <appSettings>
    <add key="AcsSignKey" value="[YOUR ACS SIGNING KEY HERE]" />
    <add key="SiAudience" value="[YOUR Relying party application URL here]"/>
  </appSettings>
  ...
  <webServices>
      <!-- To fully secure the service, let it listen to only HttpSoap protocol! -->
      <protocols>
        <clear />
        <add name="HttpSoap"/>
      </protocols>
      <soapExtensionTypes>
        <!-- Add custom SOAP Extension that will perform Token validation and injection tasks -->
        <add type="CloudElite.ServiceModel.Protocols.TokenExtension,
              CloudElite.ServiceModel" priority="1"/>
      </soapExtensionTypes>
    </webServices>