using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Continuum_Windows_Testing_Agent
{
    public class Selenium_Test_Trinome
    {
        private String _command;
        private String _target;
        private String _value;

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

    }
}
