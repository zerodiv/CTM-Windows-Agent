using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using System.Collections;
using System.Collections.ObjectModel;

namespace Selenium.Internal.SeleniumEmulation
{
    internal class CTM_verifySelectedLabel : SeleneseCommand
    {
        private Regex TextMatchingStrategyAndValueRegex;
        private Dictionary<String,ITextMatchingStrategy> textMatchingStrategies;
        
        public CTM_verifySelectedLabel()
        {
            this.TextMatchingStrategyAndValueRegex = new Regex("^([glob|regexp|exact]):(.*)");
            this.textMatchingStrategies = new Dictionary<String,ITextMatchingStrategy>();

            this.SetUpTextMatchingStrategies();
        }

        protected override object HandleSeleneseCommand(IWebDriver driver, string locator, string pattern)
        {

            String xSelectPath = "//select[@id='" + locator + "' or @name='" + locator + "']";

            IWebElement selectBox = driver.FindElement(By.XPath(xSelectPath));

            ReadOnlyCollection<IWebElement> options = selectBox.FindElements(By.TagName("option"));

            string strategyName = "implicit";
            string use = pattern;

            if (TextMatchingStrategyAndValueRegex.IsMatch(pattern))
            {
                Match textMatch = TextMatchingStrategyAndValueRegex.Match(pattern);
                strategyName = textMatch.Groups[1].Value;
                use = textMatch.Groups[2].Value;
            }

            ITextMatchingStrategy strategy = (ITextMatchingStrategy) this.textMatchingStrategies[strategyName];
            
            foreach ( IWebElement option in options ) {

                String text = String.Empty;
                
                text = option.Text;
                text = text.Trim();

                if ( strategy.IsAMatch(use, text) ) {
                    return true;
                }
            }

            return false;

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
