using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.IO;

namespace Continuum_Windows_Testing_Agent
{
    class CTM_WebBrowser_Factory
    {
        public static CTM_WebBrowser factory(String browserName)
        {

            switch (browserName)
            {
                case "safari":
                    return CTM_WebBrowser_Factory.findSafariBrowser();
                case "googlechrome":
                    return CTM_WebBrowser_Factory.findGoogleChromeBrowser();
                case "firefox":
                    return CTM_WebBrowser_Factory.findFirefoxBrowser();
                case "iexplore":
                    return CTM_WebBrowser_Factory.findIEBrowser();
                case "android":
                    return CTM_WebBrowser_Factory.findAndroidBrowser();
                case "iphone":
                    return CTM_WebBrowser_Factory.findIphoneBrowser();
            }

            throw new Exception("We expect a supported browser string [safari, googlechrome, firefox, ie]");

        }

        private static CTM_WebBrowser findAndroidBrowser()
        {
            CTM_WebBrowser browser = new CTM_WebBrowser();
            browser.setIsRemote(true);
            browser.setInternalName("android");
            browser.setPrettyName("Google Android");
            return browser;
        }

        private static CTM_WebBrowser findIphoneBrowser()
        {
            CTM_WebBrowser browser = new CTM_WebBrowser();
            browser.setIsRemote(true);
            browser.setInternalName("iphone");
            browser.setPrettyName("iPhone");
            return browser;
        }

        private static CTM_WebBrowser findSafariBrowser()
        {
            CTM_WebBrowser browser = new CTM_WebBrowser();
            browser.setInternalName("safari");
            browser.setPrettyName("Safari");

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
                        browser.setIsAvailable(true);
                        browser.setMajor(Convert.ToInt32(versionMatch.Groups["major"].Value));
                        browser.setMinor(Convert.ToInt32(versionMatch.Groups["minor"].Value));
                        browser.setPatch(Convert.ToInt32(versionMatch.Groups["patch"].Value));
                    }
                }
            }
            return browser;
        }


        private static CTM_WebBrowser findGoogleChromeBrowser()
        {
            CTM_WebBrowser browser = new CTM_WebBrowser();
            browser.setInternalName("googlechrome");
            browser.setPrettyName("Google Chrome");


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
                        browser.setIsAvailable(true);
                        browser.setMajor(Convert.ToInt32(versionMatch.Groups["major"].Value));
                        browser.setMinor(Convert.ToInt32(versionMatch.Groups["minor"].Value));
                        browser.setPatch(Convert.ToInt32(versionMatch.Groups["patch"].Value));
                    }
                }
            }
            catch
            {
                browser.setIsAvailable(false);
            }

            return browser;

        }

        private static String findFirefoxBrowserApplicationManifest()
        {
            // Hunt for a 64 bit version of firefox.
            String ff64bitApplicationFile = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\Mozilla Firefox\application.ini";

            if (File.Exists(ff64bitApplicationFile))
            {
                return ff64bitApplicationFile;
            }

            // Hunt for a 32 bit version of firefox.
            String ff32bitApplicationFile = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\Mozilla Firefox\application.ini";
            if (File.Exists(ff32bitApplicationFile))
            {
                return ff64bitApplicationFile;
            }

            return null;
        }

        private static CTM_WebBrowser findFirefoxBrowser()
        {

            CTM_WebBrowser browser = new CTM_WebBrowser();
            browser.setInternalName("firefox");
            browser.setPrettyName("Mozilla Firefox");

            String versionData = null;

            // Windows 7 Compatibility.
            String appManifestFile = CTM_WebBrowser_Factory.findFirefoxBrowserApplicationManifest();

            if (versionData == null && appManifestFile != null)
            {
                // Version=3.6.12
                CTM_Ini appManifest = new CTM_Ini(appManifestFile);
                versionData = appManifest.ReadValue("App", "Version");
            }

            RegistryKey dkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Mozilla\\Mozilla Firefox");
            if (versionData == null && dkey != null)
            {
                versionData = dkey.GetValue("CurrentVersion").ToString();
            }

            if (versionData != null)
            {
                Regex versionRegex = new Regex(@"(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)");
                Match versionMatch = versionRegex.Match(versionData);
                if (versionMatch != null)
                {
                    browser.setIsAvailable(true);
                    browser.setMajor(Convert.ToInt32(versionMatch.Groups["major"].Value));
                    browser.setMinor(Convert.ToInt32(versionMatch.Groups["minor"].Value));
                    browser.setPatch(Convert.ToInt32(versionMatch.Groups["patch"].Value));
                }
            }
            return browser;
        }

        private static CTM_WebBrowser findIEBrowser()
        {
            CTM_WebBrowser browser = new CTM_WebBrowser();
            browser.setInternalName("iexplore");
            browser.setPrettyName("Internet Explorer");

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
                        browser.setIsAvailable(true);
                        browser.setMajor(Convert.ToInt32(versionMatch.Groups["major"].Value));
                        browser.setMinor(Convert.ToInt32(versionMatch.Groups["minor"].Value));
                        browser.setPatch(Convert.ToInt32(versionMatch.Groups["patch"].Value));

                    }
                }
            }
            return browser;
        }

    }
}
