using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Continuum_Windows_Testing_Agent
{
    class Selenese_Locator
    {
        private Selenium_Test_Log log;
        private IWebDriver webDriver;

        public Selenese_Locator(Selenium_Test_Log log, IWebDriver webDriver)
        {
            this.log = log;
            this.webDriver = webDriver;
        }

        public By convert(String locator)
        {
            // TODO: locate by DOM, if it is even needed.

            if (locator.StartsWith("css="))
            {
                locator = locator.Replace("css=", "");
                return By.CssSelector(locator);
            }

            if (locator.StartsWith("class="))
            {
                locator = locator.Replace("class=", "");
                return By.ClassName(locator);
            }

            if (locator.StartsWith("link="))
            {
                locator = locator.Replace("link=", "");
                return By.LinkText(locator);
            }
            if (locator.StartsWith("//"))
            {
                return By.XPath(locator);
            }
            if (locator.StartsWith("xpath="))
            {
                locator = locator.Replace("xpath=", "");
                return By.XPath(locator);
            }
            if (locator.StartsWith("name="))
            {
                locator = locator.Replace("name=", "");
                return By.Name(locator);
            }
            if (locator.StartsWith("id="))
            {
                locator = locator.Replace("id=", "");
                if (locator.Contains("__"))
                {
                    // JEO: There is a bug with by id finder where it won't find __ valued items.
                    //html/body/form/*[@id='byId__Sel']
                    String byIdXpath = "//*[@id='" + locator + "']";
                    return By.XPath(byIdXpath);
                }
                return By.Id(locator);
            }
            if (locator.StartsWith("identifier="))
            {
                locator = locator.Replace("identifier=", "");
                return By.Id(locator);
            }

            try
            {
                this.log.message("legacy byName start");
                By byName = By.Name(locator);
                IWebElement target = this.webDriver.FindElement(byName);
                this.log.message("legacy byName end");
                return byName;
            }
            catch
            {
                this.log.message("legacy byName end - failover to id");
                // okay so by name failed, try id, otherwise they are fucked =)
                return By.Id(locator);
            }


        }
    }
}
