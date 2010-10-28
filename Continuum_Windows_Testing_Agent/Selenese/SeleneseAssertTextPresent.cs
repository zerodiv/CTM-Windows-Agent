using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OpenQA.Selenium;

namespace Continuum_Windows_Testing_Agent.Selenese
{
    class SeleneseAssertTextPresent : Selenese_Command
    {
        public SeleneseAssertTextPresent(Selenium_Test_Log log, IWebDriver webDriver) : base(log,webDriver) {
        }

        public override Boolean run(Selenium_Test_Trinome testCommand)
        {
            this.log.startTimer();
            // TODO: We should add wildcard support into this. 
            // TODO: We should also see why the xpath selector of //*[text()='baz'] is not working. Is this a function of top level selectors? (confused, and will come back later)
            try
            {
                Regex matchRegex = null;
                if (testCommand.getTarget().Contains("*"))
                {
                    String strRegex = testCommand.getTarget();
                    strRegex = strRegex.Replace("*", ".*");

                    matchRegex = new Regex(strRegex);
                }

                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> elements = this.webDriver.FindElements(By.XPath("//html/*"));

                foreach (IWebElement element in elements)
                {
                    if (matchRegex != null )
                    {
                        if (matchRegex.IsMatch(element.Text))
                        {
                            this.log.logSuccess(testCommand, "");
                            return true;
                        }
                    }
                    else
                    {
                        if (element.Text == testCommand.getTarget())
                        {
                            // Exact match. 
                            this.log.logSuccess(testCommand, "");
                            return true;
                        }
                        if (element.Text.Contains(testCommand.getTarget()))
                        {
                            // Contains support.
                            this.log.logSuccess(testCommand, "");
                            return true;
                        }
                    }
                }

                this.log.logFailure(testCommand, "Failed to find target text within page.");
                return false;
            }
            catch (Exception e)
            {
                this.log.logFailure(testCommand, e.Message);
            }
            return false;
        }

    }
}
