using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.IO;

namespace Continuum_Windows_Testing_Agent
{
    public class CTM_LocalWebBrowser
    {
        public Boolean exists;
        public int major;
        public int minor;
        public int patch;

        public CTM_LocalWebBrowser(String browserType)
        {

            this.exists = false;
            this.major = 0;
            this.minor = 0;
            this.patch = 0;
            
            if (browserType == "safari")
            {
                this.findSafariBrowser();
            } else if (browserType == "googlechrome")
            {
                this.findGoogleChromeBrowser();
            } else if (browserType == "firefox")
            {
                this.findFirefoxBrowser();
            }
            else if (browserType == "ie")
            {
                this.findIEBrowser();
            }
            else
            {
                throw new Exception("We expect a supported browser string [safari, googlechrome, firefox, ie]");
            }


        }

        public String getVersion()
        {
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

        private void findSafariBrowser()
        {
            RegistryKey dkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Apple Computer, Inc.\\Safari");
            if (dkey != null)
            {
                string bVersion = dkey.GetValue("Version").ToString();
                if (bVersion != null)
                {
                    Regex versionRegex = new Regex(@"(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)\.\d+");
                    Match versionMatch = versionRegex.Match(bVersion);
                    if (versionMatch.Success)
                    {
                        this.exists = true;
                        this.major = Convert.ToInt32(versionMatch.Groups["major"].Value);
                        this.minor = Convert.ToInt32(versionMatch.Groups["minor"].Value);
                        this.patch = Convert.ToInt32(versionMatch.Groups["patch"].Value);
                    }
                }
            }
        }

        private void findGoogleChromeBrowser()
        {
            try
            {

                DirectoryInfo di = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Google\\Chrome\\Application");

                DirectoryInfo[] dirs = di.GetDirectories("*.*.*.*");

                Regex versionRegex = new Regex(@"(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)\.\d+");

                foreach (DirectoryInfo diNext in dirs)
                {
                    Match versionMatch = versionRegex.Match(diNext.Name);
                    if (versionMatch.Success)
                    {
                        this.exists = true;
                        this.major = Convert.ToInt32(versionMatch.Groups["major"].Value);
                        this.minor = Convert.ToInt32(versionMatch.Groups["minor"].Value);
                        this.patch = Convert.ToInt32(versionMatch.Groups["patch"].Value);

                    }
                }
            }
            catch
            {
                this.exists = false;
            }

        }

        private void findFirefoxBrowser()
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
                        this.exists = true;
                        this.major = Convert.ToInt32(versionMatch.Groups["major"].Value);
                        this.minor = Convert.ToInt32(versionMatch.Groups["minor"].Value);
                        this.patch = Convert.ToInt32(versionMatch.Groups["patch"].Value);
                    }
                }
            }
        }

        private void findIEBrowser()
        {

            RegistryKey dkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Internet Explorer");
            if (dkey != null)
            {
                string bVersion = dkey.GetValue("Version").ToString();
                if (bVersion != null)
                {
                    Regex versionRegex = new Regex(@"(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)");
                    Match versionMatch = versionRegex.Match(bVersion);
                    if (versionMatch.Success)
                    {
                        this.exists = true;
                        this.major = Convert.ToInt32(versionMatch.Groups["major"].Value);
                        this.minor = Convert.ToInt32(versionMatch.Groups["minor"].Value);
                        this.patch = Convert.ToInt32(versionMatch.Groups["patch"].Value);
                    }
                }
            }
        }
    }
}
