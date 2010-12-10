using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Selenium.Internal.SeleniumEmulation;

namespace Continuum_Windows_Testing_Agent
{
    internal class CTM_PageLoadWaiter : Waiter
    {
        private IWebDriver driver;
        private int timeToWaitAfterPageLoad;
        private DateTime started = DateTime.Now;

        public CTM_PageLoadWaiter(IWebDriver driver, int timeToWaitAfterPageLoad)
            : base()
        {
            this.driver = driver;

            
            this.timeToWaitAfterPageLoad = timeToWaitAfterPageLoad;
        }

        public override bool Until()
        {
            try
            {
                object result = ((IJavaScriptExecutor)driver).ExecuteScript("return document['readyState'] ? 'complete' == document.readyState : true");

                DateTime now = DateTime.Now;

                // JEO: The logic here is odd if the page is loaded return true since it's done otherwise wait until the 
                // return happens.
                if (result != null && result is bool && (bool)result) {
                    double foo = now.Subtract(started).TotalMilliseconds;
                    if ((bool) result == true)
                    {
                        return true;
                    }
                    if (now.Subtract(started).TotalMilliseconds > timeToWaitAfterPageLoad)
                    {
                        return true;
                    }
                }
                else
                {
                    started = now;
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
