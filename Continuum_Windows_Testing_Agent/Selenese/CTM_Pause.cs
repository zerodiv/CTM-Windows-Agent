using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using Selenium.Internal.SeleniumEmulation;

namespace Continuum_Windows_Testing_Agent.Selenese
{
    class CTM_Pause : SeleneseCommand
    {
        public CTM_Pause()
        {
        }

        protected override object HandleSeleneseCommand(IWebDriver driver, string value, string ignore )
        {
            int testWait = 3000;

            if (value != "")
            {
                testWait = Convert.ToInt32(value);
            }

            if (testWait > 0)
            {
                System.Threading.Thread.Sleep(testWait);
            }

            return true;
        }
    }
}
