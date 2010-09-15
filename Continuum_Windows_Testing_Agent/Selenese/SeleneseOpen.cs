using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Continuum_Windows_Testing_Agent.Selenese
{
    class SeleneseOpen : Selenese_Command
    {
        private String baseUrl;

        public SeleneseOpen(Selenium_Test_Log log, IWebDriver webDriver)
            : base(log, webDriver)
        {
            this.baseUrl = "";
        }

        public override Boolean run(Selenium_Test_Trinome testCommand)
        {
            this.log.startTimer();
            try
            {
                if (testCommand.target.StartsWith("http") || testCommand.target.StartsWith("https"))
                {
                    this.baseUrl = testCommand.target;
                    if (this.baseUrl.EndsWith("/"))
                    {
                        this.baseUrl = this.baseUrl.Remove(this.baseUrl.Length - 1);
                    }
                    this.webDriver.Navigate().GoToUrl(testCommand.target);

                }
                else
                {
                    String partialUrl = this.baseUrl + testCommand.target;
                    this.webDriver.Navigate().GoToUrl(partialUrl);
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
