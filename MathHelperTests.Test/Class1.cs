using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Firefox;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace MathHelperTests.Test
{
	[TestFixture, Category("Unit")]
    public class TestClass
    {
		[Test]
		public void AddTest()
		{
			ConsoleApplication1.MathsHelper helper = new ConsoleApplication1.MathsHelper();
			int result = helper.Add(20, 10);
			Assert.AreEqual(30, result);
		}

		[Test]
		public void SubtractTest()
		{
			ConsoleApplication1.MathsHelper helper = new ConsoleApplication1.MathsHelper();
			int result = helper.Subtract(20, 10);
			Assert.AreEqual(10, result);
		}
	}
	[TestFixture, Category("UI")]
	public class StoreTests
	{
		/// <summary>
		/// In this collection of tests various store related features will be tested from the UI perspective (i.e. regression testing)
		/// </summary>
		
		//This is the store object that will be used for the regression tests (also opens the FF instance)
		Store store = new Store();
		String Url = "http://mystore.storefront.co.za";
		private String uname = randomUname(); //This variable will be used to derive random username and email addresses for testing purposes

		//Dataset for registration forma validation and registration
		String registrationPageTitle = "[Rr][Ee][Gg][Ii][Ss][Tt][Ee][Rr]";

		//Dataset for store tests log in tests
		String nonExistentEmail = "klsdf@sldkf.com";
		String validEmail = "tester1.warp@gmail.com";
		String invalidPassword = "hellopeter*1";
		String homePageTitle = "[Hh][Oo][Mm][Ee]";
		String pageTitle = "[Ll][Oo][Gg][Ii][Nn]";
		//String loginErrorText = "Invalid username and password combination! Please try again!";
		String validPassword = "hellopeter*1";

		[Test]
		public void AsuccessfulLogin()
		{
			IWebDriver driver = this.store.WebDriver;

			driver.Navigate().GoToUrl(Url);
			store.findLink(driver.FindElements(By.TagName("a")), "[Ll][Oo][Gg][Ii][Nn]", driver).Click();
			IWebElement loginForm = store.tryAssignElement(driver, "login-form");
			if (loginForm != null)
			{
				loginForm.FindElement(By.Id("Email")).Clear();
				loginForm.FindElement(By.Id("Email")).SendKeys(validEmail);
				loginForm.FindElement(By.Id("Password")).Clear();
				loginForm.FindElement(By.Id("Password")).SendKeys(validPassword);
			}
			else
			{
				driver.FindElement(By.Id("Email")).Clear();
				driver.FindElement(By.Id("Email")).SendKeys(validEmail);
				driver.FindElement(By.Id("Password")).Clear();
				driver.FindElement(By.Id("Password")).SendKeys(validPassword);
			}
			store.findLoginButton(driver.FindElements(By.TagName("button")), driver).Click();
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
			

		[Test]
		public void BsuccessfulLogOut()
		{
			IWebDriver driver = this.store.WebDriver;
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
			Assert.True(store.findLink(links, ".*[Ll][Oo][Gg][Ii][Nn].*", driver) != null);

		}

		[Test]
		public void CregistrationFormValidation()
		{
			//Data
			String email = uname + "@" + uname + ".com";

			IWebDriver driver = this.store.WebDriver;
			//incomplete form test
			driver.Navigate().GoToUrl(Url + "/");

			//If there is no link that is explicitly called register, then the navigating to the Login screen will provide the appropriate fields, in certain templates
			IReadOnlyCollection<IWebElement> links = driver.FindElements(By.TagName("a"));
			IWebElement registerLink = store.findLink(links, "[Rr][Ee][Gg][Ii][Ss][Tt][Ee][Rr]", driver);
			IWebElement loginLink = store.findLink(links, ".*[Ll][Oo][Gg][Ii][Nn].*", driver);
			if(registerLink == null)
			{
				registerLink.Click();
			}
			else
			{
				loginLink.Click();
			}

			//find the register form on the page
			IWebElement registerForm = store.tryAssignElement(driver, "register-form");

			driver.FindElement(By.Id("FirstName")).Clear();
			driver.FindElement(By.Id("FirstName")).SendKeys(uname);
			driver.FindElement(By.Id("Surname")).Clear();
			driver.FindElement(By.Id("Surname")).SendKeys(uname);
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
			store.findRegisterButton(driver.FindElements(By.TagName("button")), driver).Click();
			//Assert that the page title has not changed (i.e. no navigaiton has taken place) because form validaiton is handled by JS
			Assert.True(Regex.IsMatch(driver.Title, ".*" + registrationPageTitle + ".*"));
							
			//Password mismatch test
			registerForm = store.tryAssignElement(driver, "register-form");
			driver.FindElement(By.Id("FirstName")).Clear();
			driver.FindElement(By.Id("FirstName")).SendKeys(uname);
			driver.FindElement(By.Id("Surname")).Clear();
			driver.FindElement(By.Id("Surname")).SendKeys(uname);
			//Cater for template specific layouts
			if (registerForm != null)
			{
				registerForm.FindElement(By.Id("Email")).Clear();
				registerForm.FindElement(By.Id("Email")).SendKeys(email);
				registerForm.FindElement(By.Id("Password")).Clear();
				registerForm.FindElement(By.Id("Password")).SendKeys(uname);
			}
			else
			{
				driver.FindElement(By.Id("Email")).Clear();
				driver.FindElement(By.Id("Email")).SendKeys(email);
				driver.FindElement(By.Id("Password")).Clear();
				driver.FindElement(By.Id("Password")).SendKeys(uname);
			}
			driver.FindElement(By.Id("ConfirmPassword")).Clear();
			driver.FindElement(By.Id("ConfirmPassword")).SendKeys(uname + "1");
			store.findRegisterButton(driver.FindElements(By.TagName("button")), driver).Click();
			//Assert that the following text is present:
			Assert.True(Regex.IsMatch(driver.Title, "[Rr][Ee][Gg][Ii][Ss][Tt][Ee][Rr]"));
				
			//Register existing user test
			registerForm = store.tryAssignElement(driver, "register-form");

			driver.FindElement(By.Id("FirstName")).Clear();
			driver.FindElement(By.Id("FirstName")).SendKeys(uname); //on mystore this email will already be registered
			driver.FindElement(By.Id("Surname")).Clear();
			driver.FindElement(By.Id("Surname")).SendKeys(uname);
			//Cater for template specific layouts
			if (registerForm != null)
			{
				registerForm.FindElement(By.Id("Email")).Clear();
				registerForm.FindElement(By.Id("Email")).SendKeys("tester1.warp@gmail.com");
				registerForm.FindElement(By.Id("Password")).Clear();
				registerForm.FindElement(By.Id("Password")).SendKeys(uname);
			}
			else
			{
				driver.FindElement(By.Id("Email")).Clear();
				driver.FindElement(By.Id("Email")).SendKeys(email);
				driver.FindElement(By.Id("Password")).Clear();
				driver.FindElement(By.Id("Password")).SendKeys(uname);
			}
			driver.FindElement(By.Id("ConfirmPassword")).Clear();
			driver.FindElement(By.Id("ConfirmPassword")).SendKeys(uname);
			store.findRegisterButton(driver.FindElements(By.TagName("button")), driver).Click();
			
			//Assert that the driver has not been redirected (i.e. still on the registration screen)
			Assert.True(Regex.IsMatch(driver.Title, ".*" + registrationPageTitle + ".*"));
			
		}

		[Test]
		public void DforgotPasswordNonExistentEmail()
		{
			//Data
			String fpNonExistentEmail = "3274822@835.co.za";
			IReadOnlyCollection<IWebElement> buttons;

			IWebDriver WebDriver = this.store.WebDriver;
			WebDriver.Navigate().GoToUrl(Url);
			//The following list will be look for all links on the page and then search for the Login link by way of a regex
			store.findLink(WebDriver.FindElements(By.TagName("a")), "[Ll][Oo][Gg][Ii][Nn]", WebDriver).Click();
			store.findForgotPasswordLink(WebDriver.FindElements(By.TagName("a")), WebDriver).Click();
			WebDriver.FindElement(By.Id("Email")).SendKeys(fpNonExistentEmail);
			buttons = WebDriver.FindElements(By.TagName("button"));
			store.findButton(buttons, "[Ss][Ee][Nn][Dd]", WebDriver).Click();

			//No email will be sent because this is a non-existant email but a success message will be displayed
			Assert.IsTrue(Regex.IsMatch(WebDriver.FindElement(By.ClassName("alert-success")).Text, "[Ss][Uu][Cc][Cc][Ee][Ss][Ss]"));
			
		}

		[Test]
		public void EregisterUser()
		{
			IWebDriver driver = this.store.WebDriver;
			driver.Navigate().GoToUrl(Url);

			//Data & Variables for this test
			IReadOnlyCollection<IWebElement> links = driver.FindElements(By.TagName("a"));
			IWebElement registerLink = store.findLink(links, "[Rr][Ee][Gg][Ii][Ss][Tt][Ee][Rr]", driver);
			IWebElement loginLink = store.findLink(links, ".*[Ll][Oo][Gg][Ii][Nn].*", driver);
			String email = uname + "@" + uname + ".com";
				
			if (registerLink != null)
			{
				registerLink.Click();
			}
			else
			{
				loginLink.Click();
			}

			IWebElement registerForm = store.tryAssignElement(driver, "register-form");

			driver.FindElement(By.Id("FirstName")).Clear();
			driver.FindElement(By.Id("FirstName")).SendKeys(uname);
			driver.FindElement(By.Id("Surname")).Clear();
			driver.FindElement(By.Id("Surname")).SendKeys(uname);
			//Cater for template specific layouts
			if (registerForm != null)
			{
				registerForm.FindElement(By.Id("Email")).Clear();
				registerForm.FindElement(By.Id("Email")).SendKeys(email);
				registerForm.FindElement(By.Id("Password")).Clear();
				registerForm.FindElement(By.Id("Password")).SendKeys(uname);
			}
			else
			{
				driver.FindElement(By.Id("Email")).Clear();
				driver.FindElement(By.Id("Email")).SendKeys(email);
				driver.FindElement(By.Id("Password")).Clear();
				driver.FindElement(By.Id("Password")).SendKeys(uname);
			}
			driver.FindElement(By.Id("ConfirmPassword")).Clear();
			driver.FindElement(By.Id("ConfirmPassword")).SendKeys(uname);
			store.findRegisterButton(driver.FindElements(By.TagName("button")), driver).Click();

			//Assert that the driver has been redirected to the home page of the site
			Assert.True(Regex.IsMatch(driver.Title, homePageTitle));
		}

		[Test]
		public void FaddItemsToBasket()
		{
			//Data and variables
			String productToSearchFor = "Product1Test";//The assumption here is that the product has been created in the store by way of the API

			IWebDriver WebDriver = this.store.WebDriver;

			WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(2));
			//Add a product and its variant (find product via search functionality)

			//Homepage
			WebDriver.Navigate().GoToUrl(Url);

			IWebElement searchForm = store.tryAssignElement(WebDriver, "search-form");
			//Depending on the template being used; if there is no visible search form then navigate to the following url
			if(!searchForm.Displayed || searchForm == null){
				//via URL
				WebDriver.Navigate().GoToUrl(Url + "/search/results?term=" + productToSearchFor);
			}
			else{
				//via UI
				searchForm.FindElement(By.Id("term")).SendKeys(productToSearchFor);
				searchForm.FindElement(By.TagName("button")).Click();
			}
			IReadOnlyCollection<IWebElement> links = WebDriver.FindElements(By.TagName("a"));
			store.findLink(links, productToSearchFor, WebDriver).Click();

			//declare the variables that are to be used in the coming for loops.
			Int32 segmentNumber;
			SelectElement variantElement;
			//Add the first product (2 variants)
			for (int x = 1; x < 3; x++)
			{
				segmentNumber = 0;
				variantElement = store.tryAssignSelectElement(WebDriver, "Segment_" + segmentNumber.ToString());
				
				//If there are variants to be selected then do the following
				while (variantElement != null)
				{
					variantElement.SelectByIndex(x);
					segmentNumber += 1;
					variantElement = store.tryAssignSelectElement(WebDriver, "Segment_" + segmentNumber.ToString());
				}
				//Checking whether particular product available
				Thread.Sleep(TimeSpan.FromSeconds(2));

				links = WebDriver.FindElements(By.TagName("a"));
				store.findLink(links,"[Aa][Dd][Dd].*[Cc][Aa][Rr][Tt]", WebDriver).Click();

				//Wait for the basket amount to be updated
				Thread.Sleep(TimeSpan.FromSeconds(1));
				Assert.True(store.tryAssignElement(WebDriver, "added-to-cart") != null);

				//Keep track of the total basket amount
				//Console.WriteLine(WebDriver.FindElement(By.Id("product-price")).Text);
				//String productPriceText = WebDriver.FindElement(By.Id("product-price")).Text;
				//totalBasketAmount += Convert.ToDouble(productPriceText.Substring(1), CultureInfo.InvariantCulture);
				//totalBasketAmount += parseQuantity(productPriceText);
			}
			//Add a third and a fourth product (from the featured products section on the homepage)

			for (Int32 x = 0; x < 2; x++)
			{
				WebDriver.Navigate().GoToUrl(Url);

				IWebElement featuredProducts = store.tryAssignElement(WebDriver, "featured-products");
				if (featuredProducts == null)
				{
					featuredProducts = store.tryAssignElement(WebDriver, "featured");
				}
				//All of the links in the featured products section, here assumed to be the products themselves.
				IReadOnlyCollection<IWebElement> featuredProductsLinks = featuredProducts.FindElements(By.TagName("a"));
				//Click on the first link (any product)
				foreach (IWebElement link in featuredProductsLinks)
				{
					if (link.Displayed && link.Enabled && link.FindElement(By.TagName("img")) != null)
					{
						link.Click();
						break;
					}
				}

				//add the item
				segmentNumber = 0;
				variantElement = store.tryAssignSelectElement(WebDriver, "Segment_" + segmentNumber.ToString());

				//If there are variants to be selected then do the following
				while (variantElement != null)
				{
					variantElement.SelectByIndex(1);
					segmentNumber += 1;
					variantElement = store.tryAssignSelectElement(WebDriver, "Segment_" + segmentNumber.ToString());
				}
				//Checking whether particular product available
				Thread.Sleep(TimeSpan.FromSeconds(2));

				links = WebDriver.FindElements(By.TagName("a"));
				store.findLink(links, "[Aa][Dd][Dd].*[Cc][Aa][Rr][Tt]", WebDriver).Click();

				//Wait for the basket amount to be updated
				Thread.Sleep(TimeSpan.FromSeconds(1));
				Assert.True(store.tryAssignElement(WebDriver, "added-to-cart") != null);

				//Keep track of the total basket amount
				//Console.WriteLine(WebDriver.FindElement(By.Id("product-price")).Text);
				String productPriceText = WebDriver.FindElement(By.Id("product-price")).Text;
				//totalBasketAmount += Convert.ToDouble(productPriceText.Substring(1), CultureInfo.InvariantCulture);
				//totalBasketAmount += parseQuantity(productPriceText);
				//Console.WriteLine(totalBasketAmount);
			}
				//Add the first variant of the first product again (i.e. fifth product)

			WebDriver.Navigate().GoToUrl(Url + "/search/results?term=" + productToSearchFor);
			
			links = WebDriver.FindElements(By.TagName("a"));
			store.findLink(links, productToSearchFor, WebDriver).Click();

			segmentNumber = 0;
			variantElement = store.tryAssignSelectElement(WebDriver, "Segment_" + segmentNumber.ToString());

			//If there are variants to be selected then do the following
			while (variantElement != null)
			{
				variantElement.SelectByIndex(1);
				segmentNumber += 1;
				variantElement = store.tryAssignSelectElement(WebDriver, "Segment_" + segmentNumber.ToString());
			}
			//Checking item availability and price
			Thread.Sleep(TimeSpan.FromSeconds(2));

			links = WebDriver.FindElements(By.TagName("a"));
			store.findLink(links, "[Aa][Dd][Dd].*[Cc][Aa][Rr][Tt]", WebDriver).Click();
			
			//Wait for the basket amount to be updated
			Thread.Sleep(TimeSpan.FromSeconds(1));
			Assert.True(store.tryAssignElement(WebDriver, "added-to-cart") != null);

			//Keep track of the total basket amount
			//Console.WriteLine(WebDriver.FindElement(By.Id("product-price")).Text);
			//totalBasketAmount += Convert.ToDouble(productPriceText.Substring(1), CultureInfo.InvariantCulture);
			//totalBasketAmount += parseQuantity(WebDriver.FindElement(By.Id("product-price")).Text);
			
			}


		[Test]
		public void GaddGiftCards()
		{
			String giftCardSenderName = "Peter";
			String giftCardReceiverEmail = "tester1.warp@gmail.com";
			String[] giftCardAmount = { "500", "350", "450" };

			//Declare the variables that will be used in the test
			Int32 x = 0;
			IReadOnlyCollection<IWebElement> giftCardImages;
			IReadOnlyCollection<IWebElement> links;
			IReadOnlyCollection<IWebElement> buttons;

			IWebDriver WebDriver = this.store.WebDriver;
			WebDriver.Navigate().GoToUrl(Url);

			links = WebDriver.FindElements(By.TagName("a"));
			//Find the gift cards link on the homepage
			store.findLink(links, "[Gg][Ii][Ff][Tt].*[Cc][Aa][Rr][Dd]", WebDriver).Click();

			//Assumption here is that one of these will contain an img that will be a gift card


			//Add 3 different cards
			while (x < 3)
			{
				giftCardImages = WebDriver.FindElements(By.ClassName("gift-card-picture"));
				giftCardImages.ElementAt(0).Click();
				WebDriver.FindElement(By.Id("CustomValue")).Clear();
				WebDriver.FindElement(By.Id("CustomValue")).SendKeys(giftCardAmount[x].ToString());
				WebDriver.FindElement(By.Id("CustomMessage")).Clear();
				WebDriver.FindElement(By.Id("CustomMessage")).SendKeys("Yaay!");
				WebDriver.FindElement(By.Id("FromName")).Clear();
				WebDriver.FindElement(By.Id("FromName")).SendKeys(giftCardSenderName);
				WebDriver.FindElement(By.Id("RecipientEmail")).Clear();
				WebDriver.FindElement(By.Id("RecipientEmail")).SendKeys(giftCardReceiverEmail);
				WebDriver.FindElement(By.Id("ConfirmRecipientEmail")).Clear();
				WebDriver.FindElement(By.Id("ConfirmRecipientEmail")).SendKeys(giftCardReceiverEmail);

				buttons = WebDriver.FindElements(By.TagName("button"));

				store.findAddToCartButton(buttons, WebDriver).Click();

				Assert.True(WebDriver.FindElement(By.ClassName("alert-success")) != null);
				x++;
			}
		}

		[Test]
		public void HreviewBasket()
		{
			IReadOnlyCollection<IWebElement> itemRows;
			IWebElement itemToIncrease;

			IWebDriver WebDriver = this.store.WebDriver;
			IWebElement miniCart = WebDriver.FindElement(By.Id("mini-cart"));
			miniCart.FindElement(By.TagName("a")).Click();

			//figure out which template is currently being used
			/*if(tryAssignElement(WebDriver, "cart-items") != null)
			{
				itemRows = WebDriver.FindElements(By.ClassName("row"));
				foreach(IWebElement row in itemRows)
				{
					if(Regex.IsMatch(row.Text, productToSearchFor))
					{
						//Console.WriteLine(row.Text);
					}
				}
			}*/
			
		}

		[Test]
		public void ICheckout()
		{
			//Dataset
			String contactNumber = "0836666666";
			String addressLine1 = "1 Broad Street";
			String city = "Cape Town";
			String suburb = "Deep River";
			String postalCode = "1234";
			String masterCardTestNumber ="5340831953675721";
			String cardHolderName = "Philips";
			String cardExpMonth = "1";
			String cardExpYear = "2019";
			String cardCVV = "123";

			IWebElement miniCart;
			IWebElement payUCreditCard;
			IReadOnlyCollection<IWebElement> links;
			IReadOnlyCollection<IWebElement> buttons;
			IReadOnlyCollection<IWebElement> paymentMethods;

			IWebDriver WebDriver = this.store.WebDriver;

			WebDriver.Navigate().GoToUrl(Url);
			miniCart = WebDriver.FindElement(By.Id("mini-cart"));
			miniCart.FindElement(By.TagName("a")).Click();

			Thread.Sleep(TimeSpan.FromSeconds(1));
			//links = WebDriver.FindElements(By.TagName("a"));
			//store.findLink(links, "[Cc][Hh][Ee][Cc][Kk][Oo][Uu][Tt]", WebDriver).Click();
			
			//This is a workaround because the Checkout button on the review view of the checkout process was not clickable via webdriver
			WebDriver.Navigate().GoToUrl(Url + "/checkout/details");

			WebDriver.FindElement(By.Id("new_billing_address")).Click();

			//Complete the new billing address form
			WebDriver.FindElement(By.Id("BillingAddress_Phone")).Clear();
			WebDriver.FindElement(By.Id("BillingAddress_Phone")).SendKeys(contactNumber);
			WebDriver.FindElement(By.Id("BillingAddress_Address1")).Clear();
			WebDriver.FindElement(By.Id("BillingAddress_Address1")).SendKeys(addressLine1);
			WebDriver.FindElement(By.Id("BillingAddress_City")).Clear();
			WebDriver.FindElement(By.Id("BillingAddress_City")).SendKeys(city);
			WebDriver.FindElement(By.Id("BillingAddress_Address3")).Clear();
			WebDriver.FindElement(By.Id("BillingAddress_Address3")).SendKeys(suburb);
			WebDriver.FindElement(By.Id("BillingAddress_Code")).Clear();
			WebDriver.FindElement(By.Id("BillingAddress_Code")).SendKeys(postalCode);
			new SelectElement(WebDriver.FindElement(By.Id("BillingAddress_CountryId"))).SelectByIndex(1);
			new SelectElement(WebDriver.FindElement(By.Id("BillingAddress_RegionId"))).SelectByIndex(1);
			
			if(store.tryAssignElement(WebDriver, "SpecialInstructions") != null)
			{
				//This applies to the checkout process of templates like KE and similar
				new SelectElement(WebDriver.FindElement(By.Id("SpecialInstructions"))).SelectByIndex(3);
				WebDriver.FindElement(By.Id("accept-terms")).Click();
				buttons = WebDriver.FindElements(By.TagName("button"));
				store.findButton(buttons, "[Pp][Ll][Aa][Cc][Ee]", WebDriver).Click();
				
				buttons = WebDriver.FindElements(By.TagName("button"));
				
				store.findButton(buttons, "[Cc][Oo][Nn][Ff][Ii][Rr][Mm]", WebDriver).Click();

				//Collect all of the payment methods and find the on that says credit card.
				paymentMethods = WebDriver.FindElements(By.Id("payment-method"));
				foreach(IWebElement paymentMethod in paymentMethods)
				{
					if(Regex.IsMatch(paymentMethod.Text, ".*[Cc][Rr][Ee][Dd][Ii][Tt].*"))
					{
						paymentMethod.FindElement(By.TagName("Input")).Click();
						break;
					}
				}
					
				buttons = WebDriver.FindElements(By.TagName("button"));
				store.findButton(buttons, "[Pp][Aa][Yy][Mm][Ee][Nn][Tt]", WebDriver).Click();

				//The following steps should be in the PayU UI
				payUCreditCard = WebDriver.FindElement(By.Id("panel_creditcard"));
				payUCreditCard.FindElement(By.TagName("input")).Click();
				WebDriver.FindElement(By.Id("card")).Clear();
				WebDriver.FindElement(By.Id("card")).SendKeys(masterCardTestNumber);
				WebDriver.FindElement(By.Id("ccholdername")).Clear();
				WebDriver.FindElement(By.Id("ccholdername")).SendKeys(cardHolderName);
				new SelectElement(WebDriver.FindElement(By.Id("expMonth"))).SelectByValue(cardExpMonth);
				new SelectElement(WebDriver.FindElement(By.Id("expYear"))).SelectByValue(cardExpYear);
				WebDriver.FindElement(By.Id("cvvnumber")).Clear();
				WebDriver.FindElement(By.Id("cvvnumber")).SendKeys(cardCVV);
				
				//This should be the final step in the payment process.
				buttons = WebDriver.FindElements(By.TagName("button"));
				store.findButton(buttons, "[Pp][Aa][Yy]", WebDriver).Click();

				//This test still requires an assertion regarding successful payment, at the time of this writing the 
				//payment process on the vanilla account is still not functional.
				
			}
			else
			{
				WebDriver.FindElement(By.Id("Delivery")).Click();
				buttons = WebDriver.FindElements(By.TagName("button"));
				store.findButton(buttons, "[Cc][Oo][Nn][Tt][Ii][Nn][Uu][Ee]", WebDriver).Click();
			}
			
			WebDriver.FindElement(By.Id("place-order-btn"));
			//This test still requires an assertion
		}
			

		/*[Test]
		public void JTeardown()
		{
			store.TeardownTest();
		}*/
		public static String randomUname()
		{
			Random random = new Random();
			String uname = random.Next(0x1000000).ToString();
			return uname;
		}
	}
}
