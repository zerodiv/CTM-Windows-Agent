using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace Continuum_Windows_Testing_Agent
{
    public class Selenium_Test_Log
    {
        private string logFile;
        private string bodyFile;

        private StreamWriter fh;
        private StreamWriter bodyFh;

        private long totalCommands;
        private long totalSuccessfulCommands;
        private long totalFailedCommands;
        private SortedDictionary<String,TimeSpan> totalTimeByCommand;
        private Dictionary<String, ulong> totalCommandCount;

        public Selenium_Test_Log(String logFile ) {
            try
            {
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

                this.totalCommands = 0;
                this.totalFailedCommands = 0;
                this.totalSuccessfulCommands = 0;

                this.totalTimeByCommand = new SortedDictionary<String,TimeSpan>();
                this.totalCommandCount = new Dictionary<string,ulong>();

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

                // create the table for time spent.
                String cssClass = "odd";
                
                this.fh.WriteLine("<table class=\"ctmTable aiFullWidth\">");
                
                this.fh.WriteLine("<tr>");
                this.fh.WriteLine("<th colspan=\"4\">Selenese Command Breakdown:</th>");
                this.fh.WriteLine("</tr>");

                this.fh.WriteLine("<tr>");
                this.fh.WriteLine("<th>Command:</th>");
                this.fh.WriteLine("<th>Total:</th>");
                this.fh.WriteLine("<th>Time Spent:</th>");
                this.fh.WriteLine("<th>Avg Spent:</th>");
                this.fh.WriteLine("</tr>");

                foreach (String cmd in this.totalTimeByCommand.Keys)
                {
                    this.fh.WriteLine("<tr class=\"" + cssClass + "\">");
                    this.fh.WriteLine("<td>" + cmd + "</td>" );
                    this.fh.WriteLine("<td>" + this.totalCommandCount[cmd] + "</td>");
                    this.fh.WriteLine("<td>" + this.totalTimeByCommand[cmd] + "</td>");
                    this.fh.WriteLine("<td>" + System.TimeSpan.FromSeconds(this.totalTimeByCommand[cmd].TotalSeconds / this.totalCommandCount[cmd]) + "</td>");
                    this.fh.WriteLine("</tr>");

                    if (cssClass == "odd")
                    {
                        cssClass = "even";
                    }
                    else
                    {
                        cssClass = "odd";
                    }
                }
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

        public void logTrinome(Boolean sucessful, String command, String target, String value, String startTime, String stopTime, String elapsed, String message )
        {
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

            if (elapsed != "")
            {
                TimeSpan elap = System.TimeSpan.Parse(elapsed);

                if (this.totalTimeByCommand.ContainsKey(command))
                {
                    elap = elap.Add(this.totalTimeByCommand[command]);
                }

                // Add the elapsed time to the total time for this command.
                this.totalTimeByCommand[command] = elap;
                if (this.totalCommandCount.ContainsKey(command) == false)
                {
                    ulong l = new ulong();
                    l = 0;
                    this.totalCommandCount.Add(command, l);
                }
 
                this.totalCommandCount[command]++;

            }

            
            this.totalCommands++;
            
            this.bodyFh.WriteLine("<tr class=\"" + cssClass + "\">");
            this.bodyFh.WriteLine("<td>" + this.totalCommands + "</td>");
            this.bodyFh.WriteLine("<td>" + System.Web.HttpUtility.HtmlEncode(command) + "</td>");
            this.bodyFh.WriteLine("<td>" + System.Web.HttpUtility.HtmlEncode(target) + "</td>");
            this.bodyFh.WriteLine("<td>" + System.Web.HttpUtility.HtmlEncode(value) + "</td>");
            this.bodyFh.WriteLine("<td>" + startTime + "</td>");
            this.bodyFh.WriteLine("<td>" + stopTime + "</td>");
            this.bodyFh.WriteLine("<td>" + elapsed + "</td>");
            this.bodyFh.WriteLine("<td>><pre>" + System.Web.HttpUtility.HtmlEncode(message) + "</pre></td>");
            this.bodyFh.WriteLine("</tr>");
            this.bodyFh.Flush();

        }

    }
}
