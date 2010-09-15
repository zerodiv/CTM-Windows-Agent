using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Continuum_Windows_Testing_Agent.Selenese
{
    class SeleneseStore : Selenese_Command
    {
        private Selenium_Test_Suite_Variables testVariables;

        public SeleneseStore(Selenium_Test_Log log, IWebDriver webDriver, Selenium_Test_Suite_Variables testVariables )
            : base(log, webDriver)
        {
            this.testVariables = testVariables;
        }

        public override Boolean run(Selenium_Test_Trinome testCommand)
        {
            this.log.startTimer();
            try
            {
                // If the value is javascript fix up the target / values.
                testCommand.target = this.runJavascriptValue(testCommand.target);

                this.testVariables.consumeTrinome(testCommand);
                this.log.stopTimer();
                this.log.logSuccess(testCommand, "");
                return true;
            }
            catch (Exception e)
            {
                this.log.stopTimer();
                this.log.logFailure(testCommand, e.Message);
            }
            return false;
        }

    }
}
