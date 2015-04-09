using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;

namespace MathHelperTests.Test
{
	//[TestFixture]
	partial class Store : TestBase
	{
		//Dataset
		String productToSearchFor = "Product1Test";//The assumption here is that the product has been created in the store by way of the API
		//Double totalBasketAmount = 0;
		String giftCardSenderName = "Peter";
		String giftCardReceiverEmail = "tester1.warp@gmail.com";
		//String giftCardNegativeNumberError = ".*needs to exceed.*";
		//String giftCardAddedMessage = ".*has been added.*";
		String[] giftCardAmount = { "500", "350", "450" };

		/// <summary>
		/// This first test must be updated once the API is up and running...
		/// Should: 
		/// 1) Search for specifically created products adding only them to the basket for testing purposes.
		/// 2) Make use of search bar to find products, categories and any other means of finding a particular product available on the site.
		/// 3) End with 5 items in the basket for checkout
		/// </summary>
		//[TestCase]
		public void addItemsToBasket()
		{
			try
			{
				WebDriverWait wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(2));
				//Add a product and its variant (find product via search functionality)

				//Homepage
				WebDriver.Navigate().GoToUrl(Url);

				IWebElement searchForm = tryAssignElement(WebDriver, "search-form");
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
				findLink(links, productToSearchFor, WebDriver).Click();

				//declare the variables that are to be used in the coming for loops.
				Int32 segmentNumber;
				SelectElement variantElement;
				//Add the first product (2 variants)
				for (int x = 1; x < 3; x++)
				{
					segmentNumber = 0;
					variantElement = tryAssignSelectElement(WebDriver, "Segment_" + segmentNumber.ToString());
					
					//If there are variants to be selected then do the following
					while (variantElement != null)
					{
						variantElement.SelectByIndex(x);
						segmentNumber += 1;
						variantElement = tryAssignSelectElement(WebDriver, "Segment_" + segmentNumber.ToString());
					}
					//Checking whether particular product available
					Thread.Sleep(TimeSpan.FromSeconds(2));

					links = WebDriver.FindElements(By.TagName("a"));
					findLink(links,"[Aa][Dd][Dd].*[Cc][Aa][Rr][Tt]", WebDriver).Click();

					//Wait for the basket amount to be updated
					Thread.Sleep(TimeSpan.FromSeconds(1));
					Assert.True(tryAssignElement(WebDriver, "added-to-cart") != null);

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

					IWebElement featuredProducts = tryAssignElement(WebDriver, "featured-products");
					if (featuredProducts == null)
					{
						featuredProducts = tryAssignElement(WebDriver, "featured");
					}
					//All of the links in the featured products section, here assumed to be the products themselves.
					IReadOnlyCollection<IWebElement> featuredProductsLinks = featuredProducts.FindElements(By.TagName("a"));
					//Click on the first link (any product)
					foreach(IWebElement link in featuredProductsLinks)
					{
						if(link.Displayed && link.Enabled && link.FindElement(By.TagName("img")) != null)
						{
							link.Click();
							break;
						}
					}

					//add the item
					segmentNumber = 0;
					variantElement = tryAssignSelectElement(WebDriver, "Segment_" + segmentNumber.ToString());

					//If there are variants to be selected then do the following
					while (variantElement != null)
					{
						variantElement.SelectByIndex(1);
						segmentNumber += 1;
						variantElement = tryAssignSelectElement(WebDriver, "Segment_" + segmentNumber.ToString());
					}
					//Checking whether particular product available
					Thread.Sleep(TimeSpan.FromSeconds(2));

					links = WebDriver.FindElements(By.TagName("a"));
					findLink(links, "[Aa][Dd][Dd].*[Cc][Aa][Rr][Tt]", WebDriver).Click();

					//Wait for the basket amount to be updated
					Thread.Sleep(TimeSpan.FromSeconds(1));
					Assert.True(tryAssignElement(WebDriver, "added-to-cart") != null);

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
				findLink(links, productToSearchFor, WebDriver).Click();

				segmentNumber = 0;
				variantElement = tryAssignSelectElement(WebDriver, "Segment_" + segmentNumber.ToString());

				//If there are variants to be selected then do the following
				while (variantElement != null)
				{
					variantElement.SelectByIndex(1);
					segmentNumber += 1;
					variantElement = tryAssignSelectElement(WebDriver, "Segment_" + segmentNumber.ToString());
				}
				//Checking item availability and price
				Thread.Sleep(TimeSpan.FromSeconds(2));

				links = WebDriver.FindElements(By.TagName("a"));
				findLink(links, "[Aa][Dd][Dd].*[Cc][Aa][Rr][Tt]", WebDriver).Click();

				//Wait for the basket amount to be updated
				Thread.Sleep(TimeSpan.FromSeconds(1));
				Assert.True(tryAssignElement(WebDriver, "added-to-cart") != null);

				//Keep track of the total basket amount
				//Console.WriteLine(WebDriver.FindElement(By.Id("product-price")).Text);
				//totalBasketAmount += Convert.ToDouble(productPriceText.Substring(1), CultureInfo.InvariantCulture);
				//totalBasketAmount += parseQuantity(WebDriver.FindElement(By.Id("product-price")).Text);
			

			}
			catch (Exception e)
			{
				assertionErrors.Append(e.Message);
			}
			Console.WriteLine("\nAdd Item To Basket Test: " + hasErrors(assertionErrors));
		}
		//[TestCase]
		///<summary>
		///The following test will add 3 gift cards, with different images, and test general form validation
		///and exception handling. If, however, the store that is being tested does not yet have gift cards that
		///can be added the test will essentially be skipped.
		///</summary>
		public void addGiftCards()
		{
			try
			{
				//Declare the variables that will be used in the test
				Int32 x = 0;
				IReadOnlyCollection<IWebElement> giftCardImages;
				IReadOnlyCollection<IWebElement> links;
				IReadOnlyCollection<IWebElement> buttons;

				WebDriver.Navigate().GoToUrl(Url);


				links = WebDriver.FindElements(By.TagName("a"));
				//Find the gift cards link on the homepage
				findLink(links, "[Gg][Ii][Ff][Tt]", WebDriver).Click();

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

					findAddToCartButton(buttons, WebDriver).Click();

					Assert.True(WebDriver.FindElement(By.ClassName("alert-success")) != null);
					x++;
				}
			}
			catch (Exception e)
			{
				assertionErrors.Append(e.Message);
			}
			finally
			{
				Console.WriteLine("\nCreate Gift Cards Test: " + hasErrors(assertionErrors));
			}
		}
		//[TestCase]
		public void reviewBasket()
		{
			///<summary>
			///This tests the ability to make changes to quantities of items
			///removing items as well as testing general form validation and exception handling
			///</summary>
			///
			try
			{
				//variables for the test
				IReadOnlyCollection<IWebElement> itemRows;
				IWebElement itemToIncrease;

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
			catch (Exception e)
			{
				assertionErrors.Append(e.Message);
			}
			Console.WriteLine("\nReview Basket Test: " + hasErrors(assertionErrors));
		}
		/// <summary>
		/// This method takes a string that is expected to be in some form, for example: Stock: 45.00 or 8.00
		/// and parses it into a double that represents the stock number
		/// </summary>
		/// <param name="strToParse">This is the string that will be parsed into a Double</param>
		/// <returns>Returns the Double value of the expected string</returns>
		public Double parseQuantity(String strToParse)
		{
			StringBuilder substrToParse = new StringBuilder("");
			Int32 num;

			for (Int32 x = 0; x < strToParse.Length; x++)
			{
				if (strToParse[x] == '.')
				{
					substrToParse.Append('.');
				}
				else if (Int32.TryParse(Convert.ToString(strToParse[x]), out num))
				{
					substrToParse.Append(num);
				}
			}
			//Console.WriteLine(substrToParse);
			String stringToConvert = substrToParse.ToString();
			Double result = Convert.ToDouble(stringToConvert, CultureInfo.InvariantCulture);

			return result;
		}
		public IWebElement findAddToCartButton(IReadOnlyCollection<IWebElement> buttons, IWebDriver driver)
		{
			foreach(IWebElement button in buttons)
			{
				if(Regex.IsMatch(button.Text, "[Aa][Dd][Dd] [Tt][Oo] [Cc][Aa][Rr][Tt]"))
				{
					return button;
				}
			}
			return null;
		}
	}
}

