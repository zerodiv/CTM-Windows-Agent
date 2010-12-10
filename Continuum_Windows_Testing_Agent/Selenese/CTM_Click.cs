using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
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
            try
            {
                IWebElement element = finder.FindElement(driver, locator);

                element.Click();

                if (driver.GetType().Name == "InternetExplorerDriver")
                {
                    ((InternetExplorerDriver)driver).WaitForLoadToComplete();
                }

            }
            catch
            {
                // IE Emits errornus click errors at times. We have suprressed the exceptions here.
            }

            return null;

        }
    }
}
