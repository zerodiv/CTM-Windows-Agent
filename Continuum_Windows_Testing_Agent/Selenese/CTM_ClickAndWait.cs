using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using Continuum_Windows_Testing_Agent;
// using OpenQA.Selenium.IE;
using System.Threading;

// JEO: ClickAndWait support via our own code.
namespace Selenium.Internal.SeleniumEmulation
{
    /// <summary>
    /// Defines the command for the click keyword.
    /// </summary>
    internal class CTM_ClickAndWait : SeleneseCommand
    {
        private ElementFinder finder;

        public CTM_ClickAndWait(ElementFinder finder)
        {
            this.finder = finder;
        }

        protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
        {

            IWebElement element = finder.FindElement(driver, locator);

            element.Click();

            return null;
        }
    }
}
