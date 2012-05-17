#region Copyright (c) 2012 Two10 degrees
//
// (C) Copyright 2012 Two10 degrees
//      All rights reserved.
//
// This software is provided "as is" without warranty of any kind,
// express or implied, including but not limited to warranties as to
// quality and fitness for a particular purpose. Two10 degrees
// does not support the Software, nor does it warrant that the Software
// will meet your requirements or that the operation of the Software will
// be uninterrupted or error free or that any defects will be
// corrected. Nothing in this statement is intended to limit or exclude
// any liability for personal injury or death caused by the negligence of
// Two10 degrees, its employees, contractors or agents.
//
// Two10 degrees is a trading style of Active Web Solutions Ltd.
//
#endregion

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Two10.Swt
{
    /// <summary>
    /// See http://wiki.oauth.net/w/page/12238537/OAuth%20WRAP and http://tools.ietf.org/html/draft-hardt-oauth-01
    /// </summary>
    public class SimpleWebToken
    {
        private string issuer;
        private string audience;
        private long expiresOn;
        private NameValueCollection claims;   
        private string hmacSha256;

        public string Issuer
        {
            get { return issuer; }
        }

        public string Audience
        {
            get { return audience; }
        }

        public long ExpiresOn
        {
            get { return expiresOn; }
        }

        public NameValueCollection Claims
        {
            get { return claims; }
        }

        public string HmacSha256
        {
            get { return hmacSha256; }
        }

        public SimpleWebToken(string issuer, string audience, long expiresOn, NameValueCollection claims)
        {
            this.issuer = issuer;
            this.audience = audience;
            this.expiresOn = expiresOn;
            this.claims = claims;
        }

        public SimpleWebToken(string issuer, string audience, long expiresOn, NameValueCollection claims, string hmacSha256)
        {
            this.issuer = issuer;
            this.audience = audience;
            this.expiresOn = expiresOn;
            this.claims = claims;
            this.hmacSha256 = hmacSha256;
        }

        public static SimpleWebToken Parse(string s)
        {
            string token;

            if (s.StartsWith("WRAP access_token=\"")) 
            {
                token = HttpUtility.UrlDecode( s.Substring(19, s.Length - 19 - 1));
            } 
            else 
            {
                token = HttpUtility.UrlDecode(
                    s
                    .Split('&')
                    .Single(value => value.StartsWith("wrap_access_token=", StringComparison.OrdinalIgnoreCase))
                    .Split('=')[1]);
            }

            string[] pairs = HttpUtility.UrlDecode(token).Split('&');

            long expiresOn = 0;
            string issuer = null;
            string hmacSha256 = null;
            string audience = null;

            NameValueCollection claims = new NameValueCollection();

            foreach (string pair in pairs)
            {
                string[] fields = pair.Split(new char[] { '=' }, 2);


                switch (fields[0])
                {
                    case "ExpiresOn":
                        expiresOn = long.Parse(fields[1]);
                        break;
                    case "Issuer":
                        issuer = fields[1];
                        break;
                    case "HMACSHA256":
                        hmacSha256 = fields[1].Replace("%3","=");
                        break;
                    case "Audience":
                        audience = fields[1];
                        break;
                    default:
                        claims.Add(fields[0], fields[1]);
                        break;
                }
            }
           
            return new SimpleWebToken(issuer, audience, expiresOn, claims, hmacSha256);
        }

        public static SimpleWebToken FromHttpContext(HttpContextBase httpContext)
        {
            string authorisationHeader = httpContext.Request.Headers["Authorization"];

            return SimpleWebToken.Parse(authorisationHeader);
        }

        /// <summary>
        /// Gets a token from  WRAP endpoint (e.g. the Access Control Service) using the credentials provided.
        /// Returns s a SimpleWebToken object.
        /// </summary>
        public static SimpleWebToken GetToken(string wrapEndpoint, string wrapName, string wrapPassword, string wrapScope)
        {

            WebClient client = new WebClient();
            client.BaseAddress = wrapEndpoint;

            NameValueCollection values = new NameValueCollection();
            values.Add("wrap_name", wrapName);
            values.Add("wrap_password", wrapPassword);
            values.Add("wrap_scope", wrapScope);

            // WebClient takes care of the URL Encoding
            byte[] responseBytes = client.UploadValues("", "POST", values);

            string response = Encoding.UTF8.GetString(responseBytes);

            return SimpleWebToken.Parse(response);
        }


        /// <summary>
        /// Returns the portion of the token that is used for signing.
        /// </summary>
        public string StringToSign()
        {

            StringBuilder sb = new StringBuilder();

            foreach (string claim in claims)
            {
                
                sb.Append(HttpUtility.UrlEncode(claim) + "=" + HttpUtility.UrlEncode(claims[claim]) + '&');
            }

            sb.Append("Audience=" + HttpUtility.UrlEncode(audience) + '&');
            sb.Append("ExpiresOn=" + expiresOn + '&');
            sb.Append("Issuer=" + HttpUtility.UrlEncode(issuer) );
            
            return sb.ToString();

        }

        public string ToUrlEncodedString()
        {
            return HttpUtility.UrlEncode( StringToSign() + "&HMACSHA256=" + HttpUtility.UrlEncode( hmacSha256));
        }

        public string Sign(string signingKey)
        {
            hmacSha256 = ComputeSignature(signingKey);
            return hmacSha256;
        }

        
        public string ComputeSignature(string signingKey)
        {
            HMACSHA256 hmac256 = new HMACSHA256(Convert.FromBase64String(signingKey));
            byte[] generatedSignature = hmac256.ComputeHash(Encoding.ASCII.GetBytes(StringToSign()));
            return Convert.ToBase64String(generatedSignature);
        }

        public bool CheckSignature(string signingKey)
        {
            return this.ComputeSignature(signingKey) == hmacSha256;
        }

        public bool IsExpired()
        {
            //ulong expiresOn = Convert.ToUInt64(expiresOnValue);
            ulong currentTime = Convert.ToUInt64(GenerateTimeStamp());
            if (currentTime > (ulong)this.expiresOn)
            {
                return true;
            }
            return false;
        }

        private static ulong GenerateTimeStamp()
        {
            // Default implementation of epoch time
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToUInt64(ts.TotalSeconds);
        } 
    }
}
