using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using System.Collections;

namespace Selenium.Internal.SeleniumEmulation
{
    internal class CTM_IsTextPresent : SeleneseCommand
    {
        private Regex TextMatchingStrategyAndValueRegex;
        private Hashtable textMatchingStrategies;
        
        public CTM_IsTextPresent()
        {
            this.TextMatchingStrategyAndValueRegex = new Regex("^([glob|regexp|exact]):(.*)");
            this.textMatchingStrategies = new Hashtable();

            this.SetUpTextMatchingStrategies();
        }

        protected override object HandleSeleneseCommand(IWebDriver driver, string pattern, string ignored)
        {
            string text = string.Empty;
            IWebElement body = driver.FindElement(By.XPath("/html/body"));
            
            // JEO not 100pct sure but we think this part is causing performance issues.
            /*
            IJavaScriptExecutor executor = driver as IJavaScriptExecutor;
            if (executor == null)
            {
                text = body.Text;
            }
            else
            {
                text = JavaScriptLibrary.CallEmbeddedHtmlUtils(driver, "getTextContent", body).ToString();
            }
            */
            text = body.Text;
            
            text = text.Trim();

            string strategyName = "implicit";
            string use = pattern;

            if (TextMatchingStrategyAndValueRegex.IsMatch(pattern))
            {
                Match textMatch = TextMatchingStrategyAndValueRegex.Match(pattern);
                strategyName = textMatch.Groups[1].Value;
                use = textMatch.Groups[2].Value;
            }

            ITextMatchingStrategy strategy = (ITextMatchingStrategy) this.textMatchingStrategies[strategyName];
            return strategy.IsAMatch(use, text);
        }

        private void SetUpTextMatchingStrategies()
        {
            this.textMatchingStrategies.Add("implicit", new GlobTextMatchingStrategy());
            this.textMatchingStrategies.Add("glob", new GlobTextMatchingStrategy());
            this.textMatchingStrategies.Add("regexp", new RegexTextMatchingStrategy());
            this.textMatchingStrategies.Add("exact", new ExactTextMatchingStrategy());                
        }
    }
}