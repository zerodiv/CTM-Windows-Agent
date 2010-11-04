using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Continuum_Windows_Testing_Agent
{

    class Selenium_Test_Suite_Variables
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
            foreach (String key in this.vars.Keys)
            {
                target = target.Replace("${" + key + "}", this.vars[key].ToString());
            }
            return target;
        }

    }
}
