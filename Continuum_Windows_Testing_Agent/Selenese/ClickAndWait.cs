using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using Continuum_Windows_Testing_Agent;
using System.Threading;

// JEO: ClickAndWait support via our own code.
namespace Selenium.Internal.SeleniumEmulation
{
    /// <summary>
    /// Defines the command for the click keyword.
    /// </summary>
    internal class ClickAndWait : SeleneseCommand
    {
        private ElementFinder finder;

        public ClickAndWait(ElementFinder finder)
        {
            this.finder = finder;
        }

        protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
        {
            IWebElement element = finder.FindElement(driver, locator);

            element.Click();

            // JEO: To emulate the IDE's load behavior you need to do a waitforpagetoload 30s
            /*
            int sleepTime = 30000;

            try
            {
                if (value != null)
                {
                    System.Int32.Parse(value);
                }
            }
            catch
            {
                sleepTime = 30000;
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(sleepTime));
            */
            
            // JEO: This is just me making sure this waits for the page to be fully loaded.
            PageLoadWaiter pageWaiter = new PageLoadWaiter(driver, 30000);
            pageWaiter.Wait("Page load timeout exceeded");

            return null;
        }
    }
}
