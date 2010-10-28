using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Continuum_Windows_Testing_Agent.Selenese
{
    class SelenesePause : Selenese_Command
    {
        public SelenesePause(Selenium_Test_Log log, IWebDriver webDriver)
            : base(log, webDriver)
        {
        }

        public override Boolean run(Selenium_Test_Trinome testCommand)
        {
            this.log.startTimer();
            int testWait = 3000;

            if (testCommand.getValue() != "")
            {
                testWait = Convert.ToInt32(testCommand.getValue());
            }

            if (testWait > 0)
            {
                System.Threading.Thread.Sleep(testWait);
            }

            this.log.logSuccess(testCommand, "waited: " + testWait);
            return true;
        }
    }
}
