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
            this.maxEntries = 255;
        }

        public void message(String message)
        {
            // Manage the arraylist.
            if (this.lastLogLines.Count > this.maxEntries)
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

    /*
    JEO: This is buggy right now (Popping cross-thread errors. 
    // JEO: Migrated this to a event model, it makes the UI so much easier to deal with.
    public delegate void CTM_Agent_Log_Message_Handler(object sender, CTM_Agent_Log_Message_Handler_Args e );

    public class CTM_Agent_Log_Message_Handler_Args : EventArgs
    {
        public string message;
    }

    class CTM_Agent_Log
    {
        public event CTM_Agent_Log_Message_Handler logMessage;

        private ArrayList lastLogLines;
        
        public CTM_Agent_Log()
        {
            this.logMessage += new CTM_Agent_Log_Message_Handler(nullHandler);
            this.lastLogLines = new ArrayList();
        }

        public void nullHandler(object sender, CTM_Agent_Log_Message_Handler_Args e) {
        }

        public void message(String message)
        {
            CTM_Agent_Log_Message_Handler_Args args = new CTM_Agent_Log_Message_Handler_Args();
            args.message = message;
            this.logMessage.Invoke(this, args);
        }

    }
    */
}
