using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net;
using System.Security.Permissions;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Specialized;
using System.Xml;
using System.Threading;
using System.ComponentModel;
using Ionic.Zip;

namespace Continuum_Windows_Testing_Agent
{

    class CTM_Agent
    {
        private String guid;
        private String ctmHostname;
        private String localIp;
        private String machineName;
        private Boolean isRegistered;

        private WebClient ctmClient;
        public BackgroundWorker ctmBgWorker;
        private Boolean useVerboseTestLogs;
   
        public LocalWebBrowser googlechrome;
        public LocalWebBrowser firefox;
        public LocalWebBrowser ie;
        public LocalWebBrowser safari;
        public CTM_Agent_Log log;

        public CTM_Agent()
        {

            // initalize the logging for the phone home agent.
            this.log = new CTM_Agent_Log();

            this.isRegistered = false;

            this.ctmClient = new WebClient();
            this.ctmClient.UploadValuesCompleted += new UploadValuesCompletedEventHandler(requestWork_Completed);

            this.ctmBgWorker = new BackgroundWorker();
            this.ctmBgWorker.DoWork += new DoWorkEventHandler(ctmBgWorker_DoWork);
            
            // init the various browsers
            this.googlechrome = new LocalWebBrowser("googlechrome");
            this.firefox = new LocalWebBrowser("firefox");
            this.ie = new LocalWebBrowser("ie");
            this.safari = new LocalWebBrowser("safari");

        }

        public void setUseVerboseTestLogs(Boolean useVerboseLogs)
        {
            this.useVerboseTestLogs = useVerboseLogs; 
        }

        public String setGuid(String guid)
        {
            this.guid = guid;
            return this.guid;
        }

        public String setCTMHostname(String hostname)
        {
            this.ctmHostname = hostname;
            return this.ctmHostname;
        }

        public String setMachineName(String name)
        {
            this.machineName = name;
            return this.machineName;
        }

        public String determineWindowsVersion()
        {
            String windowsVersion = "";
            windowsVersion += OSInfo.Name + " - " + OSInfo.Edition + " - " + OSInfo.ServicePack;
            return windowsVersion;
        }

        public String setLocalIp(String ip)
        {
            this.localIp = ip;
            return this.localIp;
        }

        public Boolean getIsRegistered()
        {
            return this.isRegistered;
        }

        private Boolean isInitalized()
        {
            if (this.guid.Length == 0)
            {
                return false;
            }

            if (this.ctmHostname.Length == 0)
            {
                return false;
            }

            if (localIp.Length == 0)
            {
                return false;
            }

            if (machineName.Length == 0)
            {
                return false;
            }

            return true;
        }

        
        public void run()
        {
            try
            {
                this.requestWork();
                // this.registerHost();
                // this.requestWork();
            }
            catch (Exception e ) 
            {
                // Yes this is jorcutt being paranoid about exceptions bubbling up and crashing the application.
                this.log.message("generic exception caught in Agent::run() message: " + e.Message);
            }
        }

        public void requestWork()
        {
            if (this.isInitalized() != true)
            {
                return;
            }

            if (this.ctmClient.IsBusy == true)
            {
                return;
            }

            if (this.ctmBgWorker.IsBusy == true)
            {
                return;
            }

            NameValueCollection postValues = new NameValueCollection();

            postValues.Add("guid", this.guid);

            postValues.Add("ip", this.localIp);

            postValues.Add("os", this.determineWindowsVersion());

            postValues.Add("machine_name", this.machineName);

            // add the browsers into our post params
            if (this.ie.exists == true)
            {
                postValues.Add("iexplore", "yes");
                postValues.Add("iexplore_version", this.ie.getVersion());
            }

            if (this.googlechrome.exists == true)
            {
                postValues.Add("googlechrome", "yes");
                postValues.Add("googlechrome_version", this.googlechrome.getVersion());
            }

            if (this.firefox.exists == true)
            {
                postValues.Add("firefox", "yes");
                postValues.Add("firefox_version", this.firefox.getVersion());
            }

            if (this.safari.exists == true)
            {
                postValues.Add("safari", "yes");
                postValues.Add("safari_version", this.safari.getVersion());
            }

            String pollUrl = "http://" + this.ctmHostname + "/agent/poll/";
            ctmClient.UploadValuesAsync(new Uri(pollUrl), postValues);

        }

        private void requestWork_Completed(object sender, UploadValuesCompletedEventArgs args)
        {
            try
            {
                if (args.Result != null)
                {
                    this.isRegistered = true;

                    String stringResponse = Encoding.ASCII.GetString(args.Result);

                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                    xmlDoc.LoadXml(stringResponse);

                    // Okay we're hooked in.
                    if (xmlDoc.SelectSingleNode("/etResponse/status").InnerText.Equals("OK"))
                    {
                        
                        // we have work to do lets get the party started!
                        // init the work runner obj with the test run data.

                        XmlNodeList testRuns = xmlDoc.SelectNodes("/etResponse/Runs/CTM_Test_Run_Browser");

                        if (testRuns.Count > 0)
                        {
                            this.log.message("we have work todo: " + testRuns.Count + " test runs");
                        }
                        else
                        {
                            this.log.message("we have no work to do");
                        }

                        foreach (XmlNode testRun in testRuns)
                        {
                            if (this.ctmBgWorker.IsBusy != true)
                            {

                                CTM_Work_Runner ctmWorkRunner = new CTM_Work_Runner();

                                // Convert the XML document into a ctmWorkRunner object.
                                ctmWorkRunner.testRunId = UInt64.Parse(testRun.SelectSingleNode("testRunId").InnerText);
                                ctmWorkRunner.testRunBrowserId = UInt64.Parse(testRun.SelectSingleNode("id").InnerText);

                                XmlNode ctmTestBrowser = testRun.SelectSingleNode("CTM_Test_Browser");
                                ctmWorkRunner.testBrowser = ctmTestBrowser.SelectSingleNode("name").InnerText;

                                ctmWorkRunner.useVerboseTestLogs = this.useVerboseTestLogs;

                                // create the download url.
                                ctmWorkRunner.testDownloadUrl = "http://" + this.ctmHostname + "/test/run/download/?id=" + ctmWorkRunner.testRunId;

                                this.ctmBgWorker.RunWorkerAsync(ctmWorkRunner);

                                this.log.message(" testRunId: " + ctmWorkRunner.testRunId.ToString());
                                this.log.message(" testRunBrowserId: " + ctmWorkRunner.testRunBrowserId.ToString());
                                this.log.message(" testDownloadUrl: " + ctmWorkRunner.testDownloadUrl);
                                this.log.message(" testBrowser: " + ctmWorkRunner.testBrowser);
                            }
                            else
                            {
                                this.log.message("ctmBgWorker is busy, we will come back later.");
                            }
                        
                        }

                    }
                    else
                    {
                        this.log.message("No work for us");
                    }
                    return;
                }
                this.isRegistered = false;
            }
            catch (Exception ex)
            {
                this.log.message("connection issue caught: " + ex.Message);
            }
        }

        void ctmBgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
            CTM_Work_Runner ctmWorkRunner = (CTM_Work_Runner)e.Argument;

            try
            {
                
                ctmWorkRunner.runWork();

                NameValueCollection resultPostValues = new NameValueCollection();
                resultPostValues.Add("testRunBrowserId", ctmWorkRunner.testRunBrowserId.ToString());
                // TODO: jeo - we need to make a better timeElapsed tracker.
                // resultPostValues.Add("testDuration", workRunnerObj.timeElapsed.ToString());
                resultPostValues.Add("testStatus", ctmWorkRunner.testStatus.ToString());
                resultPostValues.Add("runLog", "");
                resultPostValues.Add("seleniumLog", ctmWorkRunner.seleniumTestLog.getLogContents());

                String logUrl = "http://" + this.ctmHostname + "/et/log/";
                WebClient resultClient = new WebClient();
                resultClient.UploadValues(logUrl, resultPostValues);

                this.log.message("Completed test run: " + ctmWorkRunner.testRunId);
            }
            catch (Exception ex)
            {
                this.log.message("uncaught run work exception message: " + ex.Message);
            }
            finally
            {
                ctmWorkRunner.cleanup();
            }
        }

    }


}
