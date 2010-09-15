using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Continuum_Windows_Testing_Agent
{

    class Selenium_Test_Suite_Variables
    {
        private Hashtable vars;

        public Selenium_Test_Suite_Variables()
        {
            this.vars = new Hashtable();
        }

        public void consumeTrinome(Selenium_Test_Trinome triNome)
        {
            if (this.vars.ContainsKey(triNome.value) == true)
            {
                this.vars[triNome.value] = triNome.target;
            }
            else
            {
                this.vars.Add(triNome.value, triNome.target);
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
