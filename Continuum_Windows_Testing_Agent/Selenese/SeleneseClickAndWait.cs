using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Continuum_Windows_Testing_Agent.Selenese
{
    class SeleneseClickAndWait : Selenese_Command
    {
        public SeleneseClickAndWait(Selenium_Test_Log log, IWebDriver webDriver)
            : base(log, webDriver)
        {
        }
        public override Boolean run(Selenium_Test_Trinome testCommand)
        {
            this.log.startTimer();
            try
            {
                IWebElement element = this.webDriver.FindElement(this.locator.convert(testCommand.getTarget()));
                // Click is a blocking operand on most browsers.
                element.Click();

                if (testCommand.getValue().Length > 0)
                {

                    try
                    {
                        int timeOut = System.Int32.Parse(testCommand.getValue());

                        if (timeOut > 0)
                        {
                            this.waitForPageToLoad(timeOut);
                        }
                        else
                        {
                            this.waitForPageToLoad();
                        }

                    }
                    catch (Exception e)
                    {
                        // Failed to parse the int value, but do a normal wait for page to load then.
                        this.waitForPageToLoad();
                    }
                }
                else
                {
                    this.waitForPageToLoad();
                }

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
