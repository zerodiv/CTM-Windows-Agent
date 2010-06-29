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
using Ionic.Zip;

namespace Continuum_Windows_Testing_Agent
{

    class PhoneHomeAgent
    {
        public LocalWebBrowser googlechrome;
        public LocalWebBrowser firefox;
        public LocalWebBrowser ie;
        public LocalWebBrowser safari;
        public Boolean hasSeleniumServerJarFile;
        public AgentLog log;

        public PhoneHomeAgent()
        {

            // initalize the logging for the phone home agent.
            this.log = new AgentLog();

            // init the various browsers
            this.googlechrome = new LocalWebBrowser( "googlechrome" );
            this.firefox = new LocalWebBrowser( "firefox" );
            this.ie = new LocalWebBrowser( "ie" );
            this.safari = new LocalWebBrowser( "safari" );

        }

        public String determineWindowsVersion()
        {
            String windowsVersion = "";
            windowsVersion += OSInfo.Name + " - " + OSInfo.Edition + " - " + OSInfo.ServicePack;
            return windowsVersion;
        }

        public Boolean registerHost(String guid, String masterHostname, String localIp)
        {
            // hostname
            // ip - port
            // os
            // browser => yes
            // browser_version => xyz

            if (masterHostname.Length == 0)
            {
                return false;
            }

            if (localIp.Length == 0)
            {
                return false;
            }

            WebClient masterClient = new WebClient();

            NameValueCollection postValues = new NameValueCollection();

            postValues.Add("guid", guid);

            postValues.Add("ip", localIp);

            postValues.Add("os", this.determineWindowsVersion());

            postValues.Add("machine_name", Environment.MachineName);

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

            try
            {
                String registerUrl = "http://" + masterHostname + "/et/phone/home/1.0/";
                masterClient.UploadValues(registerUrl, postValues);
            }
            catch (WebException e)
            {
                // We should do something with e
                if (e.Message.Equals(""))
                {
                    return false;
                }
                return false;
            }
            return true;
        }






        public Boolean requestWork(String guid, String masterHostname)
        {

            this.log.reset();

            WebClient masterClient = new WebClient();

            NameValueCollection postValues = new NameValueCollection();

            postValues.Add("guid", guid);

            String stringResponse = "";
            try
            {

                String pollUrl = "http://" + masterHostname + "/et/poll/1.0/";
                this.log.message("requesting from: " + pollUrl);

                byte[] response = masterClient.UploadValues(pollUrl, postValues);
                stringResponse = Encoding.ASCII.GetString(response);
            }
            catch (WebException e)
            {
                this.log.message("Failed to do request for work: " + e.Message);
            }

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();

            try
            {
                xmlDoc.LoadXml(stringResponse);
            }
            catch (Exception e)
            {
                return false;
            }
            
            if (xmlDoc.SelectSingleNode("/etResponse/status").InnerText.Equals("OK"))
            {

                this.log.message("we have work todo");

                // we have work to do lets get the party started!
                // init the work runner obj with the test run data.
                WorkRunner workRunnerObj = new WorkRunner(this.log);                       
           
                workRunnerObj.testRunId = UInt64.Parse(xmlDoc.SelectSingleNode("/etResponse/testRunId").InnerText);
                workRunnerObj.testRunBrowserId = UInt64.Parse(xmlDoc.SelectSingleNode("/etResponse/testRunBrowserId").InnerText);
                workRunnerObj.testDownloadUrl = xmlDoc.SelectSingleNode("/etResponse/downloadUrl").InnerText;
                workRunnerObj.testBrowser = xmlDoc.SelectSingleNode("/etResponse/testBrowser").InnerText;
                workRunnerObj.testBaseurl = xmlDoc.SelectSingleNode("/etResponse/testBaseurl").InnerText;
                                
                this.log.message(" testRunId: " + workRunnerObj.testRunId.ToString());
                this.log.message(" testRunBrowserId: " + workRunnerObj.testRunBrowserId.ToString());
                this.log.message(" testDownloadUrl: " + workRunnerObj.testDownloadUrl);
                this.log.message(" testBrowser: " + workRunnerObj.testBrowser);
                this.log.message(" testBaseurl: " + workRunnerObj.testBaseurl);

                workRunnerObj.runWork();
                workRunnerObj.cleanup();
                
                NameValueCollection resultPostValues = new NameValueCollection();
                resultPostValues.Add("testRunBrowserId", workRunnerObj.testRunBrowserId.ToString());
                resultPostValues.Add("testDuration", workRunnerObj.timeElapsed.ToString());
                resultPostValues.Add("testStatus", workRunnerObj.testStatus.ToString());
                resultPostValues.Add("runLog", workRunnerObj.testLog );
                resultPostValues.Add("seleniumLog", workRunnerObj.seleniumLog);
                String logUrl = "http://" + masterHostname + "/et/log/";
                masterClient.UploadValues(logUrl, resultPostValues);

            }
            else
            {
                this.log.message("no work for us");
            }

            return true;
        }



       



    }


}
