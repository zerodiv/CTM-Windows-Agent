using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using System.Collections;

namespace Selenium.Internal.SeleniumEmulation
{

    internal class CTM_FoundSelection
    {
        public IWebElement selectBox;
        public IWebElement option;

        public CTM_FoundSelection()
        {
            this.selectBox = null;
            this.option = null;
        }

    }

    /// <summary>
    /// Defines the command for the addSelection keyword.
    /// </summary>
    internal class CTM_AddSelection : SeleneseCommand
    {

        public CTM_AddSelection()
        {
        }

        private CTM_FoundSelection findOptimized(IWebDriver driver, String locator, String value)
        {
            CTM_FoundSelection found = new CTM_FoundSelection();

            try
            {

                if (value.StartsWith("label="))
                {
                    value = value.Replace("label=", "");
                }

                if (locator.StartsWith("name="))
                {
                    locator = locator.Replace("name=", "");
                }

                // Orig:
                // String xByNamePath = "//select[@id='" + locator + "' or @name='" + locator + "']/option[normalize-space(.)='" + value + "']";

                String xSelectPath = "//select[@id='" + locator + "' or @name='" + locator + "']";
                String xOptionPath = "//option[normalize-space(.)='" + value + "']";

                found.selectBox = driver.FindElement(By.XPath(xSelectPath));

                if (found.selectBox != null)
                {
                    found.option = found.selectBox.FindElement(By.XPath(xOptionPath));
                }

            }
            catch
            {
            }

            return found;

        }

        protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
        {
            CTM_FoundSelection found = this.findOptimized(driver, locator, value);
            
            if (found.selectBox == null)
            {
                throw new Exception("Failed to find selectbox");
            }

            if (found.option == null)
            {
                throw new Exception("Failed to find selectbox option");
            }

            found.option.Select();


            OpenQA.Selenium.IJavaScriptExecutor jsExecutor = (OpenQA.Selenium.IJavaScriptExecutor)driver;

            StringBuilder ctmOnChangeJs = new StringBuilder();
            ctmOnChangeJs.AppendLine("if ( typeof jQuery == 'function' ) {");
            ctmOnChangeJs.AppendLine("   var ctmjQuery = jQuery.noConflict();");
            ctmOnChangeJs.AppendLine("   ctmjQuery('#" + found.selectBox.GetAttribute("id") + "').trigger('change');");
            ctmOnChangeJs.AppendLine("} else {");
            ctmOnChangeJs.AppendLine("   var ctmElem = document.getElementById('" + found.selectBox.GetAttribute("id") + "');");
            ctmOnChangeJs.AppendLine("   if (ctmElem.onchange != null ) {");
            ctmOnChangeJs.AppendLine("       ctmElem.onchange();");
            ctmOnChangeJs.AppendLine("   }");
            ctmOnChangeJs.AppendLine("}");

            try
            {
                jsExecutor.ExecuteScript(ctmOnChangeJs.ToString());
            }
            catch
            {
            }
            /*
            String jsChangeCall = 
                "ctmElem = document.getElementById('" + found.selectBox.GetAttribute("id") + "'); " +
                "if (ctmElem.onchange != null ) { ctmElem.onchange(); }";
            jsExecutor.ExecuteScript(jsChangeCall);
            */

            return null;
        }
    }
}
