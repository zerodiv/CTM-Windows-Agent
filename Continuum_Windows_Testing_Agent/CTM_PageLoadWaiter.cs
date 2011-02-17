using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Selenium.Internal.SeleniumEmulation;

namespace Continuum_Windows_Testing_Agent
{
    internal class CTM_PageLoadWaiter
    {
        private IWebDriver driver;
        private int waitTimeout;
        private DateTime started = DateTime.Now;
        private Boolean findjQueryRan;
        private Boolean hasjQuery;

        public CTM_PageLoadWaiter(IWebDriver driver, int waitTimeout)
        {
            this.driver = driver;
            this.findjQueryRan = false;
            this.hasjQuery = false;
            this.waitTimeout = waitTimeout;

            this.waitForLoad();

        }

        private double waitedSeconds(DateTime startTime, DateTime endTime)
        {
            TimeSpan elap = endTime.Subtract(startTime);
            return elap.TotalSeconds;
        }

        private Boolean findjQuery()
        {

            if (this.findjQueryRan == true)
            {
                return false;
            }

            this.findjQueryRan = true;

            this.hasjQuery = false;

            object result = ((IJavaScriptExecutor)driver).ExecuteScript("if (typeof jQuery == 'function') { return true; } else { return false; }");

            if (result != null && result is bool && (bool)result)
            {
                this.hasjQuery = true;
            }

            return true;

        }

        private void waitForLoad()
        {
            DateTime startTime = DateTime.Now;
            DateTime stopTime = DateTime.Now;

            double waitedSeconds = this.waitedSeconds(startTime, stopTime);

            while (waitedSeconds < waitTimeout)
            {

                stopTime = System.DateTime.UtcNow;
                waitedSeconds = this.waitedSeconds(startTime, stopTime);

                if (this.isPageLoaded() == true)
                {
                    return;
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                }

            }

            throw new Exception("Page failed to load within: " + this.waitTimeout + " waited: " + waitedSeconds);

        }

        private bool isPageLoaded()
        {

            try
            {

                if (this.findjQuery() == true)
                {

                    StringBuilder ctmJs = new StringBuilder();
                    ctmJs.AppendLine("window.ctmPageLoaded = false;");

                    // Okay this is our first pass and if we have jquery we need to bind a on load event
                    if (this.hasjQuery == true)
                    {
                        ctmJs.AppendLine("var ctmjQuery = jQuery.noConflict();");
                        ctmJs.AppendLine("ctmjQuery(document).ready(function() {");
                        ctmJs.AppendLine("   window.ctmPageLoaded = true;");
                        ctmJs.AppendLine("});");

                    }
                    else
                    {

                    }

                    String js = ctmJs.ToString();

                    ((IJavaScriptExecutor)driver).ExecuteScript(js);
                }
            }
            catch
            {
            }

            try
            {

                if (this.hasjQuery == false)
                {
                    // We don't have jquery so every time this loops the determiniation code is ran (boo).
                    StringBuilder ctmJs = new StringBuilder();
                    ctmJs.AppendLine("if ( document['readyState'] == 'complete' ) {");
                    ctmJs.AppendLine("   window.ctmPageLoaded = true;");
                    ctmJs.AppendLine("}");

                    ctmJs.AppendLine("if ( document.readyState == 'complete' ) {");
                    ctmJs.AppendLine("   window.ctmPageLoaded = true;");
                    ctmJs.AppendLine("}");

                    ((IJavaScriptExecutor)driver).ExecuteScript(ctmJs.ToString());
                }

                // object result = ((IJavaScriptExecutor)driver).ExecuteScript("return document['readyState'] ? 'complete' == document.readyState : true");
                object result = ((IJavaScriptExecutor)driver).ExecuteScript("return window.ctmPageLoaded;");

                // return happens.
                if (result != null && result is bool && (bool)result == true)
                {
                    return true;
                }

            }
            catch (Exception)
            {
                // Possible page reload. Fine
            }

            return false;
        }
    }
}
