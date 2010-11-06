using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using Selenium.Internal.SeleniumEmulation;
using System.Collections.Specialized;
using System.Web;
using Continuum_Windows_Testing_Agent;

namespace Selenium.Internal.SeleniumEmulation
{
    internal class CTM_Open : SeleneseCommand
    {
        private Uri baseUri;
        private String currentUrl;
        private NameValueCollection baseParams;

        public CTM_Open()
        {
            this.baseUri = null;
            this.currentUrl = null;
            this.baseParams = new NameValueCollection();
        }

        protected override object HandleSeleneseCommand(IWebDriver driver, string url, string ignored)
        {
            String uUrl = this.createUrl(url);

            this.currentUrl = uUrl;
            driver.Navigate().GoToUrl(uUrl);
             
            // JEO: To emulate the IDE's load behavior you need to do a waitforpagetoload 30s
            PageLoadWaiter pageWaiter = new PageLoadWaiter(driver, 30000);
            pageWaiter.Wait("Page load timeout exceeded");

            return null;
        }


        private String createUrl(string rawUrl)
        {

            if (rawUrl.StartsWith("http:") == false && rawUrl.StartsWith("https:") == false)
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

    }
}
