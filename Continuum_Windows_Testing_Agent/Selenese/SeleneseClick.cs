using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Continuum_Windows_Testing_Agent.Selenese
{
    class SeleneseClick : Selenese_Command
    {
        public SeleneseClick(Selenium_Test_Log log, IWebDriver webDriver)
            : base(log, webDriver)
        {
        }

        public override Boolean run(Selenium_Test_Trinome testCommand)
        {
            this.log.startTimer();
            try
            {
                IWebElement element = this.webDriver.FindElement(this.locator.convert(testCommand.getTarget()));
                
                element.Click();
                
                this.waitForPageToLoad();

                this.log.logSuccess(testCommand, "");
                return true;
            }
            catch (Exception e)
            {
                this.log.logFailure(testCommand, e.Message);
            }
            return false;
        }
    }
}
