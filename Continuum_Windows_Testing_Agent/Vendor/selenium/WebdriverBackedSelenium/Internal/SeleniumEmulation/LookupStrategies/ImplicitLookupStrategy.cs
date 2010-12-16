
using System;
using OpenQA.Selenium;

namespace Selenium
{
    internal class ImplicitLookupStrategy : ILookupStrategy
    {
        public IWebElement Find(IWebDriver driver, string use)
        {
            // JEO - we need to lookup by name, then id to support selenese 1.x editors
            try
            {
                return new NameLookupStrategy().Find(driver, use);
            }
            catch
            {
            }

            try
            {
                return new IdentifierLookupStrategy().Find(driver, use);
            }
            catch
            {
            }

            // JEO: If neither of these match up, try a wild card matcher against both methods, but do it sanely via xpath.
            try
            {
                String byNameXpath = "//*[contains(@name,'" + use + "')]";
                return new XPathLookupStrategy().Find(driver, byNameXpath);
            }
            catch
            {
            }

            // Finally try it by id.
            String byIdXpath = "//*[contains(@id,'" + use + "')]";
            return new XPathLookupStrategy().Find(driver, byIdXpath);

        }
    }
}