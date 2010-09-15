using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace Continuum_Windows_Testing_Agent
{
    abstract class Selenese_Command
    {
        protected Selenium_Test_Log log;
        protected IWebDriver webDriver;
        protected Selenese_Locator locator;

        public Selenese_Command(Selenium_Test_Log log, IWebDriver webDriver)
        {
            this.log = log;
            this.webDriver = webDriver;
            this.locator = new Selenese_Locator(log, webDriver);
        }

        protected String runJavascriptValue(String javascriptValue)
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

            if (! javascript.EndsWith(";"))
            {
                javascript = javascript + ";";
            }
            String value = "";
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)this.webDriver;

            if (jsExecutor.IsJavaScriptEnabled == true)
            {

                this.log.message("executing javascript: " + javascript);
                Object ret = jsExecutor.ExecuteScript(javascript);
                value = ret.ToString();
                this.log.message("end js_exec value: " + value);
            }

            return value;

        }

        abstract public Boolean run(Selenium_Test_Trinome testCommand);
    }
}
