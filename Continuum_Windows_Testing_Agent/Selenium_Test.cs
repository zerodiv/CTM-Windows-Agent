using System;
using System.Text.RegularExpressions;

using System.Collections;
using Continuum_Windows_Testing_Agent.Selenese;
using OpenQA.Selenium;


namespace Continuum_Windows_Testing_Agent
{
    class Selenium_Test
    {
        public Boolean testHadError;
        private IWebDriver webDriver;
        private Selenium_Test_Suite_Variables testVariables;
        private Selenium_Test_Log log;
        private Hashtable seleneseCommands;

        public Selenium_Test(IWebDriver webDriver, Selenium_Test_Log log)
        {
            this.testHadError = false;
            this.webDriver = webDriver;
            this.testVariables = new Selenium_Test_Suite_Variables();
            this.log = log;

            this.seleneseCommands = new Hashtable();
            this.initSeleneseCommands();
        }

        private void initSeleneseCommands()
        {
            this.seleneseCommands.Add("addSelection", new SeleneseAddSelection(this.log, this.webDriver) );
            this.seleneseCommands.Add("assertElementPresent", new SeleneseAssertElementPresent(this.log, this.webDriver));
            this.seleneseCommands.Add("assertTextPresent",  new SeleneseAssertTextPresent(this.log, this.webDriver));
            this.seleneseCommands.Add("click", new SeleneseClick(this.log, this.webDriver));
            this.seleneseCommands.Add("clickAndWait", new SeleneseClickAndWait(this.log, this.webDriver));
            this.seleneseCommands.Add("open", new SeleneseOpen(this.log, this.webDriver));
            this.seleneseCommands.Add("pause", new SelenesePause(this.log, this.webDriver));
            this.seleneseCommands.Add("select", new SeleneseSelect(this.log, this.webDriver));
            this.seleneseCommands.Add("store", new SeleneseStore(this.log, this.webDriver, this.testVariables));
            this.seleneseCommands.Add("type", new SeleneseType(this.log, this.webDriver));
            this.seleneseCommands.Add("waitForPageToLoad", new SeleneseWaitForPageToLoad(this.log, this.webDriver));
            this.seleneseCommands.Add("verifyTextPresent", new SeleneseVerifyTextPresent(this.log, this.webDriver));
        }

        private void trapSeleneseReturn(Boolean functionReturn)
        {
            if (this.testHadError == true)
            {
                return;
            }
            if (functionReturn == true)
            {
                this.testHadError = false;
                return;
            }
            this.testHadError = true;
            return;
        }

        public void processSelenese(Selenium_Test_Trinome testCommand)
        {
            if (this.testHadError == true)
            {
                this.log.logFailure(testCommand, "not executed - failure already occurred.");
                return;
            }

            // Special exception for cleaning up / interpolating the testCommand into the new values.
            if (testCommand.command != "store")
            {
                testCommand.target = this.testVariables.replaceVariables(testCommand.target);
                testCommand.value = this.testVariables.replaceVariables(testCommand.value);
            }

            if (this.seleneseCommands.ContainsKey(testCommand.command))
            {
                Selenese_Command cmd = (Selenese_Command) this.seleneseCommands[testCommand.command];
                this.trapSeleneseReturn(cmd.run(testCommand));
            }
            else
            {
                this.testHadError = true;
                this.log.logFailure(testCommand, "unimplemented selenese");
            }

        }
 
    }
}
