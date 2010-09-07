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
        private BackgroundWorker ctmBgWorker;
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
            this.ctmBgWorker = new BackgroundWorker();

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

        private void registerHost_Completed(object sender, UploadValuesCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    this.isRegistered = true;
                    return;
                }
                this.isRegistered = false;
            }
            catch (Exception ex)
            {
                this.log.message("connection issue caught: " + ex.Message);
            }
        }

        public void run()
        {
            try
            {
                this.registerHost();
                this.requestWork();
            }
            catch (Exception e ) 
            {
                // Yes this is jorcutt being paranoid about exceptions bubbling up and crashing the application.
                this.log.message("generic exception caught in Agent::run() message: " + e.Message);
            }
        }

        public Boolean registerHost()
        {
            if (this.isInitalized() != true)
            {
                return false;
            }

            // Client is busy don't try to request again.
            if (this.ctmClient.IsBusy == true)
            {
                return false;
            }

            if (this.isRegistered == true)
            {
                return true;
            }

            if (this.ctmBgWorker.IsBusy == true)
            {
                return false;
            }

            // We register every time so that if the master falls off the map we can continue
            // to operate correctly.

            ctmClient.UploadValuesCompleted += new UploadValuesCompletedEventHandler(registerHost_Completed);
            
            NameValueCollection postValues = new NameValueCollection();

            postValues.Add("guid", this.guid);

            postValues.Add("ip", this.localIp);

            postValues.Add("os", this.determineWindowsVersion());

            postValues.Add("machine_name", this.machineName);

            // add the browsers into our post params
            if (this.ie.exists == true)
            {
                postValues.Add("ie", "yes");
                postValues.Add("ie_version", this.ie.getVersion());
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

            String registerUrl = "http://" + this.ctmHostname + "/et/phone/home/1.0/";
            ctmClient.UploadValuesAsync(new Uri(registerUrl), postValues);
            
            return true;
        }

        private void requestWork_Completed(object sender, UploadValuesCompletedEventArgs args)
        {
            try
            {
                if (this.ctmBgWorker.IsBusy)
                {
                    // TODO: Watch this FIXME for coming up alot, since if it does we have to look at race states in our timers.
                    this.log.message("FIXME: Request work fired, even though a processing work.");
                    return;
                }

                if (args.Result != null)
                {
                    String stringResponse = Encoding.ASCII.GetString(args.Result);

                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

                    xmlDoc.LoadXml(stringResponse);

                    if (xmlDoc.SelectSingleNode("/etResponse/status").InnerText.Equals("OK"))
                    {

                        

                        // TODO: JEO - We might not need this.
                        // this.ctmBgWorker.WorkerSupportsCancellation = true;

                        // this.ctmBgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ctmBgWorker_RunWorkerCompleted);
                        this.ctmBgWorker.DoWork += new DoWorkEventHandler(ctmBgWorker_DoWork);


                        this.log.message("we have work todo");

                        // we have work to do lets get the party started!
                        // init the work runner obj with the test run data.


                       CTM_Work_Runner ctmWorkRunner = new CTM_Work_Runner();

                       ctmWorkRunner.testRunId = UInt64.Parse(xmlDoc.SelectSingleNode("/etResponse/testRunId").InnerText);
                       ctmWorkRunner.testRunBrowserId = UInt64.Parse(xmlDoc.SelectSingleNode("/etResponse/testRunBrowserId").InnerText);
                       ctmWorkRunner.testDownloadUrl = xmlDoc.SelectSingleNode("/etResponse/downloadUrl").InnerText;
                       ctmWorkRunner.testBrowser = xmlDoc.SelectSingleNode("/etResponse/testBrowser").InnerText;
                       ctmWorkRunner.testBaseurl = xmlDoc.SelectSingleNode("/etResponse/testBaseurl").InnerText;
                       ctmWorkRunner.useVerboseTestLogs = this.useVerboseTestLogs;

                        this.log.message(" testRunId: " + ctmWorkRunner.testRunId.ToString());
                        this.log.message(" testRunBrowserId: " + ctmWorkRunner.testRunBrowserId.ToString());
                        this.log.message(" testDownloadUrl: " + ctmWorkRunner.testDownloadUrl);
                        this.log.message(" testBrowser: " + ctmWorkRunner.testBrowser);
                        this.log.message(" testBaseurl: " + ctmWorkRunner.testBaseurl);

                        this.ctmBgWorker.RunWorkerAsync(ctmWorkRunner);

                    }
                    else
                    {
                        this.log.message("no work for us");
                    }



                }
            }
            catch (Exception e)
            {
                this.log.message("Fetch work failed message: " + e.Message);
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

        public Boolean requestWork()
        {

            if (this.isInitalized() != true)
            {
                return false;
            }

            if (this.isRegistered != true)
            {
                return false;
            }

            // Do not queue up another piece of work until the previous is done.
            if (this.ctmClient.IsBusy == true)
            {
                return false;
            }

            if (this.ctmBgWorker.IsBusy == true)
            {
                return false;
            }

            this.ctmClient.UploadValuesCompleted += new UploadValuesCompletedEventHandler(requestWork_Completed);

            NameValueCollection postValues = new NameValueCollection();

            postValues.Add("guid", this.guid);

            try
            {
                String pollUrl = "http://" + this.ctmHostname + "/et/poll/1.0/";
                this.log.message("requesting from: " + pollUrl);
                ctmClient.UploadValuesAsync(new Uri(pollUrl), postValues);
            }
            catch (WebException e)
            {
                this.log.message("Failed to do request for work: " + e.Message);
            }

            return true;
        }

    }


}
