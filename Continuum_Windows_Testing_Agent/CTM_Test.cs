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
using OpenQA.Selenium.Remote;

namespace Continuum_Windows_Testing_Agent
{
    public partial class CTM_Test : Form
    {

        #region Private Variables
        delegate void setTestNameCallback(string text);
        delegate void setSuiteTitleBoxCallback(string text);
        delegate void clearGridCallback();
        delegate void addCommandToGridCallback(Selenium_Test_Trinome cmd);
        delegate void updateCommandStatusCallback(int id, int state, DateTime startTime, DateTime stopTime, String message);
        delegate void updateTestRunProgressCallback(int completed, int total);
        delegate void toggleTestStateCallback();

        private UInt64 testRunId;
        private UInt64 testRunBrowserId;
        private String testDownloadUrl;
        private CTM_WebBrowser testBrowser;
        private Boolean haltOnError;

        private String tempTestDir;
        private String tempZipFile;
        private String testRunIndexHtml;

        public Boolean testHadError;
        private IWebDriver webDriver;
        private Selenium_Test_Suite_Variables testVariables;
        private Selenium_Test_Log log;
        private Dictionary<String, SeleneseCommand> seleneseMethods;
        private ElementFinder elementFinder;
        private SeleniumOptionSelector select;
        private KeyState keyState;

        private ArrayList tests;
        private ArrayList testCommands;

        private IJavaScriptExecutor jsExecutor;

        #endregion Private Variables

        #region Constructor
        public CTM_Test()
        {

            InitializeComponent();

            // init local variables
            this.testRunId = 0;
            this.testRunBrowserId = 0;
            this.testDownloadUrl = "";
            this.testBrowser = null;
            this.haltOnError = false;

            this.testHadError = false;

            this.testVariables = new Selenium_Test_Suite_Variables();

            this.seleneseMethods = new Dictionary<String, SeleneseCommand>();
            this.elementFinder = new ElementFinder();
            this.select = new SeleniumOptionSelector(this.elementFinder);

            this.keyState = new KeyState();

            this.tests = new ArrayList();
            this.testCommands = new ArrayList();


        }
        #endregion Constructor

        #region Getter / Setters
        public UInt64 getTestRunId()
        {
            return this.testRunId;
        }

        public void setTestRunId(UInt64 testRunId)
        {
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

        public CTM_WebBrowser getTestBrowser()
        {
            return this.testBrowser;
        }

        public void setTestBrowser(CTM_WebBrowser browser)
        {
            this.testBrowser = browser;
        }

        public void setHaltOnError(Boolean use)
        {
            this.haltOnError = use;
        }

        public Selenium_Test_Log getSeleniumTestLog()
        {
            return this.log;
        }

        public int getTestHadError()
        {
            if (this.testHadError == true)
            {
                return 0;
            }
            return 1;
        }
        #endregion Getter / Setters

        #region Async Delegates
        private void setTestNameBox(String text)
        {
            if (this.testNameBox.InvokeRequired == true)
            {
                setTestNameCallback d = new setTestNameCallback(setTestNameBox);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.testNameBox.Text = text;
            }
        }
        private void setSuiteTitleBox(String text)
        {
            if (this.testRunNameBox.InvokeRequired == true)
            {
                setSuiteTitleBoxCallback d = new setSuiteTitleBoxCallback(setSuiteTitleBox);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.testRunNameBox.Text = text;
            }
        }

        private void clearGrid()
        {
            if (this.activeTestGrid.InvokeRequired == true)
            {
                clearGridCallback d = new clearGridCallback(clearGrid);
                this.Invoke(d, new object[] { });
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
                this.activeTestGrid.Rows.Add(new string[] { 
                    cmd.getCommand(), 
                    cmd.getTarget(), 
                    cmd.getValue(), 
                    "",
                    "",
                    "",
                    ""
                });
            }
        }

        private void updateCommandStatus(int id, int state, DateTime startTime, DateTime stopTime, String message)
        {
            if (this.activeTestGrid.InvokeRequired == true)
            {
                updateCommandStatusCallback d = new updateCommandStatusCallback(updateCommandStatus);
                this.Invoke(d, new object[] { id, state, startTime, stopTime, message });
            }
            else
            {

                int rowId = id - 1;

                /*
                 if (id > 0)
                 {
                     rowId = rowId - 1;
                 }
                 */

                // Move the selected row to the next item on the stack.
                if (rowId > 0)
                {
                    this.activeTestGrid.Rows[(rowId - 1)].Selected = false;
                }

                this.activeTestGrid.CurrentCell = this.activeTestGrid.Rows[rowId].Cells[0];

                this.activeTestGrid.Rows[rowId].Selected = true;

                // Set the message (we have to allow empty to move the "Working.." message.
                this.activeTestGrid.Rows[rowId].Cells[6].Value = message;

                // Set the date / time fields.
                this.activeTestGrid.Rows[rowId].Cells[3].Value = startTime;
                this.activeTestGrid.Rows[rowId].Cells[4].Value = stopTime;

                TimeSpan elapsed = stopTime - startTime;
                this.activeTestGrid.Rows[rowId].Cells[5].Value = elapsed;

                // Set the state / color of the row based upon the result.
                if (state == -1)
                {
                    this.activeTestGrid.Rows[rowId].DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                }
                if (state == 0)
                {
                    this.activeTestGrid.Rows[rowId].DefaultCellStyle.BackColor = System.Drawing.Color.Red;

                    if (this.haltOnError == true)
                    {
                        // Move the selected row so you can see the failure. 
                        int nextRow = rowId + 1;

                        this.activeTestGrid.Rows[rowId].Selected = false;
                         
                        if (nextRow <= this.activeTestGrid.RowCount)
                        {
                            this.activeTestGrid.Rows[nextRow].Selected = true;
                        }
                    }
                }
                if (state == 1)
                {
                    this.activeTestGrid.Rows[rowId].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                }

                this.activeTestGrid.Refresh();

                // make sure the window lives in the right side bottom (personal preference) JEO - Might want to configure this eventually.
                int x = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
                int y = Screen.PrimaryScreen.WorkingArea.Height - this.Height;

                if (this.Location.X != x && this.Location.Y != y)
                {
                    this.Location = new Point(x, y);
                }

                // Bring the form to the for front.
                this.Activate();

            }
        }

        private void updateTestRunProgress(int completed, int total)
        {
            if (this.testRunProgressBar.InvokeRequired == true)
            {
                updateTestRunProgressCallback d = new updateTestRunProgressCallback(updateTestRunProgress);
                this.Invoke(d, new object[] { completed, total });
            }
            else
            {
                if (this.testRunProgressBar.Maximum != total)
                {
                    this.testRunProgressBar.Maximum = total;
                }
                if (completed > 0)
                {
                    foreach (DataGridViewRow row in this.activeTestGrid.Rows)
                    {
                        Boolean status = false;
                        if (row.DefaultCellStyle.BackColor == System.Drawing.Color.LightGreen)
                        {
                            status = true;
                        }
                        if (row.Cells[0].Value.ToString() == ":comment:")
                        {
                            this.log.insertTestComment(row.Cells[1].Value.ToString());
                        }
                        else
                        {
                            this.log.logTrinome(
                                status,
                                row.Cells[0].Value.ToString(),
                                row.Cells[1].Value.ToString(),
                                row.Cells[2].Value.ToString(),
                                row.Cells[3].Value.ToString(),
                                row.Cells[4].Value.ToString(),
                                row.Cells[5].Value.ToString(),
                                row.Cells[6].Value.ToString()
                            );
                        }
                    }
                }
                this.testRunProgressBar.Value = completed;
            }

        }

        private void toggleTestState()
        {
            if (this.pauseButton.InvokeRequired == true)
            {
                toggleTestStateCallback d = new toggleTestStateCallback(toggleTestState);
                this.Invoke(d, new object[] { });
            }
            else
            {
                if (this.pauseButton.Text == "Pause")
                {

                    this.testLocker.Reset();
                    this.pauseButton.Text = "Resume";
                }
                else
                {
                    this.testLocker.Set();
                    this.pauseButton.Text = "Pause";
                }
            }
        }
        #endregion Async Delegates

        #region Init Testing
        private Boolean initTestingDirectory()
        {
            this.tempTestDir = Environment.GetEnvironmentVariable("TEMP");
            this.tempTestDir += "\\ctmTestRun_" + this.testRunId;

            if (Directory.Exists(this.tempTestDir) == true)
            {
                Directory.Delete(this.tempTestDir, true);
            }

            Directory.CreateDirectory(this.tempTestDir);

            if (Directory.Exists(this.tempTestDir) == false)
            {
                return false;
            }

            return true;

        }

        private Boolean fetchZipFile()
        {
            // fetch the zip file from the remote server.
            this.tempZipFile = Environment.GetEnvironmentVariable("TEMP");
            this.tempZipFile += "\\ctmTestRun_" + this.testRunId + ".zip";

            if (File.Exists(this.tempZipFile) == false)
            {
                File.Delete(this.tempZipFile);
                if (File.Exists(this.tempZipFile) == true)
                {
                    return false;
                }
            }

            // download the file.
            WebClient masterClient = new WebClient();
            masterClient.DownloadFile(this.testDownloadUrl, this.tempZipFile);

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
            catch
            {
                return false;
            }

            return true;

        }

        private Boolean initLogFiles()
        {

            String seleniumLogFile = Environment.GetEnvironmentVariable("TEMP") + "\\selenium_" + this.testRunId + ".html";
            this.log = new Selenium_Test_Log(seleniumLogFile);
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
                return false;
            }

            return true;
        }

        public Boolean getTestsFromTestSuite()
        {
            if (this.tests.Count > 0)
            {
                return true;
            }

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
            this.tests = new ArrayList();

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
                    return false;
                }

                foreach (HtmlNode testRow in doc.DocumentNode.SelectNodes("/html/body/table/tr/td/a[@href]"))
                {
                    this.tests.Add(testRow.Attributes["href"].Value.Replace("./", ""));
                }

            }
            catch
            {
                return false;
            }

            return true;

        }

        public String getSuiteTitle()
        {
            String suiteTitle = "";

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.OptionFixNestedTags = true;

            doc.Load(this.testRunIndexHtml);

            if (doc.ParseErrors != null && doc.ParseErrors.Count() > 0)
            {
                foreach (HtmlParseError htmlError in doc.ParseErrors)
                {
                    Console.WriteLine("error parsing file: " + htmlError.SourceText);
                }
                return suiteTitle;
            }

            foreach (HtmlNode suiteTitleTag in doc.DocumentNode.SelectNodes("/html/head/title"))
            {
                if (suiteTitleTag.InnerText != "")
                {
                    suiteTitle = suiteTitleTag.InnerText;
                }
            }

            return suiteTitle;

        }

        public String getTestTitle(String testFile)
        {
            String testTitle = "Unknown Test";

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.OptionFixNestedTags = true;

            doc.Load(testFile);

            if (doc.ParseErrors != null && doc.ParseErrors.Count() > 0)
            {
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
                        commandId++;
                        triNome.setId(commandId);
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

                    }
                }

            }

            return testCommands;

        }
        #endregion Test File Manipulation

        private Boolean initWebDriver()
        {
            if (this.webDriver != null)
            {
                return true;
            }

            // start up the requested browser.
            switch (this.testBrowser.getInternalName())
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
                case "iphone":
                    DesiredCapabilities remoteCap = new DesiredCapabilities(); 
                    this.webDriver = new RemoteWebDriver(
                        new Uri("http://" + this.testBrowser.getHostname() + ":" + this.testBrowser.getPort() + "/hub/" ),
                        remoteCap
                    );
                    break;
                default:
                    return false;
            }

            // TODO: We should pull this up to the top level init, but the log() requirement 
            // prevents this right now.
            this.initSeleneseCommands();

            // JEO: This is a ghetto hack to help prevent issues where IE does not allow you to 
            // click on non-visibile elements. This can crop up pretty often and hopefully will
            // be fixed in later versions of Web Driver.
            webDriver.Navigate().GoToUrl("http://www.google.com");

            OpenQA.Selenium.IJavaScriptExecutor jsExecutor = (OpenQA.Selenium.IJavaScriptExecutor)webDriver;
            jsExecutor.ExecuteScript("if(window.screen){window.moveTo(0,0);window.resizeTo(window.screen.availWidth, window.screen.availHeight);};");

            return true;

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
                    this.cleanup();
                    return false;
                }


                // download zip file.
                if (this.fetchZipFile() == false)
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

                // run the tests.
                this.initWebDriver();

                this.testLocker.Set();
                this.testRunWorker.RunWorkerAsync();

                // this.log.closeLogFile();

                return true;

            }
            catch
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
            this.webDriver.Quit();
            this.Dispose();
        }


        private void initSeleneseCommands()
        {

            // this.seleneseCommands.Add("waitForPageToLoad", new SeleneseWaitForPageToLoad(this.log, this.webDriver)); - might need to jiggle this, don't know yet.

            // Code we have added or modified
            this.seleneseMethods.Add("addSelection", new CTM_AddSelection(this.elementFinder));                 // Vendor provided version had performance issues, replaced with simpler version.
            this.seleneseMethods.Add("assertElementPresent", new IsElementPresent(this.elementFinder));         // reused from mainline code.
            this.seleneseMethods.Add("assertTextPresent", new CTM_IsTextPresent());                             // reused from mainline code.
            this.seleneseMethods.Add("click", new CTM_Click(this.elementFinder));                               // Modified to include teh pageload wait.
            this.seleneseMethods.Add("clickAndWait", new CTM_ClickAndWait(this.elementFinder));                 // New
            this.seleneseMethods.Add("isTextPresent", new CTM_IsTextPresent());
            this.seleneseMethods.Add("open", new CTM_Open());                                                   // Modified functionality to support param carry over and pageload wait.
            this.seleneseMethods.Add("pause", new CTM_Pause());                                                 // New (Might not be thread safe)
            this.seleneseMethods.Add("store", new CTM_Store(this.testVariables));                               // New our version of store has to talk to the local testVariables stack.
            this.seleneseMethods.Add("type", new CTM_Type(elementFinder, this.keyState));                       // Removed the javascript based replacement. 
            this.seleneseMethods.Add("verifyTextPresent", new CTM_IsTextPresent());                             // reused from mainline code.

            // Vendor provided code we haven't modified.
            // Note the we use the names used by the CommandProcessor
            //seleneseMethods.Add("addLocationStrategy", new AddLocationStrategy(elementFinder));
            //seleneseMethods.Add("altKeyDown", new AltKeyDown(keyState));
            //seleneseMethods.Add("altKeyUp", new AltKeyUp(keyState));
            //seleneseMethods.Add("assignId", new AssignId(elementFinder));
            //seleneseMethods.Add("attachFile", new AttachFile(elementFinder));
            //seleneseMethods.Add("captureScreenshotToString", new CaptureScreenshotToString());
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
            this.seleneseMethods.Add("isElementPresent", new IsElementPresent(this.elementFinder));
            //seleneseMethods.Add("isOrdered", new IsOrdered(elementFinder));
            //seleneseMethods.Add("isSomethingSelected", new IsSomethingSelected(select));
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
            //seleneseMethods.Add("openWindow", new OpenWindow(new GetEval(baseUrl)));
            //seleneseMethods.Add("refresh", new Refresh());
            this.seleneseMethods.Add("removeAllSelections", new RemoveAllSelections(this.elementFinder));
            this.seleneseMethods.Add("removeSelection", new RemoveSelection(this.elementFinder, this.select));
            //seleneseMethods.Add("runScript", new RunScript());
            this.seleneseMethods.Add("select", new SelectOption(this.select));
            //seleneseMethods.Add("selectFrame", new SelectFrame(windows));
            //seleneseMethods.Add("selectWindow", new SelectWindow(windows));
            //seleneseMethods.Add("setBrowserLogLevel", new NoOp(null));
            //seleneseMethods.Add("setContext", new NoOp(null));
            //seleneseMethods.Add("setSpeed", new NoOp(null));
            //////seleneseMethods.Add("setTimeout", new SetTimeout(timer));
            //seleneseMethods.Add("shiftKeyDown", new ShiftKeyDown(keyState));
            //seleneseMethods.Add("shiftKeyUp", new ShiftKeyUp(keyState));
            //seleneseMethods.Add("submit", new Submit(elementFinder));
            //seleneseMethods.Add("typeKeys", new TypeKeys(elementFinder));
            //seleneseMethods.Add("uncheck", new Uncheck(elementFinder));
            //seleneseMethods.Add("useXpathLibrary", new NoOp(null));
            //seleneseMethods.Add("waitForCondition", new WaitForCondition());
            //seleneseMethods.Add("waitForFrameToLoad", new NoOp(null));
            this.seleneseMethods.Add("waitForPageToLoad", new WaitForPageToLoad());
            //seleneseMethods.Add("waitForPopUp", new WaitForPopup(windows));
            //seleneseMethods.Add("windowFocus", new WindowFocus());
            //seleneseMethods.Add("windowMaximize", new WindowMaximize());

        }

        protected String runJavascriptValue(String javascriptValue)
        {

            if (javascriptValue.Length == 0)
            {
                return javascriptValue;
            }

            if (javascriptValue.Contains("javascript") == false)
            {
                return javascriptValue;
            }

            MatchCollection matches = Regex.Matches(javascriptValue, @"javascript{(.*?)}");
            
            if (matches.Count == 0)
            {
                return javascriptValue;
            }

            if (this.jsExecutor == null)
            {
                this.jsExecutor = (IJavaScriptExecutor)this.webDriver;
            }

            foreach (Match match in matches)
            {
                String js = match.Groups[1].Value;
                
                // quick fix for people forgetting to do a return.
                if (js.Contains("return") == false)
                {
                    js = "return " + js;
                }

                if (!js.EndsWith(";"))
                {
                    js = js + ";";
                }

                if (this.jsExecutor.IsJavaScriptEnabled == true)
                {
                   String val = this.jsExecutor.ExecuteScript(js).ToString();
                    javascriptValue = javascriptValue.Replace(
                        "javascript{" + match.Groups[1].ToString() + "}",
                        val
                    );
                }

            }

            return javascriptValue;

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

        public Boolean processSelenese(Selenium_Test_Trinome testCommand, int testCmdCnt )
        {
            
            DateTime startTime = System.DateTime.UtcNow;
            DateTime stopTime = System.DateTime.UtcNow;

            if (testCommand.getCommand() == ":comment:")
            {
                this.updateCommandStatus(testCommand.getId(), 1, startTime, stopTime, "");
                return true;
            }

            if (this.testHadError == true)
            {
                this.updateCommandStatus(testCommand.getId(), 0, startTime, stopTime, "Command not executed: Failure already occurred");
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


                String message = "Working...";

                this.updateCommandStatus(testCommand.getId(), -1, startTime, stopTime, message);
               
                try
                {
                    DateTime s1 = System.DateTime.UtcNow;
                    cmd.Apply(this.webDriver, args);
                    DateTime s2 = System.DateTime.UtcNow;

                    stopTime = System.DateTime.UtcNow;
                    this.updateCommandStatus(testCommand.getId(), 1, startTime, stopTime, "");

                    this.waitForNextCommandTarget(testCommand, testCmdCnt, startTime, stopTime );
                    
                    return true;
                }
                catch (Exception e)
                {
                    message = "failed: " + e.Message;
                    stopTime = System.DateTime.UtcNow;
                    this.updateCommandStatus(testCommand.getId(), 0, startTime, stopTime, message);
                    this.testHadError = true;
                    return false;
                }
            }


            this.testHadError = true;
            this.updateCommandStatus(testCommand.getId(), 0, startTime, stopTime, "Command not executed: Unimplemented selenese.");
            return false;

        }

        private ManualResetEvent testLocker = new ManualResetEvent(true);

        private void pauseButton_Click(object sender, EventArgs e)
        {
            this.toggleTestState();
        }

        private void testRunWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Parse up the suite and start sending it over to the server.
            try
            {
                this.testLocker.WaitOne(1);

                this.getTestsFromTestSuite();

                String suiteTitle = this.getSuiteTitle();
                this.setSuiteTitleBox(suiteTitle);

                this.updateTestRunProgress(0, this.tests.Count);

                // loop across all the tests and run them.
                String testBasedir = Path.GetDirectoryName(this.testRunIndexHtml);

                int completedTestsCnt = 0;
                foreach (String test in this.tests)
                {
                    String testFile = testBasedir + "\\" + test;

                    String testTitle = this.getTestTitle(testFile);

                    // Change the title of the test to our test we are currently working on.
                    this.setTestNameBox(testTitle);

                    // output the test title.
                    this.log.startTestMessage(testTitle);

                    // Reset the test table
                    this.clearGrid();

                    this.testCommands = this.getTestCommands(testFile);

                    // Load the commands into the table
                    foreach (Selenium_Test_Trinome testCommand in this.testCommands)
                    {
                        this.addCommandToGrid(testCommand);
                    }

                    // Run the commands.

                   for( int testCmdCnt = 0; testCmdCnt < this.testCommands.Count; testCmdCnt++ ) {
                       Selenium_Test_Trinome testCommand = (Selenium_Test_Trinome) this.testCommands[testCmdCnt];

                        this.testLocker.WaitOne();

                        Boolean selReturn = this.processSelenese(testCommand, testCmdCnt);

                        

                        if (this.haltOnError == true && selReturn == false)
                        {
                            this.toggleTestState();
                        }

                    }

                    completedTestsCnt++;
                    this.updateTestRunProgress(completedTestsCnt, this.tests.Count);
                }

                // webDriver.Quit();


            }
            catch
            {
                this.testHadError = true;
            }
        }

        private void waitForNextCommandTarget(Selenium_Test_Trinome testCommand, int testCmdCnt, DateTime startTime, DateTime stopTime )
        {
           if (testCommand.getCommand() == "click" ||
                testCommand.getCommand() == "clickAndWait" ||
                testCommand.getCommand() == "open")
            {
                // Find the next valid command.
                Selenium_Test_Trinome nextCommand = this.findNextTestCommand(testCmdCnt);

                int tout = 60;

                // JEO: Pretty sure we need to limit this here to type and other commands
                if (nextCommand != null && nextCommand.getCommand() != "open")
                {
                    for (int i = 0; i < tout; i++)
                    {

                        stopTime = System.DateTime.UtcNow;
                        this.updateCommandStatus(testCommand.getId(), 1, startTime, stopTime, "Waiting for next target..");

                        try
                            {
                                IWebElement elem = this.elementFinder.FindElement(this.webDriver, nextCommand.getTarget());
                                if (elem != null)
                                {

                                    stopTime = System.DateTime.UtcNow;
                                    this.updateCommandStatus(testCommand.getId(), 1, startTime, stopTime, "");
                                    return;
                                }
                                else
                                {
                                    System.Threading.Thread.Sleep(1000);
                                }
                            }
                            catch
                            {
                                // It's okay if this fails we won't do anyting with it anyways.
                            }
                        
                    }

                    stopTime = System.DateTime.UtcNow;
                    this.updateCommandStatus(testCommand.getId(), 0, startTime, stopTime, "Failed to find the next element within " + tout + " seconds");
                    this.testHadError = true;
                    return;

                } // Skinning the cat differntly by avoiding the use of waiter()

 
           }


           return;
        }

        private Selenium_Test_Trinome findNextTestCommand(int offset)
        {
            offset = offset + 1;

            for (int i = offset; i < this.testCommands.Count; i++)
            {
                Selenium_Test_Trinome command = (Selenium_Test_Trinome)this.testCommands[i];
                if (command.getCommand() != ":comment:" &&
                    command.getCommand() != "store" &&
                    command.getCommand() != "pause" )
                {
                    return command;
                }
            }
            return null;
        }
    }
}
