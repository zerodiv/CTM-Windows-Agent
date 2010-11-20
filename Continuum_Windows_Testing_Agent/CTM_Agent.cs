using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Collections.Specialized;
using System.Xml;
using OpenQA.Selenium;

namespace Continuum_Windows_Testing_Agent
{
    public partial class CTM_Agent : Form
    {

        #region Private Variables
        delegate void SetLastRunLogBoxCallback(string text);
        delegate void ctmTestExecuteCallback();

        private CTM_Agent_Log log;
        private String guid;
        private String ctmHostname;
        private String localIp;
        private String machineName;
        private Boolean isRegistered;
        private WebClient ctmClient;
        private Boolean haltOnError;

        private Dictionary<string, CTM_WebBrowser> webBrowsers;
  
        private CTM_Test currentTest;

        #endregion Private Variables

        #region Constructor
        public CTM_Agent()
        {

            this.guid = null;
            this.ctmHostname = "";
            this.localIp = "";
            this.machineName = "";
            this.isRegistered = false;
            this.haltOnError = false;

            // initalize the logging for the phone home agent.
            this.log = new CTM_Agent_Log();

            this.isRegistered = false;

            // init the various browsers
            this.webBrowsers = new Dictionary<string, CTM_WebBrowser>();

            this.webBrowsers.Add("googlechrome", CTM_WebBrowser_Factory.factory("googlechrome") );
            this.webBrowsers.Add("firefox", CTM_WebBrowser_Factory.factory("firefox") );
            this.webBrowsers.Add("iexplore", CTM_WebBrowser_Factory.factory("iexplore"));
            
            // These phones are not local, so therefor we have stored registry keys for their settings.
            // Disabling the droid for now.
            // this.webBrowsers.Add("android", CTM_WebBrowser_Factory.factory("android"));
            
            this.webBrowsers.Add("iphone", CTM_WebBrowser_Factory.factory("iphone"));

            this.loadRegistryKeys();
            
            // attempt to connect to the settings if we have any.
            // if (this.webBrowsers["android"].getHostname().Length > 0)
            //{
            //    this.webBrowsers["android"].verify();
            //}
            
            if (this.webBrowsers["iphone"].getHostname().Length > 0)
            {
                this.webBrowsers["iphone"].verify();
            }
                                   
            this.currentTest = null;

            InitializeComponent();

            this.ctmClient = new WebClient();
            this.ctmClient.UploadValuesCompleted += new UploadValuesCompletedEventHandler(requestWork_Completed);

            this.agentBackgroundWorker.DoWork += new DoWorkEventHandler(ctmAgentBackgroundWorker_DoWork);

            
            
        }
        #endregion Constructor

        #region Registry Settings
        private void loadRegistryKeys()
        {
            
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\CTM");

            if (key.GetValue("guid") != null)
            {
                this.guid = key.GetValue("guid").ToString();
            }

            if (this.guid == "" || this.guid == null )
            {
                this.guid = System.Guid.NewGuid().ToString();
                key.SetValue("guid", this.guid);
            }
            
            if (key.GetValue("ctmHostname") != null)
            {
                this.ctmHostname = key.GetValue("ctmHostname").ToString();
            }

            if (key.GetValue("localIp") != null)
            {
                this.localIp = key.GetValue("localIp").ToString();
            }

            if (this.localIp == "")
            {
                String ip = "";

                IPAddress[] localIps = Dns.GetHostAddresses(Dns.GetHostName());

                foreach (IPAddress hostIp in localIps)
                {
                    if (!IPAddress.IsLoopback(hostIp) && hostIp.AddressFamily.ToString() == "InterNetwork" )
                    {
                        ip = hostIp.ToString();
                    }
                }

                if (ip.Length > 0)
                {
                    this.localIp = ip;
                    key.SetValue("localIp", this.localIp);
                }
            }

            if (key.GetValue("machineName") != null )
            {
                this.machineName = key.GetValue("machineName").ToString();
            }

            if (this.machineName == "")
            {
                this.machineName = Environment.MachineName;

                if (machineName.Length > 0)
                {
                    key.SetValue("machineName", this.machineName);
                }
            }

            if (key.GetValue("iphoneHostname") != null)
            {
                this.webBrowsers["iphone"].setHostname(key.GetValue("iphoneHostname").ToString());
                this.webBrowsers["iphone"].setPort(Int32.Parse(key.GetValue("iphonePort").ToString()));
            }

            /*
            if (key.GetValue("androidHostname") != null)
            {
                this.webBrowsers["android"].setHostname(key.GetValue("androidHostname").ToString());
                this.webBrowsers["android"].setPort(Int32.Parse(key.GetValue("androidPort").ToString()));
            }
            */

        }

        public void saveRegistryKeys()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\CTM");
            key.SetValue("guid", this.guid);
            key.SetValue("ctmHostname", this.ctmHostname);
            key.SetValue("localIp", this.localIp);
            key.SetValue("machineName", this.machineName);
            
            // iphone 
            key.SetValue("iphoneHostname", this.webBrowsers["iphone"].getHostname());
            key.SetValue("iphonePort", this.webBrowsers["iphone"].getPort());

            /*
            // android
            key.SetValue("androidHostname", this.webBrowsers["android"].getHostname());
            key.SetValue("androidPort", this.webBrowsers["android"].getPort());
            */

        }
        #endregion Registry Settings

        #region Helper Functions
        private string getCTMBuild()
        {
            string ctmBuild;

            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                ctmBuild = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            else
            {
                ctmBuild = "0.0.0";
            }
            return ctmBuild;
        }

        private String determineWindowsVersion()
        {
            
            Platform p = Platform.CurrentPlatform;
            return p.Type.ToString();

        }
        #endregion Helper Functions

        #region Form Load
        private void CTM_Agent_Load_1(object sender, EventArgs e)
        {

            this.localIpBox.Text = this.localIp;
            this.ctmHostnameBox.Text = this.ctmHostname;
            this.guidBox.Text = this.guid;
            this.machineNameBox.Text = this.machineName;
            
            this.Text = "Continuum Testing Agent - " + this.getCTMBuild();

            this.osVersionBox.Text = this.determineWindowsVersion();

            // Load up the inital state of all the browsers available to this box.

            // Load up IE.
            foreach (string browserName in this.webBrowsers.Keys)
            {
                Boolean canEdit = false;

                if (this.webBrowsers[browserName].getIsRemote() == true)
                {
                    canEdit = true;
                }

                this.addBrowserToGrid(this.webBrowsers[browserName], canEdit);
            }
        }

        private void addBrowserToGrid(CTM_WebBrowser browser, Boolean canEdit)
        {
            String bHostname = browser.getHostname();
            String bPort = browser.getPort().ToString();

            if (browser.getIsRemote() == false)
            {
                bHostname = "";
                bPort = "";
            }

            int bRowOffset = browser.getGridRowId();

            if (bRowOffset >= 0)
            {
                this.browserGrid.Rows[bRowOffset].Cells["isAvailable"].Value = browser.getIsAvailable().ToString();
                this.browserGrid.Rows[bRowOffset].Cells["browserName"].Value = browser.getPrettyName();
                this.browserGrid.Rows[bRowOffset].Cells["browserVersion"].Value = browser.getVersion();
            }
            else
            {
                bRowOffset = this.browserGrid.Rows.Add(new string[] {
                browser.getIsAvailable().ToString(),  // available.
                browser.getPrettyName(),        // browser name
                browser.getVersion(),       // browser version
                bHostname,
                bPort,
                browser.getInternalName()
                });
            }

            if (canEdit != true)
            {
                this.browserGrid.Rows[bRowOffset].ReadOnly = true;
            }

            if (browser.getIsAvailable())
            {
                this.browserGrid.Rows[bRowOffset].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
            }
            else
            {
                this.browserGrid.Rows[bRowOffset].DefaultCellStyle.BackColor = System.Drawing.Color.LightSteelBlue;
            }

            browser.setGridRowId(bRowOffset);

        }

        #endregion Form Load

        #region Button Actions
        private void configSaveSettingsBtn_Click(object sender, EventArgs e)
        {

            this.ctmHostname = this.ctmHostnameBox.Text;
            this.localIp = this.localIpBox.Text;
            this.machineName = this.machineNameBox.Text;

            // set the entries for the iphone and android browsers.
            foreach (string browserName in this.webBrowsers.Keys)
            {
                if (this.webBrowsers[browserName].getIsRemote() == true && this.webBrowsers[browserName].getHostname().Length > 0 )
                {
                    // Suck in the settings from the datagrid.
                    int gridId = this.webBrowsers[browserName].getGridRowId();
                    string hostname = this.browserGrid.Rows[gridId].Cells["hostname"].Value.ToString();
                    int port = System.Int32.Parse(this.browserGrid.Rows[gridId].Cells["port"].Value.ToString());
                    this.webBrowsers[browserName].setHostname(hostname);
                    this.webBrowsers[browserName].setPort(port);

                    // Verify that the browser is available.
                    this.webBrowsers[browserName].verify();

                    // Add it to the grid / refresh the value.
                    this.addBrowserToGrid(this.webBrowsers[browserName], true);

                    // If it failed to verify, notify the user via the log entries.
                    if (this.webBrowsers[browserName].getIsAvailable() == false)
                    {
                        this.log.message(
                            "Unable to connect to " +
                                this.webBrowsers[browserName].getPrettyName() +
                            " webdriver service on: " +
                                this.webBrowsers[browserName].getHostname() +
                                ":" + this.webBrowsers[browserName].getPort()
                        );
                        this.updateLastRunLogBox(this.log.getLastLogLines());
                        return;
                    }
                }
            }

            this.saveRegistryKeys();

            this.callHomeTimer.Interval = 1;

        }

        private void regenerateGuidBtn_Click(object sender, EventArgs e)
        {
           Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\CTM");
           this.guid = System.Guid.NewGuid().ToString(); ;
           this.guidBox.Text = this.guid;
           key.SetValue("guid", this.guid);
        }

        private void forcePollBtn_Click(object sender, EventArgs e)
        {
            this.callHomeTimer.Interval = 1;
        }

        private void haltOnErrorBox_CheckedChanged(object sender, EventArgs e)
        {
            this.haltOnError = this.haltOnErrorBox.Checked;
        }

        #endregion Button Actions

        private Boolean readyToAttatchToServer()
        {
            String message = "";
            Boolean isReady = true;

            if (this.guid.Length == 0 && isReady == true )
            {
                message = "Missing guid, please click on regenerate.";
                isReady = false;
            }

            if (this.ctmHostname.Length == 0 && isReady == true)
            {
                message = "Please provide a CTM server name.";
                isReady = false;
            }

            if (localIp.Length == 0 && isReady == true)
            {
                message = "Unable to determin our local IP.";
                isReady = false;
            }

            if (machineName.Length == 0 && isReady == true)
            {
                message = "Please provide a name for this host.";
                isReady = false;
            }

            if (isReady == false)
            {
                this.log.message(message);
                this.ctmStatusLabel.Text = message;
                return isReady;
            }

            return true;
        }

        private void updateLastRunLogBox(String text)
        {
            if (this.lastRunLogBox.InvokeRequired == true)
            {
                SetLastRunLogBoxCallback d = new SetLastRunLogBoxCallback(updateLastRunLogBox);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lastRunLogBox.Text = text;
                // this.log.getLastLogLines();
            }
        }

        private void ctmTestExecute()
        {
            if (this.currentTest.InvokeRequired == true)
            {
                ctmTestExecuteCallback d = new ctmTestExecuteCallback(ctmTestExecute);
                this.Invoke(d, new object[] { });
            }
            else
            {
                this.currentTest.Activate();
                this.currentTest.runWork();
            }
        }

        #region Call Home Timer
        private void callHomeTimer_Tick(object sender, EventArgs e)
        {
            // Check to see if we are already doing work, otherwise reset our poll interval to 30s
            if (this.agentBackgroundWorker.IsBusy == true || this.currentTest != null )
            {
                String message = "Running test: " + this.currentTest.getTestRunId() + " going to sleep for 5 minutes";
                this.log.message(message);
                this.ctmStatusLabel.Text = message;
                this.callHomeTimer.Interval = 60 * 5 * 1000;
                return;
            }
            else
            {
                // We get noisy when we're not loaded.
                this.callHomeTimer.Interval = 30000;
            }

            // see if we're ready to talk to the server. 
            if (!this.readyToAttatchToServer())
            {
                return;
            }

            this.requestWork();
        }
        #endregion Call Home Timer

        void testRunWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.currentTest.getSeleniumTestLog().closeLogFile();

                NameValueCollection resultPostValues = new NameValueCollection();
                resultPostValues.Add("testRunBrowserId", this.currentTest.getTestRunBrowserId().ToString());
                // TODO: jeo - we need to make a better timeElapsed tracker.
                // resultPostValues.Add("testDuration", workRunnerObj.timeElapsed.ToString());
                resultPostValues.Add("testStatus", this.currentTest.getTestHadError().ToString());
                resultPostValues.Add("runLog", "");
                resultPostValues.Add("seleniumLog", this.currentTest.getSeleniumTestLog().getLogContents());

                String logUrl = "http://" + this.ctmHostname + "/et/log/";
                WebClient resultClient = new WebClient();
                resultClient.UploadValues(logUrl, resultPostValues);

                this.log.message("Completed test run: " + this.currentTest.getTestRunId());
            }
            catch (Exception ex)
            {
                this.log.message("uncaught run work exception message: " + ex.Message);
            }
            finally
            {
                this.updateLastRunLogBox(this.log.getLastLogLines());

                this.currentTest.cleanup();
                
                this.currentTest = null;

            }               
        }

        void ctmAgentBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.ctmTestExecute();
        }

        private void requestWork_Completed(object sender, UploadValuesCompletedEventArgs args)
        {
            try
            {
                if ( this.agentBackgroundWorker.IsBusy != true && this.currentTest == null ) {
                }
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

                            if (this.agentBackgroundWorker.IsBusy != true && this.currentTest == null )
                            {

                                this.currentTest = new CTM_Test();

                                // Convert the XML document into a ctmWorkRunner object.
                                this.currentTest.setTestRunId( UInt64.Parse(testRun.SelectSingleNode("testRunId").InnerText) );
                                this.currentTest.setTestRunBrowserId( UInt64.Parse(testRun.SelectSingleNode("id").InnerText) );

                                XmlNode ctmTestBrowser = testRun.SelectSingleNode("CTM_Test_Browser");

                                this.currentTest.setTestBrowser(this.webBrowsers[ctmTestBrowser.SelectSingleNode("name").InnerText]);

                                // create the download url.
                                this.currentTest.setTestDownloadUrl( "http://" + this.ctmHostname + "/test/run/download/?id=" + this.currentTest.getTestRunId() );
                                
                                this.currentTest.setHaltOnError( this.haltOnError );

                                this.log.message(" testRunId: " + this.currentTest.getTestRunId());
                                this.log.message(" testRunBrowserId: " + this.currentTest.getTestRunBrowserId());
                                this.log.message(" testDownloadUrl: " + this.currentTest.getTestDownloadUrl());
                                this.log.message(" testBrowser: " + this.currentTest.getTestBrowser());

                                this.currentTest.Visible = true;

                                this.currentTest.testRunWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(testRunWorker_RunWorkerCompleted);
                    
                                this.agentBackgroundWorker.RunWorkerAsync();

                            }
                            else
                            {
                                this.log.message("ctmBgWorker is busy with: " + this.currentTest.getTestRunId() + " we will come back later to run: " + testRun.SelectSingleNode("testRunId").InnerText);
                            }

                        }

                    }
                    else
                    {
                        this.log.message("No work for us");
                    }

                    // this.updateLastRunLogBox();

                }
                else
                {
                    this.isRegistered = false;
                }
               
            }
            catch (Exception ex)
            {
                this.log.message("connection issue caught: " + ex.Message);
            }

            if (this.isRegistered == true)
            {
                DateTime now = DateTime.Now;
                this.ctmStatusLabel.Text = "Last check in: " + String.Format("{0:r}", now);
            }

            this.updateLastRunLogBox( this.log.getLastLogLines() );

        }

        public void requestWork()
        {

            if (this.readyToAttatchToServer() != true)
            {
                this.log.message("We are not ready to attatch to a CTM server.");
                return;
            }

            if (this.ctmClient.IsBusy == true)
            {
                this.log.message("Agent Busy: We are already requesting work.");
                return;
            }

            if (this.agentBackgroundWorker.IsBusy == true)
            {
                this.log.message("Agent is actively running a test");
                return;
            }

            if (this.currentTest != null)
            {
                this.log.message("Agent is actively running a test");
                return;
            }

            this.ctmStatusLabel.Text = "Phoning home now";
           
            NameValueCollection postValues = new NameValueCollection();

            postValues.Add("guid", this.guid);

            postValues.Add("ip", this.localIp);

            postValues.Add("os", this.determineWindowsVersion());

            postValues.Add("machine_name", this.machineName);

            // add the browsers into our post params
            foreach (string browserName in this.webBrowsers.Keys)
            {
                if (this.webBrowsers[browserName].getIsAvailable() == true)
                {
                    postValues.Add(browserName, "yes");
                    postValues.Add(browserName + "_version", this.webBrowsers[browserName].getVersion());
                }
            }

            String pollUrl = "http://" + this.ctmHostname + "/agent/poll/";
            ctmClient.UploadValuesAsync(new Uri(pollUrl), postValues);

        }

        

    }
}
