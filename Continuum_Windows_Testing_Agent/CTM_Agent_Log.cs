using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace Continuum_Windows_Testing_Agent
{
    class CTM_Agent_Log
    {
        private ArrayList lastLogLines;
        
        public CTM_Agent_Log()
        {
            this.lastLogLines = new ArrayList();
        }

        public void message(String message)
        {
            // Manage the arraylist.
            int maxEntries = 255;
            if (this.lastLogLines.Count > maxEntries)
            {
                this.lastLogLines.RemoveRange(0, 1);
            }
            String logLine = System.DateTime.Now.ToString() + " - " + message;
            this.lastLogLines.Add(logLine + "\r\n");
        }


        public String getLastLogLines()
        {
            String logLines = "";
            foreach (String logLine in this.lastLogLines)
            {
                logLines += logLine;
            }
            return logLines;
        }

    }
}
