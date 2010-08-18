using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Ionic.Zip;
// using Selenium;

namespace Continuum_Windows_Testing_Agent
{
    class WorkRunner
    {
        public AgentLog log;
        public UInt64 testRunId;
        public UInt64 testRunBrowserId;
        public String testDownloadUrl;
        public String testBrowser;
        public String testBaseurl;

        public int testStatus;
        public String testLog;
        public long timeElapsed;
        public String seleniumStdout;
        public String seleniumStderr;
        public String seleniumLog;

        private String proxyFile;
        private String tempTestDir;
        private String tempZipFile;
        private String tempLogFile;
        private String testRunIndexHtml;
        private String seleniumJarFile;
        private String seleniumCommandLine;

        private System.Diagnostics.Process seleniumServer;

        public WorkRunner(AgentLog log)
        {
            this.log = log;
        }

        private Boolean initTestingDirectory()
        {
            this.tempTestDir = Environment.GetEnvironmentVariable("TEMP");
            this.tempTestDir += "\\ctmTestRun_" + this.testRunId;

            this.log.message("tempTestDir: " + this.tempTestDir);

            if (Directory.Exists(this.tempTestDir) == true)
            {
                this.log.message("temp dir was already there cleaning up from previous run");
                Directory.Delete(this.tempTestDir, true);
            }

            Directory.CreateDirectory(this.tempTestDir);

            if (Directory.Exists(this.tempTestDir) == false)
            {
                this.log.message("failed to create temp dir: " + this.tempTestDir);
                return false;
            }

            return true;

        }

        private Boolean fetchZipFile()
        {
            // fetch the zip file from the remote server.
            this.tempZipFile = Environment.GetEnvironmentVariable("TEMP");
            this.tempZipFile += "\\ctmTestRun_" + this.testRunId + ".zip";

            this.log.message(" tempZipFile: " + this.tempZipFile);

            if (File.Exists(this.tempZipFile) == false)
            {
                File.Delete(this.tempZipFile);
                if (File.Exists(this.tempZipFile) == true)
                {
                    this.log.message("failed to remove tempZipFile: " + this.tempZipFile);
                    return false;
                }
            }

            // download the file.
            this.log.message("downloading zip file");
            WebClient masterClient = new WebClient();
            masterClient.DownloadFile(this.testDownloadUrl, this.tempZipFile);
            this.log.message("zip file downloaded");

            // unzip the file into the temp directory.
            try
            {
                using (ZipFile zip = ZipFile.Read(this.tempZipFile))
                {
                    foreach (ZipEntry e in zip)
                    {
                        e.Extract(this.tempTestDir);
                    }
                }
            }
            catch (Exception e)
            {
                this.log.message("Failed to unzip");
                return false;
            }

            return true;

        }

        private Boolean initLogFile()
        {
            this.tempLogFile = tempTestDir + "\\test.log";

            this.log.message("tempLogFile: " + this.tempLogFile);
            return true;
        }

        private Boolean findTestIndexFile()
        {

            // find the index.html associated with this test run
            String[] subDirs = Directory.GetDirectories(this.tempTestDir);

            foreach (String sDir in subDirs)
            {
                this.testRunIndexHtml = sDir + "\\index.html";
                if (File.Exists(this.testRunIndexHtml))
                {
                    // we are done.
                    break;
                }
            }

            if (File.Exists(this.testRunIndexHtml) == false)
            {
                this.log.message("failed to find test run index.html file");
                return false;
            }

            this.log.message("testRunIndexHtml: " + this.testRunIndexHtml);
            return true;
        }

        private Boolean findSeleniumServerJarFile()
        {
            this.seleniumJarFile = Directory.GetCurrentDirectory() + "\\selenium-server.jar";
            return true;
        }

        private Boolean createProxyFile()
        {

            this.proxyFile = Environment.GetEnvironmentVariable("TEMP");
            this.proxyFile += "\\ctmProxy.pac";

            this.log.message("proxyFile: " + this.proxyFile);

            if (File.Exists(this.proxyFile))
            {
                return true;
            }

            using (StreamWriter sw = new StreamWriter(this.proxyFile, false, Encoding.Default))
            {
                sw.WriteLine("function FindProxyForURL(url, host) {");
                sw.WriteLine("        return 'PROXY localhost:4444; DIRECT';");
                sw.WriteLine("}");
                sw.Close();

                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings");
                key.SetValue("AutoConfigUrl", "file://" + this.proxyFile.Replace("\\", "/"));

                return true;
            }


        }

        private Boolean createTestRunCommandLine()
        {
            // run the test against the harness with the logging on.
            this.seleniumCommandLine = "";

            this.seleniumCommandLine += "-Dhttp.proxyHost=localhost ";
            this.seleniumCommandLine += "-Dhttp.proxyPort=4444 ";

            // point to the selenium-server.jar file.
            this.seleniumCommandLine += "-jar \"" + this.seleniumJarFile + "\" ";

            // JEO - Duane claims that this is not needed.
            // this.seleniumCommandLine += "-multiwindow ";

            // suprise suprise IE is special and needs the singleWindow param 
            if (this.testBrowser == "iexplore")
            {
                this.seleniumCommandLine += "-singleWindow ";
                // this.seleniumCommandLine += "-avoidProxy ";
            }
            // We are running a htmlSuite
            this.seleniumCommandLine += "-htmlSuite ";

            // add the test browser information
            /*
            if (testBrowser == "iexplore")
            {
               testBrowser = "iexploreproxy";
            }
            */

            String IE_Path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles) +
                "\\Internet Explorer\\iexplore.exe";
            if (testBrowser == "iexplore" && File.Exists(IE_Path))
            {
                this.seleniumCommandLine += "\"*custom " + IE_Path + "\" ";
            }
            else
            {
                this.seleniumCommandLine += "\"*" + this.testBrowser + "\" ";

            }

            this.seleniumCommandLine += "\"" + testBaseurl + "\" ";
            this.seleniumCommandLine += "\"" + testRunIndexHtml + "\" ";
            this.seleniumCommandLine += "\"" + tempLogFile + "\" ";

            this.log.message("seleniumCommandline:\n" + this.seleniumCommandLine);

            return true;
        }

        private Boolean execTestSuite()
        {


            System.Diagnostics.Process seleniumServer = new System.Diagnostics.Process();
            seleniumServer.EnableRaisingEvents = false;
            seleniumServer.StartInfo.UseShellExecute = false;
            seleniumServer.StartInfo.FileName = "java";
            seleniumServer.StartInfo.Arguments = this.seleniumCommandLine;
            seleniumServer.StartInfo.RedirectStandardError = true;
            seleniumServer.StartInfo.RedirectStandardOutput = true;

            seleniumServer.Start();

            // seleniumServer.BeginOutputReadLine();
            this.seleniumStdout = seleniumServer.StandardOutput.ReadToEnd();
            this.seleniumStderr = seleniumServer.StandardError.ReadToEnd();

            seleniumServer.WaitForExit();

            // push the log back up to the server.
            this.timeElapsed =
                seleniumServer.ExitTime.ToFileTimeUtc() -
                seleniumServer.StartTime.ToFileTimeUtc();

            this.testStatus = 0;
            if (seleniumServer.ExitCode == 0)
            {
                this.testStatus = 1;
                this.log.message("ran test");
            }
            else
            {
                this.testStatus = 0;
                this.log.message("failed to run test");
            }

            seleniumServer.Close();

            if (this.testStatus == 0)
            {
                this.testLog += "<pre>\n";
                this.testLog += "Stdout:\n";
                this.testLog += this.seleniumStdout;
                this.testLog += "Stderr:\n";
                this.testLog += this.seleniumStderr;
                this.testLog += "</pre>\n";
            }

            if (File.Exists(this.tempLogFile))
            {
                this.seleniumLog = File.ReadAllText(this.tempLogFile);
            }

            return true;
        }

        private Boolean startSeleniumServer()
        {
            this.seleniumServer = new System.Diagnostics.Process();
            this.seleniumServer.EnableRaisingEvents = false;
            this.seleniumServer.StartInfo.UseShellExecute = false;
            this.seleniumServer.StartInfo.FileName = "java";
            this.seleniumServer.StartInfo.Arguments = "-jar \"" + this.seleniumJarFile + "\" ";
            this.seleniumServer.StartInfo.RedirectStandardError = true;
            this.seleniumServer.StartInfo.RedirectStandardOutput = true;
            // this.seleniumServer.StartInfo.CreateNoWindow = true;            
            return this.seleniumServer.Start();
        }

        private Boolean sendWorkToServer()
        {
            /*
            DefaultSelenium se = new DefaultSelenium("localhost", 4444, "*" + this.testBrowser, this.testBaseurl);
            se.Start();
            // se.RunScript(File.ReadAllText(@"c:\1.html"));
            System.Threading.Thread.Sleep(2000);
            se.Stop();
            */
            return true;
        }

        public Boolean runWork()
        {
            try
            {

                // init testing directory.
                if (this.initTestingDirectory() == false)
                {
                    this.cleanup();
                    return false;
                }

                // download zip file.
                if (this.fetchZipFile() == false)
                {
                    this.cleanup();
                    return false;
                }

                // setup the log file
                if (this.initLogFile() == false)
                {
                    this.cleanup();
                    return false;
                }

                // find the index.html file
                if (this.findTestIndexFile() == false)
                {
                    this.cleanup();
                    return false;
                }

                // find the selenium jar file
                if (this.findSeleniumServerJarFile() == false)
                {
                    this.cleanup();
                    return false;
                }

                if (this.createProxyFile() == false)
                {
                    this.cleanup();
                    return false;
                }

                /*
                // jeo - temp
                if (this.startSeleniumServer() == true)
                {
                    this.sendWorkToServer();
                    System.Threading.Thread.Sleep(2000);
                }
                this.cleanup();
                return false;
                */

                // create the commandline 
                if (this.createTestRunCommandLine() == false)
                {
                    this.cleanup();
                    return false;
                }

                // execute the selenium test suite
                this.execTestSuite();

                return true;

            }
            catch (Exception e)
            {
                this.cleanup();
                return false;
            }
        }

        public void cleanup()
        {
            if (Directory.Exists(this.tempTestDir) == true)
            {
                Directory.Delete(this.tempTestDir, true);
            }
            if (File.Exists(this.tempZipFile) == true)
            {
                File.Delete(this.tempZipFile);
            }
        }

    }
}
