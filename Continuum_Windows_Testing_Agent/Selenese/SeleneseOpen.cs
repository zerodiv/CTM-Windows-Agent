using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using System.Collections.Specialized;
using System.Web;

namespace Continuum_Windows_Testing_Agent.Selenese
{
    class SeleneseOpen : Selenese_Command
    {
        private Uri baseUri;
        private NameValueCollection baseParams;

        public SeleneseOpen(Selenium_Test_Log log, IWebDriver webDriver)
            : base(log, webDriver)
        {
            this.baseUri = null;
            this.baseParams = new NameValueCollection();
        }

        private String parseUri(string rawUrl)
        {
            if (rawUrl.StartsWith("http:") == false && rawUrl.StartsWith("http:") == false)
            {
                // fragment url add in our existing baseurl params.
                String tUrl = this.baseUri.Scheme + "://" + this.baseUri.Host;
                if (this.baseUri.IsDefaultPort == false)
                {
                    tUrl = tUrl + ":" + this.baseUri.Port;
                }
                tUrl = tUrl + rawUrl;
                rawUrl = tUrl;
            }

            Uri uri = new Uri(rawUrl);

            if (uri.Query != "")
            {
                // We have parameters.
                NameValueCollection cBaseParams = HttpUtility.ParseQueryString(uri.Query);
                foreach (String key in cBaseParams.Keys)
                {
                    this.baseParams[key] = cBaseParams[key];
                }

            }

            // rebuild the params.
            String uParams = "";
            foreach (String key in this.baseParams)
            {
                if (uParams != "")
                {
                    uParams = uParams + "&";
                }
                uParams = HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(this.baseParams[key]);
            }

            // make a url out of the new data.
            String nUrl = "";

            nUrl = uri.Scheme + "://" + uri.Host;

            if (uri.IsDefaultPort == false)
            {
                nUrl = nUrl + ":" + uri.Port;
            }


            nUrl = nUrl + uri.AbsolutePath;

            if (uParams != "")
            {
                nUrl = nUrl + "?" + uParams;
            }

            // Save the new url to the stack
            this.baseUri = new Uri(nUrl);

            return nUrl;

        }

        public override Boolean run(Selenium_Test_Trinome testCommand)
        {
            this.log.startTimer();
            try
            {
                this.webDriver.Navigate().GoToUrl(this.parseUri(testCommand.target));
                this.log.logSuccess(testCommand, "");
                return true;
            }
            catch (Exception e)
            {
                this.log.logFailure(testCommand, e.Message);
            }
            return false;
        }


    }
}
