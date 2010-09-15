using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using OpenQA.Selenium;

namespace Continuum_Windows_Testing_Agent.Selenese
{
    class SeleneseType : Selenese_Command
    {
        public SeleneseType(Selenium_Test_Log log, IWebDriver webDriver)
            : base(log, webDriver)
        {
        }

        public override Boolean run(Selenium_Test_Trinome testCommand)
        {
            this.log.startTimer();

            // TinyMCE Hack - We don't need full dom= support, but we do need to support tinyMCE in our environment.
            // dom=document.getElementById('job_requirements_ifr').contentWindow.document.body
            try
            {
                Regex domRegex = new Regex(@"^dom=document.getElementById\('(.*?)'\)");
                Match domMatch = domRegex.Match(testCommand.target);

                if (domMatch.Success)
                {
                    String tinyMCEiFrame = domMatch.Groups[1].Value;
                    this.webDriver.SwitchTo().Frame(tinyMCEiFrame);
                    IWebElement element = this.webDriver.FindElement(By.Id("tinymce"));
                    element.SendKeys(this.runJavascriptValue(testCommand.value));
                    this.webDriver.SwitchTo().Window(this.webDriver.GetWindowHandle());
                    this.log.logSuccess(testCommand, "");
                    return true;
                }
            }
            catch (Exception e)
            {
                this.log.logFailure(testCommand, "failed dom= syntax: " + e.Message);
                return false;
            }

            try
            {
                IWebElement element = this.webDriver.FindElement(this.locator.convert(testCommand.target));
                element.SendKeys(this.runJavascriptValue(testCommand.value));
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
