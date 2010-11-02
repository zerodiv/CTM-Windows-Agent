using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Ionic.Zip;
using System.Collections;
using HtmlAgilityPack;
using OpenQA.Selenium;
using System.Threading;

using Selenium.Internal.SeleniumEmulation;
using Continuum_Windows_Testing_Agent.Selenese;
using System.Text.RegularExpressions;

namespace Continuum_Windows_Testing_Agent
{
    public partial class CTM_Test : Form
    {

        #region Private Variables
        delegate void SetTestNameCallback(string text);

        delegate void clearGridCallback();
        delegate void addCommandToGridCallback(Selenium_Test_Trinome cmd);
        delegate void updateCommandStatusCallback(int id, Boolean state, String message);
        
        private UInt64 testRunId;
        private UInt64 testRunBrowserId;
        private String testDownloadUrl;
        private String testBrowser;
        private int testStatus;
        private Boolean useVerboseTestLogs;
        private Boolean haltOnError;
        private String tempTestDir;
        private String tempZipFile;
        private String testRunIndexHtml;

        public Boolean testHadError;
        private IWebDriver webDriver;
        private Selenium_Test_Suite_Variables testVariables;
        private Selenium_Test_Log log;
        private Hashtable seleneseCommands;

        private Hashtable seleneseMethods;
        private ElementFinder elementFinder;
        private SeleniumOptionSelector select;
        private KeyState keyState;

        #endregion Private Variables

        #region Constructor
        public CTM_Test()
        {

            InitializeComponent();

            // init local variables
            this.testRunId = 0;
            this.testRunBrowserId = 0;
            this.testDownloadUrl = "";
            this.testBrowser = "";
            this.testStatus = 0;
            this.useVerboseTestLogs = false;
            this.haltOnError = false;

            this.testHadError = false;
            
            this.testVariables = new Selenium_Test_Suite_Variables();
            
            this.seleneseCommands = new Hashtable();

            this.seleneseMethods = new Hashtable();
            this.elementFinder = new ElementFinder();
            this.select = new SeleniumOptionSelector(this.elementFinder);

            this.keyState = new KeyState();

        }
        #endregion Constructor

        #region Getter / Setters
        public UInt64 getTestRunId() {
            return this.testRunId;
        }

        public void setTestRunId(UInt64 testRunId) {
            this.testRunId = testRunId;
        }

        public UInt64 getTestRunBrowserId()
        {
            return this.testRunBrowserId;
        }

        public void setTestRunBrowserId(UInt64 testRunBrowserId)
        {
            this.testRunBrowserId = testRunBrowserId;
        }

        public string getTestDownloadUrl()
        {
            return this.testDownloadUrl;
        }

        public void setTestDownloadUrl(String url)
        {
            this.testDownloadUrl = url;
        }

        public string getTestBrowser()
        {
            return this.testBrowser;
        }

        public void setTestBrowser(string browser)
        {
            this.testBrowser = browser;
        }

        public int getTestStatus()
        {
            return this.testStatus;
        }

        public void setUseVerboseTestLogs(Boolean use)
        {
            this.useVerboseTestLogs = use;
        }

        public void setHaltOnError(Boolean use)
        {
            this.haltOnError = use;
        }

        public Selenium_Test_Log getSeleniumTestLog()
        {
            return this.log;
        }
#endregion Getter / Setters

        #region Async Delegates
        private void setTestNameBox(String text)
        {
            if (this.testNameBox.InvokeRequired == true)
            {
                SetTestNameCallback d = new SetTestNameCallback(setTestNameBox);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.testNameBox.Text = text;
            }
        }

        private void clearGrid()
        {
            if (this.activeTestGrid.InvokeRequired == true)
            {
                clearGridCallback d = new clearGridCallback(clearGrid);
                this.Invoke(d, new object[] {});
            }
            else
            {
                this.activeTestGrid.Rows.Clear();
            }
        }

        private void addCommandToGrid(Selenium_Test_Trinome cmd)
        {
            if (this.activeTestGrid.InvokeRequired == true)
            {
                addCommandToGridCallback d = new addCommandToGridCallback(addCommandToGrid);
                this.Invoke(d, new object[] { cmd });
            }
            else
            {
                this.activeTestGrid.Rows.Add(new string[] { cmd.getCommand(), cmd.getTarget(), cmd.getValue(), "" });
            }
        }

        private void updateCommandStatus(int id, Boolean state, String message)
        {
            if (this.activeTestGrid.InvokeRequired == true)
            {
                updateCommandStatusCallback d = new updateCommandStatusCallback(updateCommandStatus);
                this.Invoke(d, new object[] { id, state, message });
            }
            else
            {
                int rowId = id - 1;
                this.activeTestGrid.Rows[rowId].Cells[3].Value = message;
                if (state == true)
                    {
                        this.activeTestGrid.Rows[rowId].DefaultCellStyle.BackColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        this.activeTestGrid.Rows[rowId].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                    }
                    
            }
        }

        #endregion Async Delegates

        #region Init Testing
        private Boolean initTestingDirectory()
        {
            this.log.message("initalize testing directories");

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
                this.log.message("Failed to unzip message: " + e.Message);
                return false;
            }

            return true;

        }

        private Boolean initLogFiles()
        {

            String seleniumLogFile = Environment.GetEnvironmentVariable("TEMP") + "\\selenium_" + this.testRunId + ".html";

            this.log = new Selenium_Test_Log(this.useVerboseTestLogs, seleniumLogFile);
            this.log.message("init seleniumLogFile: " + seleniumLogFile);
            

            return true;
        }
        #endregion Init Testing

        #region Test File Manipulation
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

                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

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
                this.log.message("Failed to parse testSuiteHtml: " + this.testRunIndexHtml + " errorMessage: " + e.Message);
            }

            this.log.message("Found: " + tests.Count + " tests in your test suite.");

            return tests;

        }

        public String getTestTitle(String testFile)
        {
            String testTitle = "Unknown Test";

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.OptionFixNestedTags = true;

            doc.Load(testFile);

            if (doc.ParseErrors != null && doc.ParseErrors.Count() > 0)
            {
                foreach (HtmlParseError htmlError in doc.ParseErrors)
                {
                    this.log.message("testFile: " + testFile);
                    this.log.message("error parsing file: " + htmlError.SourceText);
                }
                return testTitle;
            }
            foreach (HtmlNode testTitleRow in doc.DocumentNode.SelectNodes("/html/body/table/thead/*"))
            {
                foreach (HtmlNode testTitleCell in testTitleRow.SelectNodes("td"))
                {
                    testTitle = testTitleCell.InnerHtml;
                }
            }
            return testTitle;
        }

        public ArrayList getTestCommands(String testFile)
        {
            ArrayList testCommands = new ArrayList();

            // this.log.message("testFile: " + testFile);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.OptionFixNestedTags = true;

            doc.Load(testFile);

            if (doc.ParseErrors != null && doc.ParseErrors.Count() > 0)
            {
                foreach (HtmlParseError htmlError in doc.ParseErrors)
                {
                    this.log.message("testFile: " + testFile);
                    this.log.message("error parsing file: " + htmlError.SourceText);
                }
                return testCommands;
            }

            int commandId = 0;

            foreach (HtmlNode testCommandRow in doc.DocumentNode.SelectNodes("/html/body/table/tbody"))
            {
                foreach (HtmlNode testNode in testCommandRow.ChildNodes)
                {
                    if (testNode.Name == "#comment")
                    {
                        Selenium_Test_Trinome triNome = new Selenium_Test_Trinome();
                        triNome.setCommand(":comment:");
                        String comment = System.Web.HttpUtility.HtmlDecode(testNode.InnerText);
                        comment = comment.Replace("<!-- ", "");
                        comment = comment.Replace(" -->", "");
                        triNome.setTarget(comment);
                        testCommands.Add(triNome);
                    }
                    if (testNode.Name == "tr")
                    {
                        Selenium_Test_Trinome triNome = new Selenium_Test_Trinome();
                        int tri = 0;
                        foreach (HtmlNode testTrinome in testNode.ChildNodes)
                        {
                            if (testTrinome.Name == "td")
                            {
                                tri++;

                                if (tri == 1)
                                {
                                    triNome.setCommand(testTrinome.InnerHtml);
                                }
                                if (tri == 2)
                                {
                                    triNome.setTarget(System.Web.HttpUtility.HtmlDecode(testTrinome.InnerHtml));
                                }
                                if (tri == 3)
                                {
                                    triNome.setValue(System.Web.HttpUtility.HtmlDecode(testTrinome.InnerHtml));
                                }
                            }
                        }

                        commandId++;
                        triNome.setId(commandId);

                        testCommands.Add(triNome);

                        this.log.message(
                            "testCommand: " + triNome.getCommand() + " " +
                            "testTarget: " + triNome.getTarget() + " " +
                            "testValue: " + triNome.getValue()
                        );
                    }
                }

            }

            this.log.message("found: " + testCommands.Count + " testCommands in test file");
            return testCommands;

        }
        #endregion Test File Manipulation

        private Boolean runTestSuite()
        {
            // Parse up the suite and start sending it over to the server.
            try
            {
                ArrayList tests = this.getTestsFromTestSuite();

                // start up the requested browser.
                this.log.message("starting up browser: " + this.testBrowser);

                switch (this.testBrowser)
                {
                    case "chrome":
                        this.webDriver = new OpenQA.Selenium.Chrome.ChromeDriver();
                        break;
                    case "firefox":
                        this.webDriver = new OpenQA.Selenium.Firefox.FirefoxDriver();
                        break;
                    case "googlechrome":
                        this.webDriver = new OpenQA.Selenium.Chrome.ChromeDriver();
                        break;
                    case "iexplore":
                        this.webDriver = new OpenQA.Selenium.IE.InternetExplorerDriver();
                        break;
                    default:
                        this.log.message("Invalid browser specificed: " + this.testBrowser);
                        return false;
                }

                // TODO: We should pull this up to the top level init, but the log() requirement 
                // prevents this right now.
                this.initSeleneseCommands();
        
                // JEO: This is a ghetto hack to help prevent issues where IE does not allow you to 
                // click on non-visibile elements. This can crop up pretty often and hopefully will
                // be fixed in later versions of Web Driver.
                webDriver.Navigate().GoToUrl("http://www.google.com");

                // ImplicityWait() change
                // webDriver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 1, 0));

                OpenQA.Selenium.IJavaScriptExecutor jsExecutor = (OpenQA.Selenium.IJavaScriptExecutor)webDriver;
                jsExecutor.ExecuteScript("if(window.screen){window.moveTo(0,0);window.resizeTo(window.screen.availWidth, window.screen.availHeight);};");

                // loop across all the tests and run them.
                String testBasedir = Path.GetDirectoryName(this.testRunIndexHtml);

                foreach (String test in tests)
                {
                    String testFile = testBasedir + "\\" + test;

                    this.log.message("running test: " + testFile);

                    String testTitle = this.getTestTitle(testFile);

                    // Change the title of the test to our test we are currently working on.
                    this.setTestNameBox(testTitle);

                    // output the test title.
                    this.log.startTestMessage(testTitle);

                    // Reset the test table
                    this.clearGrid();

                    ArrayList testCommands = this.getTestCommands(testFile);

                    // Load the commands into the table
                    foreach (Selenium_Test_Trinome testCommand in testCommands)
                    {
                        this.addCommandToGrid(testCommand);
                    }

                    // Run the commands.
                    foreach (Selenium_Test_Trinome testCommand in testCommands)
                    {
                        this.log.message(
                            "testCommand[" + testCommand.getId() + " of " + testCommands.Count + "] " +
                            "command: " + testCommand.getCommand() + " " +
                            "target: " + testCommand.getTarget() + " " +
                            "value: " + testCommand.getValue());
                        
                        // this.log.message("testCommand[" + commandId + " of " + testCommands.Count + "]: '" + testCommand.command + "'");
                        Boolean selReturn = this.processSelenese(testCommand);

                        if (this.haltOnError == true && selReturn == false)
                        {
                            Thread.Sleep(30000);
                        }

                        this.log.message("testCommand finished");
                       
                    }

                    this.log.message("finished test: " + testFile);
                }

                this.log.message("shutting down selenium");

                webDriver.Quit();

                if (this.testHadError == false)
                {
                    this.log.message("completed running suite - successful");
                    return true;
                }
                this.log.message("completed running suite - failure");
                return false;
            }
            catch (Exception e)
            {
                this.log.message("Failed running test: " + e.Message);
            }

            return false;
        }

        public Boolean runWork()
        {

            try
            {

                // setup the log file
                if (this.initLogFiles() == false)
                {
                    this.cleanup();
                    return false;
                }

                // init testing directory.
                if (this.initTestingDirectory() == false)
                {
                    this.log.message("Failed to init testing directories");
                    this.cleanup();
                    return false;
                }


                // download zip file.
                if (this.fetchZipFile() == false)
                {
                    this.log.message("Failed to download zip file");
                    this.cleanup();
                    return false;
                }

                // find the index.html file
                if (this.findTestIndexFile() == false)
                {
                    this.log.message("Failed to find test index file");
                    this.cleanup();
                    return false;
                }

                // run the tests.
                if (this.runTestSuite() == true)
                {
                    this.testStatus = 1;
                }
                else
                {
                    this.testStatus = 0;
                }

                this.log.closeLogFile();

                return true;

            }
            catch (Exception e)
            {
                // this.log.message("failed to run test suite message: " + e.Message);
                this.log.message("Failed to run test suite message: " + e.Message);
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
            this.log = null;
           this.tempTestDir = null;
            this.tempZipFile = null;
            this.testBrowser = null;
            this.testDownloadUrl = null;
            this.testLog = null;
            this.testRunBrowserId = 0;
            this.testRunId = 0;
            this.testRunIndexHtml = null;
            this.testStatus = 0;
            */
        }


        private void initSeleneseCommands()
        {
            this.seleneseCommands.Add("addSelection", new SeleneseAddSelection(this.log, this.webDriver));
            this.seleneseCommands.Add("assertElementPresent", new SeleneseAssertElementPresent(this.log, this.webDriver));
            this.seleneseCommands.Add("assertTextPresent", new SeleneseAssertTextPresent(this.log, this.webDriver));
            // this.seleneseCommands.Add("click", new SeleneseClick(this.log, this.webDriver));
            // this.seleneseCommands.Add("clickAndWait", new SeleneseClickAndWait(this.log, this.webDriver));
            this.seleneseCommands.Add("open", new SeleneseOpen(this.log, this.webDriver));
            this.seleneseCommands.Add("pause", new SelenesePause(this.log, this.webDriver));
            this.seleneseCommands.Add("removeSelection", new SeleneseRemoveSelection(this.log, this.webDriver));
            this.seleneseCommands.Add("select", new SeleneseSelect(this.log, this.webDriver));
            this.seleneseCommands.Add("store", new SeleneseStore(this.log, this.webDriver, this.testVariables));
            // this.seleneseCommands.Add("type", new SeleneseType(this.log, this.webDriver));
            this.seleneseCommands.Add("waitForPageToLoad", new SeleneseWaitForPageToLoad(this.log, this.webDriver));
            this.seleneseCommands.Add("verifyTextPresent", new SeleneseVerifyTextPresent(this.log, this.webDriver));

            // Vendor provided code we haven't modified.
            // Note the we use the names used by the CommandProcessor
            //seleneseMethods.Add("addLocationStrategy", new AddLocationStrategy(elementFinder));
            //seleneseMethods.Add("addSelection", new AddSelection(elementFinder, select));
            //seleneseMethods.Add("altKeyDown", new AltKeyDown(keyState));
            //seleneseMethods.Add("altKeyUp", new AltKeyUp(keyState));
            //seleneseMethods.Add("assignId", new AssignId(elementFinder));
            //seleneseMethods.Add("attachFile", new AttachFile(elementFinder));
            //seleneseMethods.Add("captureScreenshotToString", new CaptureScreenshotToString());
            this.seleneseMethods.Add("click", new Click(this.elementFinder));
            this.seleneseMethods.Add("clickAndWait", new ClickAndWait(this.elementFinder));
            //seleneseMethods.Add("check", new Check(elementFinder));
            //seleneseMethods.Add("close", new Close());
            //seleneseMethods.Add("createCookie", new CreateCookie());
            //seleneseMethods.Add("controlKeyDown", new ControlKeyDown(keyState));
            //seleneseMethods.Add("controlKeyUp", new ControlKeyUp(keyState));
            //seleneseMethods.Add("deleteAllVisibleCookies", new DeleteAllVisibleCookies());
            //seleneseMethods.Add("deleteCookie", new DeleteCookie());
            //seleneseMethods.Add("doubleClick", new DoubleClick(elementFinder));
            //seleneseMethods.Add("dragdrop", new DragAndDrop(elementFinder));
            //seleneseMethods.Add("dragAndDrop", new DragAndDrop(elementFinder));
            //seleneseMethods.Add("dragAndDropToObject", new DragAndDropToObject(elementFinder));
            //seleneseMethods.Add("fireEvent", new FireEvent(elementFinder));
            //seleneseMethods.Add("focus", new FireNamedEvent(elementFinder, "focus"));
            //seleneseMethods.Add("getAllButtons", new GetAllButtons());
            //seleneseMethods.Add("getAllFields", new GetAllFields());
            //seleneseMethods.Add("getAllLinks", new GetAllLinks());
            //seleneseMethods.Add("getAllWindowTitles", new GetAllWindowTitles());
            //seleneseMethods.Add("getAttribute", new GetAttribute(elementFinder));
            //seleneseMethods.Add("getAttributeFromAllWindows", new GetAttributeFromAllWindows());
            //seleneseMethods.Add("getBodyText", new GetBodyText());
            //seleneseMethods.Add("getCookie", new GetCookie());
            //seleneseMethods.Add("getCookieByName", new GetCookieByName());
            //seleneseMethods.Add("getElementHeight", new GetElementHeight(elementFinder));
            //seleneseMethods.Add("getElementIndex", new GetElementIndex(elementFinder));
            //seleneseMethods.Add("getElementPositionLeft", new GetElementPositionLeft(elementFinder));
            //seleneseMethods.Add("getElementPositionTop", new GetElementPositionTop(elementFinder));
            //seleneseMethods.Add("getElementWidth", new GetElementWidth(elementFinder));
            //seleneseMethods.Add("getEval", new GetEval(baseUrl));
            //seleneseMethods.Add("getHtmlSource", new GetHtmlSource());
            //seleneseMethods.Add("getLocation", new GetLocation());
            //seleneseMethods.Add("getSelectedId", new FindFirstSelectedOptionProperty(select, SeleniumOptionSelector.Property.ID));
            //seleneseMethods.Add("getSelectedIds", new FindSelectedOptionProperties(select, SeleniumOptionSelector.Property.ID));
            //seleneseMethods.Add("getSelectedIndex", new FindFirstSelectedOptionProperty(select, SeleniumOptionSelector.Property.Index));
            //seleneseMethods.Add("getSelectedIndexes", new FindSelectedOptionProperties(select, SeleniumOptionSelector.Property.Index));
            //seleneseMethods.Add("getSelectedLabel", new FindFirstSelectedOptionProperty(select, SeleniumOptionSelector.Property.Text));
            //seleneseMethods.Add("getSelectedLabels", new FindSelectedOptionProperties(select, SeleniumOptionSelector.Property.Text));
            //seleneseMethods.Add("getSelectedValue", new FindFirstSelectedOptionProperty(select, SeleniumOptionSelector.Property.Value));
            //seleneseMethods.Add("getSelectedValues", new FindSelectedOptionProperties(select, SeleniumOptionSelector.Property.Value));
            //seleneseMethods.Add("getSelectOptions", new GetSelectOptions(select));
            //seleneseMethods.Add("getSpeed", new NoOp("0"));
            //seleneseMethods.Add("getTable", new GetTable(elementFinder));
            //seleneseMethods.Add("getText", new GetText(elementFinder));
            //seleneseMethods.Add("getTitle", new GetTitle());
            //seleneseMethods.Add("getValue", new GetValue(elementFinder));
            //seleneseMethods.Add("getXpathCount", new GetXpathCount());
            //seleneseMethods.Add("goBack", new GoBack());
            //seleneseMethods.Add("highlight", new Highlight(elementFinder));
            //seleneseMethods.Add("isChecked", new IsChecked(elementFinder));
            //seleneseMethods.Add("isCookiePresent", new IsCookiePresent());
            //seleneseMethods.Add("isEditable", new IsEditable(elementFinder));
            //seleneseMethods.Add("isElementPresent", new IsElementPresent(elementFinder));
            //seleneseMethods.Add("isOrdered", new IsOrdered(elementFinder));
            //seleneseMethods.Add("isSomethingSelected", new IsSomethingSelected(select));
            //seleneseMethods.Add("isTextPresent", new IsTextPresent());
            //seleneseMethods.Add("isVisible", new IsVisible(elementFinder));
            //seleneseMethods.Add("keyDown", new KeyEvent(elementFinder, keyState, "doKeyDown"));
            //seleneseMethods.Add("keyPress", new TypeKeys(elementFinder));
            //seleneseMethods.Add("keyUp", new KeyEvent(elementFinder, keyState, "doKeyUp"));
            //seleneseMethods.Add("metaKeyDown", new MetaKeyDown(keyState));
            //seleneseMethods.Add("metaKeyUp", new MetaKeyUp(keyState));
            //seleneseMethods.Add("mouseOver", new MouseEvent(elementFinder, "mouseover"));
            //seleneseMethods.Add("mouseOut", new MouseEvent(elementFinder, "mouseout"));
            //seleneseMethods.Add("mouseDown", new MouseEvent(elementFinder, "mousedown"));
            //seleneseMethods.Add("mouseDownAt", new MouseEventAt(elementFinder, "mousedown"));
            //seleneseMethods.Add("mouseMove", new MouseEvent(elementFinder, "mousemove"));
            //seleneseMethods.Add("mouseMoveAt", new MouseEventAt(elementFinder, "mousemove"));
            //seleneseMethods.Add("mouseUp", new MouseEvent(elementFinder, "mouseup"));
            //seleneseMethods.Add("mouseUpAt", new MouseEventAt(elementFinder, "mouseup"));
            //seleneseMethods.Add("open", new Open(baseUrl));
            //seleneseMethods.Add("openWindow", new OpenWindow(new GetEval(baseUrl)));
            //seleneseMethods.Add("refresh", new Refresh());
            //seleneseMethods.Add("removeAllSelections", new RemoveAllSelections(elementFinder));
            //seleneseMethods.Add("removeSelection", new RemoveSelection(elementFinder, select));
            //seleneseMethods.Add("runScript", new RunScript());
            //seleneseMethods.Add("select", new SelectOption(select));
            //seleneseMethods.Add("selectFrame", new SelectFrame(windows));
            //seleneseMethods.Add("selectWindow", new SelectWindow(windows));
            //seleneseMethods.Add("setBrowserLogLevel", new NoOp(null));
            //seleneseMethods.Add("setContext", new NoOp(null));
            //seleneseMethods.Add("setSpeed", new NoOp(null));
            //////seleneseMethods.Add("setTimeout", new SetTimeout(timer));
            //seleneseMethods.Add("shiftKeyDown", new ShiftKeyDown(keyState));
            //seleneseMethods.Add("shiftKeyUp", new ShiftKeyUp(keyState));
            //seleneseMethods.Add("submit", new Submit(elementFinder));
            this.seleneseMethods.Add("type", new Selenium.Internal.SeleniumEmulation.Type(elementFinder, this.keyState));
            //seleneseMethods.Add("typeKeys", new TypeKeys(elementFinder));
            //seleneseMethods.Add("uncheck", new Uncheck(elementFinder));
            //seleneseMethods.Add("useXpathLibrary", new NoOp(null));
            //seleneseMethods.Add("waitForCondition", new WaitForCondition());
            //seleneseMethods.Add("waitForFrameToLoad", new NoOp(null));
            //seleneseMethods.Add("waitForPageToLoad", new WaitForPageToLoad());
            //seleneseMethods.Add("waitForPopUp", new WaitForPopup(windows));
            //seleneseMethods.Add("windowFocus", new WindowFocus());
            //seleneseMethods.Add("windowMaximize", new WindowMaximize());

        }

        protected String runJavascriptValue(String javascriptValue)
        {
            String javascript = "";
            if (javascriptValue.StartsWith("javascript{"))
            {
                javascript = javascriptValue;
                javascript = Regex.Replace(javascript, @"^javascript{", "");
                javascript = Regex.Replace(javascript, @"}$", "");
            }
            else
            {
                return javascriptValue;
            }

            // quick fix for people forgetting to do a return.
            if (javascript.Contains("return") == false)
            {
                javascript = "return " + javascript;
            }

            if (!javascript.EndsWith(";"))
            {
                javascript = javascript + ";";
            }
            String value = "";
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)this.webDriver;

            if (jsExecutor.IsJavaScriptEnabled == true)
            {

                this.log.message("executing javascript: " + javascript);
                Object ret = jsExecutor.ExecuteScript(javascript);
                value = ret.ToString();
                this.log.message("end js_exec value: " + value);
            }

            return value;

        }

        public Selenium_Test_Trinome interpolateSeleneseVariables(Selenium_Test_Trinome testCommand)
        {
            // Special exception for cleaning up / interpolating the testCommand into the new values.
            if (testCommand.getCommand() != "store" && testCommand.getCommand() != ":comment:")
            {
                testCommand.setTarget(this.testVariables.replaceVariables(testCommand.getTarget()));
                testCommand.setValue(this.testVariables.replaceVariables(testCommand.getValue()));
            }

            // If the value contains javascript run it and replace the value.
            testCommand.setValue(this.runJavascriptValue(testCommand.getValue()));

            return testCommand;
        }

        public Boolean processSelenese(Selenium_Test_Trinome testCommand)
        {

            if (testCommand.getCommand() == ":comment:")
            {
                this.log.insertTestComment(testCommand.getTarget());
                return true;
            }

            if (this.testHadError == true)
            {
                this.log.logFailure(testCommand, "not executed - failure already occurred.");
                return false;
            }

            if (this.seleneseMethods.ContainsKey(testCommand.getCommand()))
            {
                SeleneseCommand cmd = (SeleneseCommand)this.seleneseMethods[testCommand.getCommand()];

                testCommand = this.interpolateSeleneseVariables(testCommand);

                // Found the command in the vendor commands.
                String[] args;

                if (testCommand.getTarget().Length > 0 && testCommand.getValue().Length > 0)
                {
                    args = new String[2];
                    args[0] = testCommand.getTarget();
                    args[1] = testCommand.getValue();
                }
                else if (testCommand.getTarget().Length > 0)
                {
                    args = new String[1];
                    args[0] = testCommand.getTarget();
                }
                else
                {
                    args = null;
                }

                try
                {
                    this.log.startTimer();
                    cmd.Apply(this.webDriver, args);
                    this.log.logSuccess(testCommand, "");
                    this.updateCommandStatus(testCommand.getId(), true, "");
                    return true;
                }
                catch (Exception e)
                {
                    String message = "failed: " + e.Message;
                    this.log.logFailure(testCommand, message);

                    this.updateCommandStatus(testCommand.getId(), false, message );
                    this.testHadError = true;
                    return false;
                }
            }



            if (this.seleneseCommands.ContainsKey(testCommand.getCommand()))
            {
                Selenese_Command cmd = (Selenese_Command)this.seleneseCommands[testCommand.getCommand()];
                testCommand = this.interpolateSeleneseVariables(testCommand);

                if (cmd.run(testCommand) == true)
                {
                    return true;
                }
                else
                {
                    this.testHadError = true;
                    return false;
                }

            }

            this.testHadError = true;
            this.log.logFailure(testCommand, "unimplemented selenese");
            return false;

        }

    }
}
