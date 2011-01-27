using System;
using OpenQA.Selenium;

namespace Selenium
{
    internal class LinkLookupStrategy : ILookupStrategy
    {
        public IWebElement Find(IWebDriver driver, string use)
        {
            // Testing out issue with By.LinkText with SE 2.0b1
            //select[@id='" + locator + "' or @name='" + locator + "']/option[normalize-space(.)='" + value + "']"
            return driver.FindElement(By.XPath("//a[normalize-space(.)='" + use + "']"));

            // return driver.FindElement(By.LinkText(use));
        }
    }
}
