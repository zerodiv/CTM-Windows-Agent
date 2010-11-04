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
        public int maxEntries;

        public CTM_Agent_Log()
        {
            this.lastLogLines = new ArrayList();
            this.maxEntries = 25;
        }

        public void message(String message)
        {
            // Manage the arraylist.
            if (this.lastLogLines.Count > this.maxEntries)
            {
                this.lastLogLines.RemoveAt(0);
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
