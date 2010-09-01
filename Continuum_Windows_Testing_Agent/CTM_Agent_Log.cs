using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Continuum_Windows_Testing_Agent
{
    class CTM_Agent_Log
    {
        private Boolean useLogFile;
        private String logFile;

        public CTM_Agent_Log()
        {
            this.useLogFile = true;
            this.logFile = Environment.GetEnvironmentVariable("TEMP") + "\\ctm_agent.log";
            if (File.Exists(this.logFile))
            {
                File.Delete(this.logFile);
            }
        }

        public CTM_Agent_Log(String logFile)
        {
            this.useLogFile = true;
            this.logFile = logFile;
        }

        public void message(String message)
        {
            String ts = System.DateTime.Now.ToString();

            if (this.useLogFile == true)
            {
                using (StreamWriter fh = new StreamWriter(this.logFile, true))
                {
                    fh.WriteLine(ts + " - " + message);
                    fh.Flush();
                    fh.Close();
                }
            }
        }

        public String getLogContents()
        {
            if (this.useLogFile != true)
            {
                return "";
            }
            if (File.Exists(this.logFile))
            {
                return File.ReadAllText(this.logFile);
            }
            return "";
        }

    }
}
