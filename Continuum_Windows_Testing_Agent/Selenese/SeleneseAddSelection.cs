using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Continuum_Windows_Testing_Agent
{
    class SeleneseAddSelection : SeleneseSelect
    {
        public SeleneseAddSelection(Selenium_Test_Log log, IWebDriver webDriver)
            : base(log, webDriver)
        {
        }
    }
}
