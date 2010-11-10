using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.IO;
using OpenQA.Selenium.Remote;

namespace Continuum_Windows_Testing_Agent
{
    public class CTM_WebBrowser
    {
        private Boolean isRemote;
        private Boolean isAvailable;
        private int major;
        private int minor;
        private int patch;
        private string internalName;
        private string prettyName;
        private string hostname;
        private int port;
        private int gridRowId;

        public CTM_WebBrowser()
        {
            this.isRemote = false;
            this.isAvailable = false;
            this.major = 0;
            this.minor = 0;
            this.patch = 0;
            this.internalName = "unknown";
            this.prettyName = "Unknown Browser";
            this.hostname = "";
            this.port = 8080;
            this.gridRowId = -1;

        }

        public Boolean getIsRemote()
        {
            return this.isRemote;
        }

        public void setIsRemote(Boolean isRemote)
        {
            this.isRemote = isRemote;
        }

        public String getInternalName()
        {
            return this.internalName;
        }

        public void setInternalName(String name)
        {
            this.internalName = name;
        }

        public Boolean getIsAvailable()
        {
            return this.isAvailable;
        }

        public void setIsAvailable(Boolean avail)
        {
            this.isAvailable = avail;
        }

        public String getPrettyName()
        {
            return this.prettyName;
        }

        public void setPrettyName(String name)
        {
            this.prettyName = name;
        }

        public void setMajor(int major)
        {
            this.major = major;
        }

        public void setMinor(int minor)
        {
            this.minor = minor;
        }

        public void setPatch(int patch)
        {
            this.patch = patch;
        }

        public String getVersion()
        {
            if (this.getIsAvailable())
            {
                String version = "";
                version += this.major;
                version += "." + this.minor;
                version += "." + this.patch;
                return version;
            }
            return "Not Available";
        }

        public string getHostname()
        {
            return this.hostname;
        }

        public void setHostname(string host)
        {
            this.hostname = host;
        }

        public int getPort()
        {
            return this.port;
        }

        public void setPort(int port)
        {
            this.port = port;
        }

        public int getGridRowId()
        {
            return this.gridRowId;
        }

        public void setGridRowId(int rowId)
        {
            this.gridRowId = rowId;
        }

        public void verify()
        {
            
            if ( this.getIsRemote() != true ) {
                return;
            }
            // in the case of remote browsers reverify that we are still existant
            try
            {
                DesiredCapabilities remoteCap = new DesiredCapabilities();
                // remoteCap.IsJavaScriptEnabled = true;
                
                RemoteWebDriver rWebDriver = new RemoteWebDriver(
                    new Uri("http://" + this.getHostname() + ":" + this.getPort() + "/hub/" ),
                    remoteCap
                );

                this.setPrettyName(rWebDriver.Capabilities.BrowserName);
                
                // Attempt to parse the version id.
                 Regex versionRegex = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)");
                    Match versionMatch = versionRegex.Match(rWebDriver.Capabilities.Version);
                    if (versionMatch.Success)
                    {
                        this.setMajor(Convert.ToInt32(versionMatch.Groups["major"].Value));
                        this.setMinor(Convert.ToInt32(versionMatch.Groups["minor"].Value));
                        this.setPatch(0);
                    }

                // Wholy hell.

                this.setIsAvailable(true);

                // new OpenQA.Selenium.Remote.RemoteWebDriver
            }
            catch (Exception e )
            {
                this.setIsAvailable(false);
            }
        }

    }
}
