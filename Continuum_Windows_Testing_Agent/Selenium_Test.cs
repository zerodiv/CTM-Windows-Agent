using System;
using System.Text.RegularExpressions;

using System.Collections;
using Continuum_Windows_Testing_Agent.Selenese;
using OpenQA.Selenium;
using Selenium.Internal.SeleniumEmulation;
using System.Collections.Generic;


namespace Continuum_Windows_Testing_Agent
{
    class Selenium_Test
    {
        public Boolean testHadError;
        private IWebDriver webDriver;
        private Selenium_Test_Suite_Variables testVariables;
        private Selenium_Test_Log log;
        private Hashtable seleneseCommands;

        private Hashtable seleneseMethods;
        private ElementFinder elementFinder;
        private SeleniumOptionSelector select;
        private KeyState keyState;


        public Selenium_Test(IWebDriver webDriver, Selenium_Test_Log log)
        {
            this.testHadError = false;
            this.webDriver = webDriver;

            this.testVariables = new Selenium_Test_Suite_Variables();
            this.log = log;

            this.seleneseCommands = new Hashtable();

            this.seleneseMethods = new Hashtable();
            this.elementFinder = new ElementFinder();
            this.select = new SeleniumOptionSelector(this.elementFinder);

            this.keyState = new KeyState();

            this.initSeleneseCommands();

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

            if (this.seleneseMethods.ContainsKey(testCommand.getCommand())) {
                SeleneseCommand cmd = (SeleneseCommand) this.seleneseMethods[testCommand.getCommand()];

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
                    return true;
                }
                catch (Exception e)
                {
                    this.log.logFailure(testCommand, "failed: " + e.Message);
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
                } else {
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
