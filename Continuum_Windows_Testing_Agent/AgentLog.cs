using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Continuum_Windows_Testing_Agent
{
    class AgentLog
    {
        private StreamWriter fh;
        private Boolean useLogFile;
        private String logFile;

        public AgentLog()
        {
            this.useLogFile = true;
            this.logFile = Environment.GetEnvironmentVariable("TEMP") + "\\ctm_agent.log";
            this.initLog();
        }

        public AgentLog(String logFile)
        {
            this.useLogFile = true;
            this.logFile = logFile;
            this.initLog();
        }

        private void initLog()
        {
            if (this.useLogFile != true)
            {
                return;
            }

            if (this.fh != null)
            {
                return;
            }

            try
            {
                this.fh = new StreamWriter(this.logFile);
            }
            catch
            {
                this.useLogFile = false;
                this.logFile = "";
            }
        }

        public void message(String message)
        {
            this.initLog();

            String ts = System.DateTime.Now.ToString();

            if (this.useLogFile == true)
            {
                this.fh.WriteLine(ts + " - " + message);
                this.fh.Flush();
            }

        }

        public String getLogContents()
        {
            if (this.useLogFile != true)
            {
                return "";
            }
            if (this.fh != null)
            {
                this.fh.Close();
                this.fh = null;
            }
            return File.ReadAllText(this.logFile);
        }

    }
}
