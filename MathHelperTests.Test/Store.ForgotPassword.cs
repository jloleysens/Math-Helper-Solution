using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace MathHelperTests.Test
{
	//[TestFixture]
	partial class Store : TestBase
	{
		//Dataset for forgotten passwords
		String fpNonExistentEmail = "kdjvn@lkdfnj.com";
		String successAlert = ".*email was successfully sent.*";
		String existingEmail = "tester1.warp@gmail.com";
		String existingEmailPassword = "hellopeter*1";
		String mailUrl = "http://mail.google.com";
		String storeName = "kidsemporium";
		String passwordMismatchAlert = ".*do not match.*";
		//[Test]
		public Boolean forgotPasswordNonExistentEmail()
		{
			IWebDriver driver = this.WebDriver;
			try
			{
				//Varibles to used in the following test
				IReadOnlyCollection<IWebElement> buttons;

				driver.Navigate().GoToUrl(Url);
				//The following list will be look for all links on the page and then search for the Login link by way of a regex
				findLink(WebDriver.FindElements(By.TagName("a")),"[Ll][Oo][Gg][Ii][Nn]", WebDriver).Click();
				findForgotPasswordLink(WebDriver.FindElements(By.TagName("a")), WebDriver).Click();
				driver.FindElement(By.Id("Email")).SendKeys(fpNonExistentEmail);
				buttons = WebDriver.FindElements(By.TagName("button"));
				findButton(buttons, "[Ss][Ee][Nn][Dd]", WebDriver).Click();

				//No email will be sent because this is a non-existant email but a success message will be displayed
				Assert.IsTrue(Regex.IsMatch(driver.FindElement(By.ClassName("alert-success")).Text, successAlert));

			}
			catch (Exception e)
			{
				assertionErrors.Append(e.Message);
			}
			//Console.WriteLine("\nForgot Password Non-Existant Mail Test: " + hasErrors(assertionErrors));
			if (Regex.IsMatch(hasErrors(assertionErrors), "PASSED"))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		//[Test]
		public Boolean resetPassword()
		{
			///<summary>
			///This unit test as of yet only works with gmail accounts.
			///</summary>
			///
			IReadOnlyCollection<IWebElement> buttons;
			WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(20));
			//Attempt to reset the passsword with passwords that do not match (i.e. test form validation)
			try{
				WebDriver.Navigate().GoToUrl(Url);
				findLink(WebDriver.FindElements(By.TagName("a")),"[Ll][Oo][Gg][Ii][Nn]", WebDriver).Click();
				findForgotPasswordLink(WebDriver.FindElements(By.TagName("a")), WebDriver).Click();
				WebDriver.FindElement(By.Id("Email")).SendKeys(existingEmail);
				buttons = WebDriver.FindElements(By.TagName("button"));
				findButton(buttons, "[Ss][Ee][Nn][Dd]", WebDriver).Click();

				//once again this message will appear indicating that the message has been sent
				Assert.IsTrue(Regex.IsMatch(WebDriver.FindElement(By.ClassName("alert-success")).Text, successAlert));

				//navigate to the user's email address
				/*WebDriver.Navigate().GoToUrl(mailUrl);
				WebDriver.FindElement(By.Id("Email")).SendKeys(existingEmail);
				WebDriver.FindElement(By.Id("Passwd")).SendKeys(existingEmailPassword);
				WebDriver.FindElement(By.Id("signIn")).Click();

				wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[7]/div[3]/div/div[2]/div[1]/div[2]/div/div/div/div[2]/div[1]/div[1]/div/div/div[8]/div/div[1]/div[2]/div/table/tbody/tr[1]")));
				WebDriver.FindElement(By.XPath("/html/body/div[7]/div[3]/div/div[2]/div[1]/div[2]/div/div/div/div[2]/div[1]/div[1]/div/div/div[8]/div/div[1]/div[2]/div/table/tbody/tr[1]")).Click();
				WebDriver.FindElement(By.PartialLinkText(storeName)).Click();

				WebDriver.SwitchTo().Window(WebDriver.WindowHandles.Last());

				WebDriver.FindElement(By.Id("Password")).SendKeys(existingEmailPassword);
				//Make the passwords not matching to test form validation
				WebDriver.FindElement(By.Id("ConfirmPassword")).SendKeys(existingEmailPassword + "d");
				//Assert that the alert has appeared on the screen
				WebDriver.FindElement(By.ClassName("alert-danger"));
				WebDriver.FindElement(By.XPath("//fieldset/div[4]/div/button")).Click();
				Assert.IsTrue(Regex.IsMatch(WebDriver.FindElement(By.ClassName("alert-danger")).Text, passwordMismatchAlert));

				WebDriver.FindElement(By.Id("Password")).Clear();
				WebDriver.FindElement(By.Id("Password")).SendKeys(existingEmailPassword);
				WebDriver.FindElement(By.Id("ConfirmPassword")).Clear();
				WebDriver.FindElement(By.Id("ConfirmPassword")).SendKeys(existingEmailPassword);

				WebDriver.FindElement(By.XPath("//fieldset/div[4]/div/button")).Click();

				Assert.True(Regex.IsMatch(WebDriver.Title, ".*[Ll][Oo][Gg][Ii][Nn].*"));*/
			}
			catch(Exception e)
			{
				assertionErrors.Append(e.Message);
			}
			if (Regex.IsMatch(hasErrors(assertionErrors), "PASSED"))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public IWebElement findForgotPasswordLink(IReadOnlyCollection<IWebElement> links, IWebDriver driver)
		{
			foreach(IWebElement link in links)
			{
				if(Regex.IsMatch(link.Text, ".*[Ff][Oo][Rr][Gg][Oo][Tt].*"))
				{
					return link;
				}
				if(Regex.IsMatch(link.Text, ".*[Rr][Ee][Ss][Ee][Tt].*"))
				{
					return link;
				}
			}
			return null;
		}
	}
}
