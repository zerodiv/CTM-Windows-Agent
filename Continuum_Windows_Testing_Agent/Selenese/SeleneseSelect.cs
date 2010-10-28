using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Continuum_Windows_Testing_Agent
{

    class SeleneseSelect : Selenese_Command
    {

        public SeleneseSelect(Selenium_Test_Log log, IWebDriver webDriver) : base(log,webDriver) {
        }

        public override Boolean run(Selenium_Test_Trinome testCommand)
        {

            this.log.startTimer();

            // label=bar
            String label = "";
            if (testCommand.getValue().StartsWith("label="))
            {
                label = testCommand.getValue();
                label = label.Replace("label=", "");
            }
            else
            {
                this.log.logFailure(testCommand, "Value is expected to be in the form of label=");
                return false;
            }

            // First attempte is via constructed xpath.

            IWebElement option = null;
            Boolean optionFound = false;

            try
            {
                String xByNamePath = "//select[@name='" + testCommand.getTarget() + "']/option[normalize-space(.)='" + label + "']";
                option = this.webDriver.FindElement(this.locator.convert(xByNamePath));
                optionFound = true;
            }
            catch
            {
                optionFound = false;
            }

            // Try by id if the option was not found. 
            if (optionFound == false)
            {
                try
                {
                    String xByIdPath = "//select[@id='" + testCommand.getTarget() + "']/option[normalize-space(.)='" + label + "']";
                    option = this.webDriver.FindElement(this.locator.convert(xByIdPath));
                    optionFound = true;
                }
                catch
                {
                    optionFound = false;
                }
            }

            if (optionFound == false)
            {
                this.log.logFailure(testCommand, "Failed to find option");
                return false;
            }

            try
            {
                option.Select();
                this.log.logSuccess(testCommand, "");
                return true;

            }
            catch (Exception e)
            {
                this.log.logFailure(testCommand, e.Message);
            }


            return false;

            /*
            try
            {
                

                IWebElement element = this.webDriver.FindElement(this.convertSelenseLocatorString(testCommand.target));
                
                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> options = element.FindElements(By.TagName("option"));

                foreach (IWebElement option in options)
                {
                    if (option.Text == label)
                    {
                        option.Select();
                        this.log.logSuccess(testCommand, "");
                        return true;
                    }
                }
                this.log.logFailure(testCommand, "Did not find the target label=" + label);
                return true;
            }
            catch (Exception e)
            {
                this.log.logFailure(testCommand, e.Message);
            }
            */

        }
    }

}
