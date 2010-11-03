using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Selenium.Internal.SeleniumEmulation;

namespace Continuum_Windows_Testing_Agent.Selenese
{
    class CTM_Store : SeleneseCommand
    {
        private Selenium_Test_Suite_Variables testVariables;

        public CTM_Store(Selenium_Test_Suite_Variables testVariables )
        {
            this.testVariables = testVariables;
        }

        protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
        {
            this.testVariables.consumeCommand(value, locator);
            return null;
        }

    }
}
