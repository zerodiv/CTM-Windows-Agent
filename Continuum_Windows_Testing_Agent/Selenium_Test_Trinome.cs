using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using System.Text.RegularExpressions;

namespace Continuum_Windows_Testing_Agent
{
    public class Selenium_Test_Trinome
    {
        private int _id;
        private String _command;
        private String _target;
        private String _value;

        public void setId(int id)
        {
            this._id = id;
        }

        public int getId()
        {
            return this._id;
        }

        public void setCommand(String command)
        {
            this._command = command;
        }

        public String getCommand()
        {
            return this._command;
        }

        public void setTarget(String target) {
            this._target = target;
        }

        public String getTarget()
        {
            return this._target;
        }

        public void setValue(String value)
        {
            this._value = value;
        }

        public String getValue()
        {
            return this._value;
        }

        public void interpolateSeleneseVariables(IWebDriver webDriver, ISeleniumTestSuiteVariables testVariables)
        {
            if (this.getTarget() == "${SaveSearchName}")
            {
            }

            // Special exception for cleaning up / interpolating the testCommand into the new values.
            if (this.getCommand() != "store" && this.getCommand() != ":comment:")
            {
                this.setTarget(testVariables.replaceVariables(this.getTarget()));
                this.setValue(testVariables.replaceVariables(this.getValue()));
            }

            if (this.getTarget() == "${SaveSearchName}")
            {
            }

            // If the value contains javascript run it and replace the value.
            this.setTarget(this.runJavascriptValue(webDriver, this.getTarget()));
            this.setValue(this.runJavascriptValue(webDriver, this.getValue()));
        }

        private String runJavascriptValue(IWebDriver webDriver, String javascriptValue)
        {

            if (javascriptValue.Length == 0)
            {
                return javascriptValue;
            }

            if (javascriptValue.Contains("javascript") == false)
            {
                return javascriptValue;
            }

            MatchCollection matches = Regex.Matches(javascriptValue, @"javascript{(.*?)}");

            if (matches.Count == 0)
            {
                return javascriptValue;
            }

            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)webDriver;

            foreach (Match match in matches)
            {
                String js = match.Groups[1].Value;

                // quick fix for people forgetting to do a return.
                if (js.Contains("return") == false)
                {
                    js = "return " + js;
                }

                if (!js.EndsWith(";"))
                {
                    js = js + ";";
                }

                if (jsExecutor.IsJavaScriptEnabled == true)
                {
                    String val = jsExecutor.ExecuteScript(js).ToString();
                    javascriptValue = javascriptValue.Replace(
                        "javascript{" + match.Groups[1].ToString() + "}",
                        val
                    );
                }

            }

            return javascriptValue;

        }

    }
}
