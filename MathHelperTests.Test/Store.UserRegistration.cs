using NUnit.Framework;
using OpenQA.Selenium;
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
		//Dataset for user registration
		String passwordMis = "hello";
		String registrationPageTitle = "[Rr][Ee][Gg][Ii][Ss][Tt][Ee][Rr]";
		//String homePageTitle = "[Hh]ome";

		//[TestCase]
		public Boolean registrationFormValidation(String firstName, String surname, String email, String password)
		{
			IWebDriver driver = this.WebDriver;
			try
			{
				//incomplete form test
				driver.Navigate().GoToUrl(Url + "/");

				//If there is no link that is explicitly called register, then the navigating to the Login screen will provide the appropriate fields, in certain templates
				IReadOnlyCollection<IWebElement> links = driver.FindElements(By.TagName("a"));
				IWebElement registerLink = findLink(links, "[Rr][Ee][Gg][Ii][Ss][Tt][Ee][Rr]", driver);
				IWebElement loginLink = findLink(links, "[Ll][Oo][Gg][Ii][Nn]", driver);
				if(registerLink == null)
				{
					registerLink.Click();
				}
				else
				{
					loginLink.Click();
				}

				//find the register form on the page
				IWebElement registerForm = tryAssignElement(driver, "register-form");

				driver.FindElement(By.Id("FirstName")).Clear();
				driver.FindElement(By.Id("FirstName")).SendKeys(firstName);
				driver.FindElement(By.Id("Surname")).Clear();
				driver.FindElement(By.Id("Surname")).SendKeys(surname);
				//Cater for template specific layouts
				if (registerForm != null)
				{
					registerForm.FindElement(By.Id("Email")).Clear();
					registerForm.FindElement(By.Id("Email")).SendKeys(email);
				}
				else
				{
					driver.FindElement(By.Id("Email")).Clear();
					driver.FindElement(By.Id("Email")).SendKeys(email);
				}
				findRegisterButton(driver.FindElements(By.TagName("button")), driver).Click();

				//Assert that the page title has not changed (i.e. no navigaiton has taken place) because form validaiton is handled by JS
				Assert.True(Regex.IsMatch(driver.Title, ".*" + registrationPageTitle + ".*"));

				

				//Password mismatch test
				registerForm = tryAssignElement(driver, "register-form");

				driver.FindElement(By.Id("FirstName")).Clear();
				driver.FindElement(By.Id("FirstName")).SendKeys(firstName);
				driver.FindElement(By.Id("Surname")).Clear();
				driver.FindElement(By.Id("Surname")).SendKeys(surname);
				//Cater for template specific layouts
				if (registerForm != null)
				{
					registerForm.FindElement(By.Id("Email")).Clear();
					registerForm.FindElement(By.Id("Email")).SendKeys(email);
					registerForm.FindElement(By.Id("Password")).Clear();
					registerForm.FindElement(By.Id("Password")).SendKeys(password);
				}
				else
				{
					driver.FindElement(By.Id("Email")).Clear();
					driver.FindElement(By.Id("Email")).SendKeys(email);
					driver.FindElement(By.Id("Password")).Clear();
					driver.FindElement(By.Id("Password")).SendKeys(password);
				}
				driver.FindElement(By.Id("ConfirmPassword")).Clear();
				driver.FindElement(By.Id("ConfirmPassword")).SendKeys(passwordMis);
				findRegisterButton(driver.FindElements(By.TagName("button")), driver).Click();

				//Assert that the following text is present:
				Assert.True(Regex.IsMatch(driver.Title, ".*[Rr][Ee][Gg][Ii][Ss][Tt].*"));
				
				//Register existing user test
				registerForm = tryAssignElement(driver, "register-form");

				driver.FindElement(By.Id("FirstName")).Clear();
				driver.FindElement(By.Id("FirstName")).SendKeys(firstName);
				driver.FindElement(By.Id("Surname")).Clear();
				driver.FindElement(By.Id("Surname")).SendKeys(surname);
				//Cater for template specific layouts
				if (registerForm != null)
				{
					registerForm.FindElement(By.Id("Email")).Clear();
					registerForm.FindElement(By.Id("Email")).SendKeys(email);
					registerForm.FindElement(By.Id("Password")).Clear();
					registerForm.FindElement(By.Id("Password")).SendKeys(password);
				}
				else
				{
					driver.FindElement(By.Id("Email")).Clear();
					driver.FindElement(By.Id("Email")).SendKeys(email);
					driver.FindElement(By.Id("Password")).Clear();
					driver.FindElement(By.Id("Password")).SendKeys(password);
				}
				driver.FindElement(By.Id("ConfirmPassword")).Clear();
				driver.FindElement(By.Id("ConfirmPassword")).SendKeys(password);
				findRegisterButton(driver.FindElements(By.TagName("button")), driver).Click();

				//Assert that the driver has not been redirected (i.e. still on the registration screen)
				Assert.True(Regex.IsMatch(driver.Title, ".*" + registrationPageTitle + ".*"));

			}
			catch (Exception e)
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
		//[TestCase]
		public Boolean registerUser(String newUserFirstname, String newUserSurname, String newUserEmail, String newUserPassword)
		{
			IWebDriver driver = this.WebDriver;

			try
			{
				driver.Navigate().GoToUrl(Url);
				IReadOnlyCollection<IWebElement> links = driver.FindElements(By.TagName("a"));
				IWebElement registerLink = findLink(links, "[Rr][Ee][Gg][Ii][Ss][Tt][Ee][Rr]", driver);
				IWebElement loginLink = findLink(links,"[Ll][Oo][Gg][Ii][Nn]" ,driver);
				
				if (registerLink != null)
				{
					registerLink.Click();
				}
				else
				{
					loginLink.Click();
				}

				IWebElement registerForm = tryAssignElement(driver, "register-form");

				driver.FindElement(By.Id("FirstName")).Clear();
				driver.FindElement(By.Id("FirstName")).SendKeys(newUserFirstname);
				driver.FindElement(By.Id("Surname")).Clear();
				driver.FindElement(By.Id("Surname")).SendKeys(newUserSurname);
				//Cater for template specific layouts
				if (registerForm != null)
				{
					registerForm.FindElement(By.Id("Email")).Clear();
					registerForm.FindElement(By.Id("Email")).SendKeys(newUserEmail);
					registerForm.FindElement(By.Id("Password")).Clear();
					registerForm.FindElement(By.Id("Password")).SendKeys(newUserPassword);
				}
				else
				{
					driver.FindElement(By.Id("Email")).Clear();
					driver.FindElement(By.Id("Email")).SendKeys(newUserEmail);
					driver.FindElement(By.Id("Password")).Clear();
					driver.FindElement(By.Id("Password")).SendKeys(newUserPassword);
				}
				driver.FindElement(By.Id("ConfirmPassword")).Clear();
				driver.FindElement(By.Id("ConfirmPassword")).SendKeys(newUserPassword);
				findRegisterButton(driver.FindElements(By.TagName("button")), driver).Click();

				//Assert that the driver has been redirected to the home page of the site
				Assert.True(Regex.IsMatch(driver.Title, homePageTitle));
			}
			catch(Exception e)
			{
				assertionErrors.Append(e.Message);
			}
			//Console.WriteLine("\nRegister New User Test: " + hasErrors(assertionErrors));
			if (Regex.IsMatch(hasErrors(assertionErrors), "PASSED"))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}

