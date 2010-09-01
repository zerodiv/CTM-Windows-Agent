using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
// using System.Xml;
// using System.Xml.XPath;
using HtmlAgilityPack;
using Ionic.Zip;
using OpenQA.Selenium;
using Selenium;
using System.Collections;
using System.ComponentModel;

namespace Continuum_Windows_Testing_Agent
{

    class CTM_Work_Runner
    {
        
        public CTM_Agent_Log agentLog;
        public CTM_Agent_Log testLog;

        public UInt64 testRunId;
        public UInt64 testRunBrowserId;
        public String testDownloadUrl;
        public String testBrowser;
        public String testBaseurl;

        public int testStatus;
        public String seleniumLogFile;
        public Selenium_Test_Log seleniumTestLog;

        private String proxyFile;
        private String tempTestDir;
        private String tempZipFile;
        private String tempLogFile;
        private String testRunIndexHtml;

        public CTM_Work_Runner(CTM_Agent_Log agentLog)
        {
            this.agentLog = agentLog;
        }

        private Boolean initTestingDirectory()
        {
            this.agentLog.message("initalize testing directories");

            this.tempTestDir = Environment.GetEnvironmentVariable("TEMP");
            this.tempTestDir += "\\ctmTestRun_" + this.testRunId;

            this.agentLog.message("tempTestDir: " + this.tempTestDir);

            if (Directory.Exists(this.tempTestDir) == true)
            {
                this.agentLog.message("temp dir was already there cleaning up from previous run");
                Directory.Delete(this.tempTestDir, true);
            }

            Directory.CreateDirectory(this.tempTestDir);

            if (Directory.Exists(this.tempTestDir) == false)
            {
                this.agentLog.message("failed to create temp dir: " + this.tempTestDir);
                return false;
            }

            return true;

        }

        private Boolean fetchZipFile()
        {
            // fetch the zip file from the remote server.
            this.tempZipFile = Environment.GetEnvironmentVariable("TEMP");
            this.tempZipFile += "\\ctmTestRun_" + this.testRunId + ".zip";

            this.testLog.message(" tempZipFile: " + this.tempZipFile);

            if (File.Exists(this.tempZipFile) == false)
            {
                File.Delete(this.tempZipFile);
                if (File.Exists(this.tempZipFile) == true)
                {
                    this.testLog.message("failed to remove tempZipFile: " + this.tempZipFile);
                    return false;
                }
            }

            // download the file.
            this.testLog.message("downloading zip file");
            WebClient masterClient = new WebClient();
            masterClient.DownloadFile(this.testDownloadUrl, this.tempZipFile);
            this.testLog.message("zip file downloaded");

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
                this.testLog.message("Failed to unzip message: " + e.Message);
                return false;
            }

            return true;

        }

        private Boolean initLogFiles()
        {

            this.tempLogFile = this.tempTestDir + "\\test.log";
            this.agentLog.message("init testLogFile: " + this.tempLogFile);

            this.testLog = new CTM_Agent_Log(this.tempLogFile);


            this.seleniumLogFile = this.tempTestDir + "\\selenium.html";
            this.agentLog.message("init seleniumLogFile: " + this.seleniumLogFile);
            this.seleniumTestLog = new Selenium_Test_Log(this.seleniumLogFile);

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
                this.testLog.message("failed to find test run index.html file");
                return false;
            }

            this.testLog.message("testRunIndexHtml: " + this.testRunIndexHtml);
            return true;
        }

        private Boolean createProxyFile()
        {

            this.proxyFile = Environment.GetEnvironmentVariable("TEMP");
            this.proxyFile += "\\ctmProxy.pac";

            this.testLog.message("proxyFile: " + this.proxyFile);

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

        public ArrayList getTestsFromTestSuite()
        {
            // slurp through the html file.
            /* Example:
             * <html>
             * <head>
             * <title>Checkout</title>
             * </head>
             * <body>
             * <table>
             * <tr><td><b>Checkout</b></td></tr>
             * <tr><td><a href="./1.html">Checkout</a></td></tr>
             * </table>
             * </body>
             * </html>
             */
            ArrayList tests = new ArrayList();

            try
            {

                HtmlDocument doc = new HtmlDocument();

                doc.OptionFixNestedTags = true;

                doc.Load(this.testRunIndexHtml);

                if (doc.ParseErrors != null && doc.ParseErrors.Count() > 0)
                {
                    foreach (HtmlParseError htmlError in doc.ParseErrors)
                    {
                        Console.WriteLine("error parsing file: " + htmlError.SourceText);
                    }
                    return tests;
                }

                foreach (HtmlNode testRow in doc.DocumentNode.SelectNodes("/html/body/table/tr/td/a[@href]"))
                {
                    tests.Add(testRow.Attributes["href"].Value.Replace("./", ""));
                }

            }
            catch (Exception e)
            {
                this.testLog.message("Failed to parse testSuiteHtml: " + this.testRunIndexHtml + " errorMessage: " + e.Message);
            }

            this.testLog.message("Found: " + tests.Count + " tests in your test suite.");

            return tests;

        }

        public ArrayList getTestCommands(String testFile)
        {
            ArrayList testCommands = new ArrayList();

            // this.testLog.message("testFile: " + testFile);

            HtmlDocument doc = new HtmlDocument();

            doc.OptionFixNestedTags = true;

            doc.Load(testFile);

            if (doc.ParseErrors != null && doc.ParseErrors.Count() > 0)
            {
                foreach (HtmlParseError htmlError in doc.ParseErrors)
                {
                    this.testLog.message("testFile: " + testFile);
                    this.testLog.message("error parsing file: " + htmlError.SourceText);
                }
                return testCommands;
            }

            foreach (HtmlNode testCommandRow in doc.DocumentNode.SelectNodes("/html/body/table/tbody/*"))
            {
                Selenium_Test_Trinome triNome = new Selenium_Test_Trinome();

                int tri = 0;
                foreach (HtmlNode testTrinome in testCommandRow.SelectNodes("td"))
                {
                    tri++;
                    switch (tri)
                    {
                        case 1:
                            triNome.command = testTrinome.InnerHtml;
                            break;
                        case 2:
                            triNome.target = testTrinome.InnerHtml;
                            break;
                        case 3:
                            triNome.value = testTrinome.InnerHtml;
                            break;
                    }

                }

                triNome.target = System.Web.HttpUtility.HtmlDecode(triNome.target);
                triNome.value = System.Web.HttpUtility.HtmlDecode(triNome.value);

                this.testLog.message("testCommand: " + triNome.command);
                testCommands.Add(triNome);
            }

            this.testLog.message("found: " + testCommands.Count + " testCommands in test file");
            return testCommands;

        }

        private Boolean runTestSuite()
        {
            // Parse up the suite and start sending it over to the server.
            try
            {

                ArrayList tests = this.getTestsFromTestSuite();

                // start up the requested browser.
                this.testLog.message("starting up browser: " + this.testBrowser);

                IWebDriver webDriver;

                switch (this.testBrowser)
                {
                    case "chrome":
                        webDriver = new OpenQA.Selenium.Chrome.ChromeDriver();
                        break;
                    case "firefox":
                        webDriver = new OpenQA.Selenium.Firefox.FirefoxDriver();
                        break;
                    case "googlechrome":
                        webDriver = new OpenQA.Selenium.Chrome.ChromeDriver();
                        break;
                    case "iexplore":
                        webDriver = new OpenQA.Selenium.IE.InternetExplorerDriver();
                        break;
                    default:
                        this.agentLog.message("Invalid browser specificed: " + this.testBrowser);
                        return false;
                }

                // JEO: This is a ghetto hack to help prevent issues where IE does not allow you to 
                // click on non-visibile elements. This can crop up pretty often and hopefully will
                // be fixed in later versions of Web Driver.
                webDriver.Navigate().GoToUrl("http://www.google.com");

                // ImplicityWait() change
                webDriver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 1, 0));

                OpenQA.Selenium.IJavaScriptExecutor jsExecutor = (OpenQA.Selenium.IJavaScriptExecutor)webDriver;
                jsExecutor.ExecuteScript("if(window.screen){window.moveTo(0,0);window.resizeTo(window.screen.availWidth, window.screen.availHeight);};");

                // loop across all the tests and run them.
                String testBasedir = Path.GetDirectoryName(this.testRunIndexHtml);

                foreach (String test in tests)
                {
                    String testFile = testBasedir + "\\" + test;

                    this.testLog.message("running test: " + testFile);

                    ArrayList testCommands = this.getTestCommands(testFile);

                    Selenium_Test seTest = new Selenium_Test(webDriver, this.seleniumTestLog);

                    int commandId = 0;
                    foreach (Selenium_Test_Trinome testCommand in testCommands)
                    {
                        commandId++;
                        /*
                        this.testLog.message(
                            "debug - testCommand[" + commandId + " of " + testCommands.Count + "] " +
                            "command: " + testCommand.command + " " +
                            "target: " + testCommand.target + " " +
                            "value: " + testCommand.value );
                        */
                    }

                    commandId = 0;
                    foreach (Selenium_Test_Trinome testCommand in testCommands)
                    {
                        commandId++;
                        this.testLog.message(
                            "testCommand[" + commandId + " of " + testCommands.Count + "] " +
                            "command: " + testCommand.command + " " +
                            "target: " + testCommand.target + " " +
                            "value: " + testCommand.value);
                        // this.testLog.message("testCommand[" + commandId + " of " + testCommands.Count + "]: '" + testCommand.command + "'");
                        seTest.processSelenese(testCommand);
                        this.testLog.message("testCommand finished");
                    }

                    this.testLog.message("finished test: " + testFile);
                }

                this.testLog.message("shutting down selenium");

                webDriver.Quit();

                this.testLog.message("completed running suite");
                return true;
            }
            catch (Exception e)
            {
                this.testLog.message("Failed running test: " + e.Message);
            }

            return false;
        }

        public Boolean runWork()
        {

            try
            {

                // init testing directory.
                if (this.initTestingDirectory() == false)
                {
                    this.agentLog.message("Failed to init testing directories");
                    this.cleanup();
                    return false;
                }

                // setup the log file
                if (this.initLogFiles() == false)
                {
                    this.agentLog.message("Failed to init log files");
                    this.cleanup();
                    return false;
                }

                // download zip file.
                if (this.fetchZipFile() == false)
                {
                    this.agentLog.message("Failed to download zip file");
                    this.cleanup();
                    return false;
                }

                // find the index.html file
                if (this.findTestIndexFile() == false)
                {
                    this.agentLog.message("Failed to find test index file");
                    this.cleanup();
                    return false;
                }

                // Create a self pointing proxy file for IE (if needed).
                if (this.createProxyFile() == false)
                {
                    this.agentLog.message("Failed to create proxy file");
                    this.cleanup();
                    return false;
                }

                // jeo - prototyping the exclusion of the commandline batch runner.
                // The big thing we want is control over timing and the logging.

                // run the tests.
                if (this.runTestSuite() == true)
                {
                    this.testStatus = 1;
                }
                else
                {
                    this.testStatus = 0;
                }

                this.seleniumTestLog.closeLogFile();

                return true;

            }
            catch (Exception e)
            {
                // this.testLog.message("failed to run test suite message: " + e.Message);
                this.agentLog.message("Failed to run test suite message: " + e.Message);
                this.cleanup();
                return false;
            }
        }

        public void cleanup()
        {
            /*
            if (Directory.Exists(this.tempTestDir) == true)
            {
                // Directory.Delete(this.tempTestDir, true);
            }
            if (File.Exists(this.tempZipFile) == true)
            {
                File.Delete(this.tempZipFile);
            }

            // Flush all the variables pertaining to this run.
            this.seleniumLogFile = null;
            this.seleniumTestLog = null;
            this.tempLogFile = null;
            this.tempTestDir = null;
            this.tempZipFile = null;
            this.testBaseurl = null;
            this.testBrowser = null;
            this.testDownloadUrl = null;
            this.testLog = null;
            this.testRunBrowserId = 0;
            this.testRunId = 0;
            this.testRunIndexHtml = null;
            this.testStatus = 0;
            */
        }

    }
}
