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
        public Boolean testHadError;
        private IWebDriver webDriver;
        private String baseUrl;

        private Selenium_Test_Suite_Variables testVariables;
        private Selenium_Test_Log log;

        public Selenium_Test(IWebDriver webDriver, Selenium_Test_Log log)
        {
            this.testHadError = false;
            this.webDriver = webDriver;
            this.baseUrl = "";
            this.testVariables = new Selenium_Test_Suite_Variables();
            this.log = log;
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
                testCommand.target = testVariables.replaceVariables(testCommand.target);
                testCommand.value = testVariables.replaceVariables(testCommand.value);
            }

            switch (testCommand.command)
            {
                case "addSelection":
                    this.trapSeleneseReturn(this.seleneseAddSelection(testCommand));
                    return;
                case "clickAndWait":
                    this.trapSeleneseReturn(this.seleneseClickAndWait(testCommand));
                    return;
                case "click":
                    this.trapSeleneseReturn(this.seleneseClick(testCommand));
                    return;
                case "doubleClick":
                    this.trapSeleneseReturn(this.seleneseDoubleClick(testCommand));
                    return;
                case "controlKeyDown":
                    this.trapSeleneseReturn(this.seleneseControlKeyDown(testCommand));
                    return;
                case "controlKeyUp":
                    this.trapSeleneseReturn(this.seleneseControlKeyUp(testCommand));
                    return;
                case "focus":
                    this.trapSeleneseReturn(this.seleneseFocus(testCommand));
                    return;
                case "open":
                    this.trapSeleneseReturn(this.seleneseOpen(testCommand));
                    return;
                case "select":
                    this.trapSeleneseReturn(this.seleneseSelect(testCommand));
                    return;
                case "selectWindow":
                    this.trapSeleneseReturn(this.seleneseSelectWindow(testCommand));
                    return;
                case "store":
                    this.trapSeleneseReturn(this.seleneseStore(testCommand));
                    return;
                case "type":
                    this.trapSeleneseReturn(this.seleneseType(testCommand));
                    return;
                case "typeAndWait":
                    this.trapSeleneseReturn(this.seleneseTypeAndWait(testCommand));
                    return;
                case "waitForPageToLoad":
                    this.trapSeleneseReturn(this.seleneseWaitForPageToLoad(testCommand));
                    return;
                case "verifyTextPresent":
                    this.trapSeleneseReturn(this.seleneseVerifyTextPresent(testCommand));
                    return;
                case "pause":
                    this.trapSeleneseReturn(this.selenesePause(testCommand));
                    return;
                default:
                    this.testHadError = true;
                    this.log.logFailure(testCommand, "unimplemented selenese");
                    return;
            }
        }

        private bool selenesePause(Selenium_Test_Trinome testCommand)
        {
            this.log.startTimer();
            int testWait = 3000;

            if (testCommand.value != "")
            {
                testWait = Convert.ToInt32(testCommand.value);
            }

            if (testWait > 0)
            {
                System.Threading.Thread.Sleep(testWait);
            }


            this.log.logSuccess(testCommand, "waited: " + testWait);
            return true;
        }

        public Boolean seleneseStore(Selenium_Test_Trinome testCommand)
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
            
                this.log.message("executing javascript: " + javascript );

                Object ret = jsExecutor.ExecuteScript(javascript);
                value = ret.ToString();
                this.log.message("end js_exec value: " + value);
            }

            return value;

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

            try
            {
                this.log.message("legacy byName start");
                By byName = By.Name(locator);
                IWebElement target = this.webDriver.FindElement(byName);
                this.log.message("legacy byName end");
                return byName;
            }
            catch
            {
                this.log.message("legacy byName end - failover to id");
                // okay so by name failed, try id, otherwise they are fucked =)
                return By.Id(locator);
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
            this.log.startTimer();
            try
            {
                IWebElement element = this.webDriver.FindElement(this.convertSelenseLocatorString(testCommand.target));

                // Click is a blocking operand on most browsers.
                element.Click();

                this.log.logSuccess(testCommand, "" );
                /*
                int testWait = 3000;

                if (testCommand.value != "")
                {
                    testWait = Convert.ToInt32(testCommand.value);
                }

                    if (testWait > 0)
                    {
                        System.Threading.Thread.Sleep(testWait);
                    }
                

                this.log.logSuccess(testCommand, "waited: " + testWait);
                */
                return true;
            }
            catch (Exception e)
            {
                this.log.logFailure(testCommand, e.Message);
            }
            return false;
        }

        public Boolean seleneseClick(Selenium_Test_Trinome testCommand)
        {
            this.log.startTimer();
            try
            {
                IWebElement element = this.webDriver.FindElement(this.convertSelenseLocatorString(testCommand.target));

                    element.Click();
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
            this.log.logSuccess(testCommand, "");
            return true;
        }

        public Boolean seleneseSelect(Selenium_Test_Trinome testCommand)
        {

            this.log.startTimer();

            // label=bar
            String label = "";
            if (testCommand.value.StartsWith("label="))
            {
                label = testCommand.value;
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
                String xByNamePath = "//select[@name='" + testCommand.target + "']/option[normalize-space(.)='" + label + "']";
                option = this.webDriver.FindElement(this.convertSelenseLocatorString(xByNamePath));
                optionFound = true;
            } catch {
                optionFound = false;
            }

            // Try by id if the option was not found. 
            if (optionFound == false)
            {
                try
                {
                    String xByIdPath = "//select[@id='" + testCommand.target + "']/option[normalize-space(.)='" + label + "']";
                    option = this.webDriver.FindElement(this.convertSelenseLocatorString(xByIdPath));
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
