
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
                return new IdentifierLookupStrategy().Find(driver, use);
            }
        }
    }
}