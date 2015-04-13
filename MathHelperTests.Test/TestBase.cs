using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MathHelperTests.Test
{
    class TestBase
    {
		public IWebDriver WebDriver = new FirefoxDriver();
        public String Url { get; set; }
		public bool acceptNextAlert = true;
		public StringBuilder verificationErrors;
		public StringBuilder assertionErrors;

		public Int32 passedCounter = 0;
		public Int32 failedCounter = 0;

        public bool IsElementPresent(By by)
        {
            try
            {
                this.WebDriver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public bool IsAlertPresent()
        {
            try
            {
                this.WebDriver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }
		/// <summary>
		/// An expectation for checking whether an element is visible.
		/// </summary>
		/// <param name="locator">The locator used to find the element.</param>
		/// <returns>The <see cref="IWebElement"/> once it is located, visible and clickable.</returns>
		public static Func<IWebDriver, IWebElement> ElementIsClickable(By locator)
		{
			return driver =>
				{
					var element = driver.FindElement(locator);
					return (element != null && element.Displayed && element.Enabled) ? element : null;
				};
		}
        public string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = this.WebDriver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
		//method for checking whether an Stringbuilder is not empty in order to determine whether errors exist or not
		public String hasErrors(StringBuilder errors)
		{
			if(errors.ToString() != "")
			{
				failedCounter += 1;
				return "FAILED\n"+errors.ToString();

			}
			else
			{
				passedCounter += 1;
				return "PASSED";

			}
		}
		/// <summary>
		/// Method that returns a particular link based on a list of IWebElements that are links
		/// and a check for their matching against a user defined regex pattern
		/// </summary>
		/// <param name="links">List of IWebElements</param>
		/// <param name="pattern">User defined regex pattern</param>
		/// <param name="driver">Current instance of IWebDriver</param>
		/// <returns>IWebelenent or null</returns>
		public IWebElement findLink(IReadOnlyCollection<IWebElement> links, String pattern, IWebDriver driver)
		{
			foreach(IWebElement link in links)
			{
				if(Regex.IsMatch(link.Text, pattern))
				{
					return link;
				}
			}
			return null;
		}
		/// <summary>
		/// Method that searches for a button and returns it based on a string pattern
		/// </summary>
		/// <param name="buttons">IWebElement collection of buttons</param>
		/// <param name="pattern">String patttern</param>
		/// <param name="driver">Current instance of the WebDriver</param>
		/// <returns>IWebElement or null</returns>
		public IWebElement findButton(IReadOnlyCollection<IWebElement> buttons, String pattern, IWebDriver driver)
		{
			foreach(IWebElement button in buttons)
			{
				if(Regex.IsMatch(button.Text, pattern))
				{
					return button;
				}
			}
			return null;
		}
		/// <summary>
		/// The following method is similar to findLoginLink and findRegisterLink, except that it
		/// looks for a button and returns it based on the criteria set by a Regex
		/// </summary>
		/// <param name="buttons">List of IWebElements</param>
		/// <param name="driver">Current instance of the webdriver</param>
		/// <returns>Either webelement or null</returns>
		public IWebElement findRegisterButton(IReadOnlyCollection<IWebElement> buttons, IWebDriver driver)
		{
			foreach (IWebElement button in buttons)
			{
				if (Regex.IsMatch(button.Text, "[Rr][Ee][Gg][Ii][Ss][Tt][Ee][Rr]"))
				{
					return button;
				}
				else if (Regex.IsMatch(button.Text, "[Cc]reate [Aa]ccount"))
				{
					return button;
				}
			}
			return null;
		}
		/// <summary>
		/// Find a webelement and if it cannot be found then return null, otherwise return the webelement
		/// </summary>
		/// <param name="driver">current driver instance</param>
		/// <param name="by">method</param>
		/// <param name="toFind">string to use as search argument</param>
		/// <returns>IWebElement or null</returns>
		public IWebElement tryAssignElement(IWebDriver driver, String idToFind)
		{
			//make object because certain elements may be selectelements while others are IWebElements
			IWebElement toReturn;

			try
			{
				toReturn = driver.FindElement(By.Id(idToFind));
				return toReturn;
			}
			catch(Exception)
			{
				return null;
			}
		}
		/// <summary>
		/// This try assignment is the same as tryAssignElement but exclusively for the SelectElement data type
		/// </summary>
		/// <param name="driver">Current instance of the IWebDriver</param>
		/// <param name="idToFind">String pattern of id</param>
		/// <returns>Either SelectElement or null</returns>
		public SelectElement tryAssignSelectElement(IWebDriver driver, String idToFind)
		{
			SelectElement toReturn;

			try
			{
				toReturn = new SelectElement(driver.FindElement(By.Id(idToFind)));
				return toReturn;
			}
			catch(Exception)
			{
				return null;
			}

		}
		[SetUp]
		public void SetupTest()
		{
			//The driver will already be running from the previous test and therefore will not need to be instantiated in the code again
			//driver = new FirefoxDriver();
			//baseURL = this.Url;
			verificationErrors = new StringBuilder();
			assertionErrors = new StringBuilder();
		}
        [TearDown]
        public void TeardownTest()
        {
            try
            {
                this.WebDriver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }
    }
}
