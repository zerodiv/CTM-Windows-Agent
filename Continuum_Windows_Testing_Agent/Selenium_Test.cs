using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
// using OpenQA.Selenium.Internal;

// using Selenium;

namespace Continuum_Windows_Testing_Agent
{
    class Selenium_Test
    {
        private IWebDriver webDriver;
        private String baseUrl;

        private Selenium_Test_Suite_Variables testVariables;
        private Selenium_Test_Log log;

        public Selenium_Test(IWebDriver webDriver, Selenium_Test_Log log)
        {
            this.webDriver = webDriver;
            this.baseUrl = "";
            this.testVariables = new Selenium_Test_Suite_Variables();
            this.log = log;
        }

        public Boolean processSelenese(Selenium_Test_Trinome testCommand)
        
        {
            // Special exception for cleaning up / interpolating the testCommand into the new values.
            if (testCommand.command != "store")
            {
                testCommand.target = testVariables.replaceVariables(testCommand.target);
                testCommand.value = testVariables.replaceVariables(testCommand.value);
            }

            switch (testCommand.command)
            {
                case "addSelection":
                    return this.seleneseAddSelection(testCommand);
                case "clickAndWait":
                    return this.seleneseClickAndWait(testCommand);
                case "click":
                    return this.seleneseClick(testCommand);
                case "doubleClick":
                    return this.seleneseDoubleClick(testCommand);
                case "controlKeyDown":
                    return this.seleneseControlKeyDown(testCommand);
                case "controlKeyUp":
                    return this.seleneseControlKeyUp(testCommand);
                case "focus":
                    return this.seleneseFocus(testCommand);
                case "open":
                    return this.seleneseOpen(testCommand);
                case "select":
                    return this.seleneseSelect(testCommand);
                case "selectWindow":
                    return this.seleneseSelectWindow(testCommand);
                case "store":
                    return this.seleneseStore(testCommand);
                case "type":
                    return this.seleneseType(testCommand);
                case "typeAndWait":
                    return this.seleneseTypeAndWait(testCommand);
                case "waitForPageToLoad":
                    return this.seleneseWaitForPageToLoad(testCommand);
                case "verifyTextPresent":
                    return this.seleneseVerifyTextPresent(testCommand);
                default:
                    return false;
            }
        }

        public Boolean seleneseStore(Selenium_Test_Trinome testCommand)
        {
            this.log.startTimer();
            try
            {
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

        public String runJavascriptValue(String javascriptValue)
        {
            String javascript = "";
            if (javascriptValue.StartsWith("javascript{"))
            {
                javascript = javascriptValue;
                javascript = Regex.Replace(javascript, @"^javascript{", "");
                javascript = Regex.Replace(javascript, @"}$", "");
            }
            else
            {
                return javascriptValue;
            }

            // quick fix for people forgetting to do a return.
            if (javascript.Contains("return") == false)
            {
                javascript = "return " + javascript;
            }

            String value = "";
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)this.webDriver;

            if (jsExecutor.IsJavaScriptEnabled == true)
            {
                Object ret = jsExecutor.ExecuteScript(javascript);
                value = ret.ToString();
            }

            return value;

            /*
            if (this.webDriver.GetType() == typeof(InternetExplorerDriver) )
            {
                if ( ((InternetExplorerDriver)this.webDriver).IsJavaScriptEnabled == true ) 
                {
                    Object ret = ((InternetExplorerDriver)this.webDriver).ExecuteScript(javascript, null);
                    value = ret.ToString();
                }
            }
            if (this.webDriver.GetType() == typeof(FirefoxDriver))
            {
                if (((FirefoxDriver)this.webDriver).IsJavaScriptEnabled == true)
                {
                    Object ret = ((FirefoxDriver)this.webDriver).ExecuteScript(javascript, null);
                    value = ret.ToString();
                }
            }
            if (this.webDriver.GetType() == typeof(ChromeDriver))
            {
                if (((ChromeDriver)this.webDriver).IsJavaScriptEnabled == true)
                {
                    Object ret = ((ChromeDriver)this.webDriver).ExecuteScript(javascript, null);
                    value = ret.ToString();
                }
            }
            return value;
            */
        }

        public By convertSelenseLocatorString(String locator)
        {
            // TODO: locate by DOM, if it is even needed.
            if (locator.StartsWith("css="))
            {
                locator = locator.Replace("css=", "");
                return By.CssSelector(locator);
            }

            if (locator.StartsWith("class="))
            {
                locator = locator.Replace("class=", "");
                return By.ClassName(locator);
            }

            if (locator.StartsWith("link=")) 
            {
                locator = locator.Replace("link=", "");
                return By.LinkText(locator);
            }
            if (locator.StartsWith("//"))
            {
                return By.XPath(locator);
            }
            if (locator.StartsWith("xpath="))
            {
                locator = locator.Replace("xpath=", "");
                return By.XPath(locator);
            }
            if (locator.StartsWith("name="))
            {
                locator = locator.Replace("name=", "");
                return By.Name(locator);
            }
            if (locator.StartsWith("identifier="))
            {
                locator.Replace("identifier=", "");
                return By.Id(locator);
            }
            // We have to lookup by name, then id because old selenium supports both.

            try
            {
                By byName = By.Name(locator);
                IWebElement target = this.webDriver.FindElement(byName);
                return byName;
            }
            catch
            {
                // okay so by name failed, try id, otherwise they are fucked =)
                By byId = By.Id(locator);
                IWebElement target = this.webDriver.FindElement(byId);
                return byId;
            }

        }

        public Boolean seleneseOpen(Selenium_Test_Trinome testCommand)
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

        public Boolean seleneseType(Selenium_Test_Trinome testCommand)
        {
           this.log.startTimer();
            try
            {
                IWebElement element = this.webDriver.FindElement(this.convertSelenseLocatorString(testCommand.target));
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

        public Boolean seleneseClickAndWait(Selenium_Test_Trinome testCommand )
        {
            // same action as a click now since page loads actually block.
            return this.seleneseClick(testCommand);
        }

        public Boolean seleneseClick(Selenium_Test_Trinome testCommand)
        {
            this.log.startTimer();
            try
            {
                IWebElement element = this.webDriver.FindElement(this.convertSelenseLocatorString(testCommand.target));

                
                if (element.GetAttribute("type") == "checkbox")
                {
                    // element.Select();
                    element.Click();
                    // element.Toggle();
                }
                else
                {
                    element.Click();
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

        public Boolean seleneseWaitForPageToLoad(Selenium_Test_Trinome testCommand)
        {
            this.log.logSuccess(testCommand, "*WARNING* This command means nothing now under webDriver, you are safe in removing this command.");
            return true;
        }

        public Boolean seleneseSelect(Selenium_Test_Trinome testCommand)
        {

            this.log.startTimer();
            try
            {
                // label=bar
                String label = "";
                if (testCommand.value.StartsWith("label="))
                {
                    label = testCommand.value;
                    label = label.Replace("label=", "");
                }
                else 
                {
                    this.log.logFailure(testCommand, "Value is expected to be in the form of label=" );
                    return false;
                }

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
            return false;
        }

        public Boolean seleneseDoubleClick(Selenium_Test_Trinome testCommand)
        {
            this.log.logFailure(testCommand, "This command is not supported for now");
            return false;
            /*
            this.log.startTimer();
            try
            {
                this.se.DoubleClick(testCommand.target);
                this.log.logSuccess(testCommand, "");
                return true;
            }
            catch (Exception e)
            {
                this.log.logFailure(testCommand, e.Message);
            }
            return false;
            */
        }

        public Boolean seleneseVerifyTextPresent(Selenium_Test_Trinome testCommand)
        {
            this.log.logFailure(testCommand, "This command is not supported for now");
            return false;
            /*
            this.log.startTimer();
            try
            {
                this.se.IsTextPresent(testCommand.target);
                this.log.logSuccess(testCommand, "");
                return true;
            }
            catch (Exception e)
            {
                this.log.logFailure(testCommand, e.Message);
            }
            return false;
            */
        }

        public Boolean seleneseFocus(Selenium_Test_Trinome testCommand)
        {
            this.log.logFailure(testCommand, "This command is not supported for now");
            return false;
            /*
            this.log.startTimer();
            try
            {
                this.se.Focus(testCommand.target);
                this.log.logSuccess(testCommand, "");
                return true;
            }
            catch (Exception e)
            {
                this.log.logFailure(testCommand, e.Message);
            }
            return false;
            */
        }
         
        public Boolean seleneseControlKeyDown(Selenium_Test_Trinome testCommand)
        {
            this.log.logFailure(testCommand, "This command is not supported for now");
            return false;
            /*
            this.log.startTimer();
            try
            {
                this.se.ControlKeyDown();
                this.log.logSuccess(testCommand, "");
                return true;
            }
            catch (Exception e)
            {
                this.log.logFailure(testCommand, e.Message);
            }
            return false;
            */
        }

        public Boolean seleneseAddSelection(Selenium_Test_Trinome testCommand)
        {
            return this.seleneseSelect(testCommand);
        }

        public Boolean seleneseControlKeyUp(Selenium_Test_Trinome testCommand)
        {
            this.log.logFailure(testCommand, "This command is not supported for now");
            return false;
            /*
            this.log.startTimer();
            try
            {
                this.se.ControlKeyUp();
                this.log.logSuccess(testCommand, "");
                return true;
            }
            catch (Exception e)
            {
                this.log.logFailure(testCommand, e.Message);
            }
            return false;
            */
        }

        public Boolean seleneseTypeAndWait(Selenium_Test_Trinome testCommand)
        {
            this.log.logFailure(testCommand, "This command is not supported for now");
            return false;
            /*
            this.log.startTimer();
            try
            {

                this.se.Type(testCommand.target, testCommand.value);
                this.se.WaitForPageToLoad("10000");
                this.log.logSuccess(testCommand, "");
                return true;
            }
            catch (Exception e)
            {
                this.log.logFailure(testCommand, e.Message);
            }
            return false;
            */
        }

        public Boolean seleneseSelectWindow(Selenium_Test_Trinome testCommand)
        {
            this.log.logFailure(testCommand, "This command is not supported for now");
            return false;
            /*
            this.log.startTimer();
            try
            {

                this.se.SelectWindow(testCommand.target);
                this.log.logSuccess(testCommand, "");
                return true;
            }
            catch (Exception e)
            {
                this.log.logFailure(testCommand, e.Message);
            }
            return false;
            */
        }

    }
}
