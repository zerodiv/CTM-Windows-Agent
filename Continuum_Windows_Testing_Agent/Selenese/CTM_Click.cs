using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using Continuum_Windows_Testing_Agent;

namespace Selenium.Internal.SeleniumEmulation
{
    /// <summary>
    /// Defines the command for the click keyword.
    /// </summary>
    internal class CTM_Click : SeleneseCommand
    {
        private ElementFinder finder;

        public CTM_Click(ElementFinder finder)
        {
            this.finder = finder;
        }

        protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
        {
            IWebElement element = finder.FindElement(driver, locator);
            element.Click();

            // JEO: To emulate the IDE's load behavior you need to do a waitforpagetoload 30s
            PageLoadWaiter pageWaiter = new PageLoadWaiter(driver, 30000);
            pageWaiter.Wait("Page load timeout exceeded");

            return null;
        }
    }
}
