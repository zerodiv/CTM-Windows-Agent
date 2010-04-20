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

namespace Continuum_Windows_Testing_Agent
{
    class LocalWebBrowser {
        public Boolean exists;
        public int major;
        public int minor;
        public int patch;

        public LocalWebBrowser() {
            this.exists = false;
            this.major = 0;
            this.minor = 0;
            this.patch = 0;
        }

        public String getVersion() {
            if (this.exists)
            {
                String version = "";
                version += this.major;
                version += "." + this.minor;
                version += "." + this.patch;
                return version;
            }
            return "Not Available";
        }

    }

    class PhoneHomeAgent
    {
        public LocalWebBrowser chrome;
        public LocalWebBrowser firefox;
        public LocalWebBrowser ie;
        public LocalWebBrowser safari;

        public PhoneHomeAgent()
        {
            // init the various browsers
            this.chrome = new LocalWebBrowser();
            this.firefox = new LocalWebBrowser();
            this.ie = new LocalWebBrowser();
            this.safari = new LocalWebBrowser();

            this.findIEBrowser();
            this.findChromeBrowser();
            this.findFirefoxBrowser();
            this.findSafariBrowser();

        }

        public String determineWindowsVersion()
        {
            String windowsVersion = "";
            windowsVersion += OSInfo.Name + " - " + OSInfo.Edition + " - " + OSInfo.ServicePack;
            return windowsVersion;
        }

        public Boolean registerHost( String guid, String masterUrl, String localIp ) {
            // hostname
            // ip - port
            // os
            // browser => yes
            // browser_version => xyz

            if (masterUrl.Length == 0)
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

            postValues.Add("ip", localIp );

            postValues.Add("os", this.determineWindowsVersion() );
            
            // add the browsers into our post params
            if (this.ie.exists == true)
            {
                postValues.Add("ie", "yes");
                postValues.Add("ie_version", this.ie.getVersion());
            }

            if (this.chrome.exists == true)
            {
                postValues.Add("chrome", "yes");
                postValues.Add("chrome_version", this.chrome.getVersion());
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
                masterClient.UploadValues(masterUrl, postValues);
            }
            catch (WebException e)
            {
                // We should do something with e
                if ( e.Message.Equals( "" ) ) {
                    return false;
                }
                return false;
            }
            return true;
        }

        public void findSafariBrowser()
        {
            RegistryKey dkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Apple Computer, Inc.\\Safari");
            if (dkey != null)
            {
                string bVersion = dkey.GetValue("Version").ToString();
                if (bVersion != null)
                {
                    Regex versionRegex = new Regex(@"(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)\.\d+");
                    Match versionMatch = versionRegex.Match(bVersion);
                    if (versionMatch != null)
                    {
                        this.safari.exists = true;
                        this.safari.major = Convert.ToInt32(versionMatch.Groups["major"].Value);
                        this.safari.minor = Convert.ToInt32(versionMatch.Groups["minor"].Value);
                        this.safari.patch = Convert.ToInt32(versionMatch.Groups["patch"].Value);
                    }
                }
            }
        }

        public void findChromeBrowser()
        {
            DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Google\\Chrome\\Application");
            DirectoryInfo[] dirs = di.GetDirectories("*.*.*.*");
            Regex versionRegex = new Regex(@"(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)\.\d+");
                    
            foreach (DirectoryInfo diNext in dirs)
            {
               Match versionMatch = versionRegex.Match(diNext.Name);
               if (versionMatch != null)
               {
                   if (this.chrome.exists == true)
                   {
                       // TODO: Need to bring the version logic in.
                       
                   }
                   else
                   {
                       this.chrome.exists = true;
                       this.chrome.major = Convert.ToInt32(versionMatch.Groups["major"].Value);
                       this.chrome.minor = Convert.ToInt32(versionMatch.Groups["minor"].Value);
                       this.chrome.patch = Convert.ToInt32(versionMatch.Groups["patch"].Value);
                   }
               }
            }
        }

        public void findFirefoxBrowser()
        {
            RegistryKey dkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Mozilla\\Mozilla Firefox");
            if (dkey != null)
            {
                string bVersion = dkey.GetValue("CurrentVersion").ToString();
                if (bVersion != null)
                {
                    Regex versionRegex = new Regex(@"(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)");
                    Match versionMatch = versionRegex.Match(bVersion);
                    if (versionMatch != null)
                    {
                        this.firefox.exists = true;
                        this.firefox.major = Convert.ToInt32(versionMatch.Groups["major"].Value);
                        this.firefox.minor = Convert.ToInt32(versionMatch.Groups["minor"].Value);
                        this.firefox.patch = Convert.ToInt32(versionMatch.Groups["patch"].Value);
                    }
                }
            }
        }

        public void findIEBrowser()
        {
            
            RegistryKey dkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer");
            if (dkey != null)
            {
                string bVersion = dkey.GetValue("Version").ToString();
                if (bVersion != null)
                {
                    Regex versionRegex = new Regex( @"(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)" );
                    Match versionMatch = versionRegex.Match(bVersion);
                    if ( versionMatch != null ) {
                        this.ie.exists = true;
                        this.ie.major = Convert.ToInt32(versionMatch.Groups["major"].Value);
                        this.ie.minor = Convert.ToInt32(versionMatch.Groups["minor"].Value);
                        this.ie.patch = Convert.ToInt32(versionMatch.Groups["patch"].Value);
                    }
                }
            }
        }

    }


}
