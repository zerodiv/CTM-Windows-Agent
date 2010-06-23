using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Continuum_Windows_Testing_Agent
{
    class AgentLog
    {
        private String log;

        public void reset()
        {
            this.log = "";
        }

        public void message(String message)
        {
            this.log += System.DateTime.Now.ToString() + " - " + message + "\r\n";
        }

        public String getLog()
        {
            return this.log;
        }

    }
}
