using System;
using OpenQA.Selenium;

namespace Selenium.Internal.SeleniumEmulation
{
    internal class CTM_Type : SeleneseCommand
    {
        private ElementFinder finder;
        private KeyState state;

        public CTM_Type(ElementFinder elementFinder, KeyState keyState)
        {
            this.finder = elementFinder;
            this.state = keyState;
        }

        protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string value)
        {
            if (state.ControlKeyDown || state.AltKeyDown || state.MetaKeyDown)
            {
                throw new SeleniumException("type not supported immediately after call to controlKeyDown() or altKeyDown() or metaKeyDown()");
            }

            string stringToType = state.ShiftKeyDown ? value.ToUpperInvariant() : value;

            IWebElement element = finder.FindElement(driver, locator);
            
            // JEO: I don't know why they did the javascript replaceText bit, but it doesn't work on all browsers.
            element.SendKeys(stringToType);

            /*
            IJavaScriptExecutor executor = driver as IJavaScriptExecutor;
            if (executor != null && executor.IsJavaScriptEnabled)
            {
                JavaScriptLibrary.CallEmbeddedSelenium(driver, "replaceText", element, stringToType);
            }
            else
            {
                element.SendKeys(stringToType);
            }
            */

            return null;
        }
    }
}
