using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Continuum_Windows_Testing_Agent.Selenese
{
    class SeleneseVerifyTextPresent : SeleneseAssertTextPresent
    {
        public SeleneseVerifyTextPresent(Selenium_Test_Log log, IWebDriver webDriver)
            : base(log, webDriver)
        {
        }
    }
}
