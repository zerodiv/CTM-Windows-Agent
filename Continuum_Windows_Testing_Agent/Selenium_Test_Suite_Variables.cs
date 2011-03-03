using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Continuum_Windows_Testing_Agent
{
    public interface ISeleniumTestSuiteVariables
    {
        void consumeCommand(String value, String target);
        String replaceVariables(String target);
    }

    public class Selenium_Test_Suite_Variables : ISeleniumTestSuiteVariables
    {
        private Dictionary<String,String> vars;

        public Selenium_Test_Suite_Variables()
        {
            this.vars = new Dictionary<String,String>();
        }

        public void consumeCommand(String value, String target)
        {
            if (this.vars.ContainsKey(value) == true)
            {
                this.vars[value] = target;
            }
            else
            {
                this.vars.Add(value, target);
            }
        }

        public String replaceVariables(String target)
        {
            // Does it even contain a variable?
            if (target.Contains("${") != true)
            {
                return target;
            }
            foreach (String key in this.vars.Keys)
            {
                target = target.Replace("${" + key + "}", this.vars[key].ToString());
            }
            return target;
        }

    }
}
