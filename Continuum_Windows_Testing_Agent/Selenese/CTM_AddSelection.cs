using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace Selenium.Internal.SeleniumEmulation
{
    
    /// <summary>
    /// Defines the command for the addSelection keyword.
    /// </summary>
    internal class CTM_AddSelection : SeleneseCommand
    {
        private ElementFinder finder;

        public CTM_AddSelection(ElementFinder finder)
        {
            this.finder = finder;
        }

        private IWebElement findOptimized(IWebDriver driver, String locator, String value)
        {
            try
            {
                if (value.StartsWith("label="))
                {
                    value = value.Replace("label=", "");
                }
                String xByNamePath = "//select[@id='" + locator + "' or @name='" + locator + "']/option[normalize-space(.)='" + value + "']";
                IWebElement option = driver.FindElement(By.XPath(xByNamePath));
                return option;
            }
            catch
            {
                return null;
            }
        }

        private IWebElement findByLabel(IWebDriver driver, String locator, String value)
        {
            try
            {
                if (value.StartsWith("label="))
                {
                    value = value.Replace("label=", "");
                }

                String xByNamePath = "//select[@name='" + locator + "']/option[normalize-space(.)='" + value + "']";
                IWebElement option = driver.FindElement(By.XPath(xByNamePath));
                return option;
            }
            catch
            {
                return null;
            }
        }

        private IWebElement findById(IWebDriver driver, String locator, String value)
        {
            try
            {
                if (value.StartsWith("id="))
                {
                    value = value.Replace("id=", "");
                }

                String xByNamePath = "//select[@id='" + locator + "']/option[normalize-space(.)='" + value + "']";
                IWebElement option = driver.FindElement(By.XPath(xByNamePath));
                return option;
            }
            catch
            {
                return null;
            }
        }

        protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
        {
            IWebElement option = null;

            if (option == null)
            {
                option = this.findOptimized(driver, locator, value);
            }

            /*
            if (option == null)
            {
                option = this.findByLabel(driver, locator, value);
            }

            if (option == null)
            {
                option = this.findById(driver, locator, value);
            }
            */

            if (option == null)
            {
                throw new Exception("Failed to find selectbox option");
            }

            option.Select();

            return null;
        }
    }
}
