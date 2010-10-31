using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Continuum_Windows_Testing_Agent
{
    public class Selenium_Test_Log
    {
        private string logFile;
        private string bodyFile;

        private StreamWriter fh;
        private StreamWriter bodyFh;

        private DateTime startTime;
        private DateTime stopTime;

        private DateTime startTest;
        private DateTime stopTest;

        private Boolean useVerboseTestLogs;

        private long totalCommands;
        private long totalSuccessfulCommands;
        private long totalFailedCommands;

        public Selenium_Test_Log(Boolean useVerboseTestLogs, String logFile ) {
            try
            {
                this.useVerboseTestLogs = useVerboseTestLogs;

                this.logFile = logFile;
                this.bodyFile = this.logFile + ".body";

                if (File.Exists(this.logFile))
                {
                    File.Delete(this.logFile);
                }

                if (File.Exists(this.bodyFile))
                {
                    File.Delete(this.bodyFile);
                }

                this.startTime = System.DateTime.UtcNow;
                this.stopTime = System.DateTime.UtcNow;

                this.startTest = System.DateTime.UtcNow;
                this.stopTime = System.DateTime.UtcNow;

                this.totalCommands = 0;
                this.totalFailedCommands = 0;
                this.totalSuccessfulCommands = 0;

                this.initLogFile();
                
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Boolean initLogFile()
        {
            try
            {
                this.fh = new StreamWriter(this.logFile);
                this.bodyFh = new StreamWriter(this.bodyFile);
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        public Boolean closeLogFile()
        {
            try
            {
                if (this.fh == null)
                {
                    return true;
                }

                this.stopTest = System.DateTime.UtcNow;

                // write header 
                String cssFile = "http://jorcutt-desktop/css/common.css";

                this.fh.WriteLine("<html>");
                this.fh.WriteLine("<head>");
                this.fh.WriteLine("<title>CTM - Test Run Log</title>");
                this.fh.WriteLine("<link href=\"" + cssFile + "\" type=\"text/css\" rel=\"stylesheet\"/>");
                this.fh.WriteLine("</head>");
                this.fh.WriteLine("<body>");

                this.fh.WriteLine("<div class=\"aiTableContainer aiFullWidth\">");
                this.fh.WriteLine("<table class=\"ctmTable aiFullWidth\">");
                this.fh.WriteLine("<tr>");
                this.fh.WriteLine("<th>Total Commands:</th>");
                this.fh.WriteLine("<th>Successful:</th>");
                this.fh.WriteLine("<th>Failed:</th>");
                this.fh.WriteLine("</tr>");

                this.fh.WriteLine("<tr class=\"odd\">");
                this.fh.WriteLine("<td><center>" + this.totalCommands + "</center></td>");
                this.fh.WriteLine("<td><center>" + this.totalSuccessfulCommands + "</center></td>");
                this.fh.WriteLine("<td><center>" + this.totalFailedCommands + "</center></td>");
                this.fh.WriteLine("</tr>");

                this.fh.WriteLine("</table>");
                this.fh.WriteLine("<table class=\"ctmTable aiFullWidth\">");
                
                // put out the colum headers.
               
                // chunk in body
                this.bodyFh.Close();
                this.bodyFh = null;
                
                StreamReader readBodyFh = new StreamReader(this.bodyFile);
                while (readBodyFh.EndOfStream != true)
                {
                    String line = readBodyFh.ReadLine();
                    this.fh.WriteLine(line);
                }

                readBodyFh.Close();
                
                // cleanup from the body.
                File.Delete(this.bodyFile);
                this.bodyFile = "";

                // write footer
                this.fh.WriteLine("</table>");
                this.fh.WriteLine("</div>");

                this.fh.WriteLine("</body>");
                this.fh.WriteLine("</html>");

                this.fh.Close();
                this.fh = null;

            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        public void startTimer()
        {
            this.startTime = System.DateTime.UtcNow;
            this.stopTime = System.DateTime.UtcNow;
        }

        public void stopTimer()
        {
            this.stopTime = System.DateTime.UtcNow;
        }

        public void logSuccess(Selenium_Test_Trinome triNome, String message)
        {
            this.stopTimer();
            this.logTrinome(triNome, true, message);
        }

        public void logFailure(Selenium_Test_Trinome triNome, String message)
        {
            this.stopTimer();
            this.logTrinome(triNome, false, message);
        }

        public String getLogContents()
        {
            try
            {
                return File.ReadAllText(this.logFile);
            }
            catch (Exception e)
            {
                return "Failed to read log file: " + e.Message;
            }
        }

        public void startTestMessage(String testName)
        {
            this.bodyFh.WriteLine("<tr>");
            this.bodyFh.WriteLine("<th colspan=\"8\">" + System.Web.HttpUtility.HtmlEncode(testName) + "</th>");
            this.bodyFh.WriteLine("</tr>");

            this.bodyFh.WriteLine("<tr>");
            this.bodyFh.WriteLine("<th>#</th>");
            this.bodyFh.WriteLine("<th>Command:</th>");
            this.bodyFh.WriteLine("<th>Target:</th>");
            this.bodyFh.WriteLine("<th>Value:</th>");
            this.bodyFh.WriteLine("<th>Start:</th>");
            this.bodyFh.WriteLine("<th>Stop:</th>");
            this.bodyFh.WriteLine("<th>Elapsed:</th>");
            this.bodyFh.WriteLine("<th>Message:</th>");
            this.bodyFh.WriteLine("</tr>");

        }

        public void insertTestComment(String comment)
        {
            this.bodyFh.WriteLine("<tr>");
            this.bodyFh.WriteLine("<td colspan=\"8\" class=\"comments\"><center>" + System.Web.HttpUtility.HtmlEncode(comment) + "</center></td>");
            this.bodyFh.WriteLine("</tr>");
        }

        public void message(String message)
        {
            if (this.useVerboseTestLogs == false)
            {
                return;
            }
            this.bodyFh.WriteLine("<tr class=\"odd\">");
            this.bodyFh.WriteLine("<td colspan=\"8\">" + System.DateTime.Now.ToString() + " - " + System.Web.HttpUtility.HtmlEncode(message) + "</td>");
            this.bodyFh.WriteLine("</tr>");
        }

        public void logTrinome(Selenium_Test_Trinome triNome, Boolean sucessful, String message )
        {
            // yes, I trust you the programmer to engage stop time.
            TimeSpan elapsed = this.stopTime - this.startTime;

            String cssClass = "failure";

            if (sucessful == true)
            {
                cssClass = "successful";
                this.totalSuccessfulCommands++;
            }
            else
            {
                this.totalFailedCommands++;
            }

            this.totalCommands++;
            
            this.bodyFh.WriteLine("<tr class=\"" + cssClass + "\">");
            this.bodyFh.WriteLine("<td>" + this.totalCommands + "</td>");
            this.bodyFh.WriteLine("<td>" + System.Web.HttpUtility.HtmlEncode(triNome.getCommand()) + "</td>");
            this.bodyFh.WriteLine("<td>" + System.Web.HttpUtility.HtmlEncode(triNome.getTarget()) + "</td>");
            this.bodyFh.WriteLine("<td>" + System.Web.HttpUtility.HtmlEncode(triNome.getValue()) + "</td>");
            this.bodyFh.WriteLine("<td>" + this.startTime + "</td>");
            this.bodyFh.WriteLine("<td>" + this.stopTime + "</td>");
            this.bodyFh.WriteLine("<td>" + elapsed + "</td>");
            this.bodyFh.WriteLine("<td>" + System.Web.HttpUtility.HtmlEncode(message) + "</td>");
            this.bodyFh.WriteLine("</tr>");
            this.bodyFh.Flush();

        }

    }
}
