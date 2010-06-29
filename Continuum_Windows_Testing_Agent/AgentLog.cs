using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Continuum_Windows_Testing_Agent
{
    class AgentLog
    {
        private String log;
        private Boolean useLogFile;
        private String logFile;

        public AgentLog()
        {
            this.useLogFile = true;
            this.logFile = Environment.GetEnvironmentVariable("TEMP") + "\\ctm_agent.log";
        }

        public void reset()
        {
            this.log = "";
        }

        public void message(String message)
        {
            String ts = System.DateTime.Now.ToString();

            if (this.useLogFile == true)
            {
                using (StreamWriter sw = File.AppendText(this.logFile))
                {
                    sw.WriteLine(ts + " - " + message);
                }  
                
            }

            this.log += ts + " - " + message + "\r\n";
        }

        public String getLog()
        {
            return this.log;
        }

    }
}
