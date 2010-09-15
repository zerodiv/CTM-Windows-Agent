using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Continuum_Windows_Testing_Agent.Selenese
{
    class SeleneseWaitForPageToLoad : Selenese_Command
    {
        public SeleneseWaitForPageToLoad(Selenium_Test_Log log, IWebDriver webDriver)
            : base(log, webDriver)
        {
        }

        public override Boolean run(Selenium_Test_Trinome testCommand)
        {
            this.log.logSuccess(testCommand, "");
            return true;
        }
    }
}
