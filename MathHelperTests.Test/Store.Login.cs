using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MathHelperTests.Test
{
	//[TestFixture]
	partial class Store : TestBase
	{
		//Dataset for store tests log in tests
		String nonExistentEmail = "klsdf@sldkf.com";
		String validEmail = "tester1.warp@gmail.com";
		String invalidPassword = "hellopeter";
		String homePageTitle = "[Hh][Oo][Mm][Ee]";
		String pageTitle = "[Ll][Oo][Gg][Ii][Nn]";
		//String loginErrorText = "Invalid username and password combination! Please try again!";
		//String validPassword = "hellopeter*1";

		//[Test]
		public void loginFormValidation()
		{
			IWebDriver driver = this.WebDriver;
			try
			{
				//incomelete login form test
				
				driver.Navigate().GoToUrl(Url);
				//The following list will store all of the links on the page and then select the LOGIN link by way of a regex
				findLink(driver.FindElements(By.TagName("a")), "[Ll][Oo][Gg][Ii][Nn]", driver).Click();

				IWebElement loginForm = tryAssignElement(driver, "login-form");

				if (loginForm != null)
				{
					loginForm.FindElement(By.Id("Email")).Clear();
					loginForm.FindElement(By.Id("Email")).SendKeys(validEmail);
				}
				else
				{
					driver.FindElement(By.Id("Email")).Clear();
					driver.FindElement(By.Id("Email")).SendKeys(validEmail);
				}
				findLoginButton(driver.FindElements(By.TagName("button")), driver).Click();
			
				Assert.True(Regex.IsMatch(driver.Title, ".*" + pageTitle + ".*"));

				//non existing user name test
				findLink(driver.FindElements(By.TagName("a")), "[Ll][Oo][Gg][Ii][Nn]", driver).Click();
				loginForm = tryAssignElement(driver, "login-form");

				if (loginForm != null)
				{
					loginForm.FindElement(By.Id("Email")).Clear();
					loginForm.FindElement(By.Id("Email")).SendKeys(nonExistentEmail);
				}
				else
				{
					driver.FindElement(By.Id("Email")).Clear();
					driver.FindElement(By.Id("Email")).SendKeys(nonExistentEmail);
				}
				driver.FindElement(By.Id("Password")).SendKeys("LoremIpsum");
				findLoginButton(driver.FindElements(By.TagName("button")), driver).Click();

				Assert.True(Regex.IsMatch(driver.Title, ".*" + pageTitle + ".*"));

				//Incorrect Email Password pair test
				findLink(driver.FindElements(By.TagName("a")), "[Ll][Oo][Gg][Ii][Nn]", driver).Click();
				loginForm = tryAssignElement(driver, "login-form");
				if (loginForm != null)
				{
					loginForm.FindElement(By.Id("Email")).Clear();
					loginForm.FindElement(By.Id("Email")).SendKeys(validEmail);
					loginForm.FindElement(By.Id("Password")).Clear();
					loginForm.FindElement(By.Id("Password")).SendKeys(invalidPassword);
				}
				else
				{
					driver.FindElement(By.Id("Email")).Clear();
					driver.FindElement(By.Id("Email")).SendKeys(validEmail);
					driver.FindElement(By.Id("Password")).Clear();
					driver.FindElement(By.Id("Password")).SendKeys(invalidPassword);
				}
				findLoginButton(driver.FindElements(By.TagName("a")), driver).Click();

				Assert.True(Regex.IsMatch(driver.Title, ".*" + pageTitle + ".*"));
			}
			catch (Exception e)
			{
				assertionErrors.Append(e.Message);
			}
			Console.WriteLine("\nLogin Form Validation Test: " + hasErrors(assertionErrors));
		}
		//[TestCase]
		public void successfulLogin(String email, String password)
		{
			IWebDriver driver = this.WebDriver;
			try
			{
				

				driver.Navigate().GoToUrl(Url);
				findLink(driver.FindElements(By.TagName("a")), "[Ll][Oo][Gg][Ii][Nn]", driver).Click();
				IWebElement loginForm = tryAssignElement(driver, "login-form");
				if (loginForm != null)
				{
					loginForm.FindElement(By.Id("Email")).Clear();
					loginForm.FindElement(By.Id("Email")).SendKeys(email);
					loginForm.FindElement(By.Id("Password")).Clear();
					loginForm.FindElement(By.Id("Password")).SendKeys(password);
				}
				else
				{
					driver.FindElement(By.Id("Email")).Clear();
					driver.FindElement(By.Id("Email")).SendKeys(email);
					driver.FindElement(By.Id("Password")).Clear();
					driver.FindElement(By.Id("Password")).SendKeys(password);
				}
				findLoginButton(driver.FindElements(By.TagName("button")), driver).Click();

				//Assert that the following text is present on the page after successful log in
				IReadOnlyCollection<IWebElement> links = driver.FindElements(By.TagName("a"));

				Boolean loggedIn = false;
				
				foreach(IWebElement link in links)
				{
					if (Regex.IsMatch(link.Text, "[Aa][Cc][Cc][Oo][Uu][Nn][Tt]"))
					{
						loggedIn = true;
					}
				}

				Assert.True(loggedIn);
			}
			catch (Exception e)
			{
				assertionErrors.Append(e.Message);
			}
			Console.WriteLine("\nSuccessful Login Test: " + hasErrors(assertionErrors));
		}
		//[TestCase]
		public void successfulLogOut()
		{
			IWebDriver driver = this.WebDriver;
			try
			{

				driver.Navigate().GoToUrl(Url);
				
				IReadOnlyCollection<IWebElement> links = driver.FindElements(By.TagName("a"));
				IWebElement accountLink = null;

				foreach(IWebElement link in links)
				{
					if(Regex.IsMatch(link.Text, "[Aa][Cc][Cc][Oo][Uu][Nn][Tt]"))
					{
						accountLink = link;
						break;
					}
				}

				//Assert that we are in fact logged in
				Assert.True(accountLink != null);

				accountLink.Click();

				foreach(IWebElement link in links)
				{
					if (Regex.IsMatch(link.Text, "[Ll][Oo][Gg][Oo][Uu][Tt]"))
					{
						link.Click();
						break;
					}
				}
				//Assert that we are currently on the home page (i.e. after logging out)
				Assert.True(Regex.IsMatch(driver.Title, ".*" + homePageTitle + ".*"));
				links = driver.FindElements(By.TagName("a"));
				//Assert that there is a login link to confirm that we have actually logged out
				Assert.True(findLink(links, ".*[Ll][Oo][Gg][Ii][Nn].*", driver) != null);

			}
			catch (Exception e)
			{
				assertionErrors.Append(e.Message);
			}
			Console.WriteLine("\nSuccessful Log Out Test: " + hasErrors(assertionErrors));
		}
		/// <summary>
		/// The following method is similar to findLoginLink and findRegisterLink, except that it
		/// looks for a button and returns it based on the criteria set by a Regex
		/// </summary>
		/// <param name="buttons">List of IWebElements</param>
		/// <param name="driver">Current instance of the webdriver</param>
		/// <returns>Either webelement or null</returns>
		public IWebElement findLoginButton(IReadOnlyCollection<IWebElement> buttons, IWebDriver driver)
		{
			foreach (IWebElement button in buttons)
			{
				if (Regex.IsMatch(button.Text, "[Ll][Oo][Gg][Ii][Nn]"))
				{
					return button;
				}
				if(Regex.IsMatch(button.Text, ".*[Ss][Ii][Gg][Nn].*"))
				{
					return button;
				}
			}
			return null;
		}
	}
}
