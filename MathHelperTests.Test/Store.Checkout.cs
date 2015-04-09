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

		/// <summary>
		/// The following test will attempt to check out using all possible methods; collection, delivery, shipping, free shipping with additional costs
		/// quoted shipping with products flagged as "free shipping". Finally it will attempt to ship products where shipping has been
		/// marked as false (i.e. AllowShipping = false)
		/// </summary>
		//[TestCase]
		public void Checkout()
		{
			try
			{
				IWebElement miniCart;
				IWebElement payUCreditCard;
				IReadOnlyCollection<IWebElement> links;
				IReadOnlyCollection<IWebElement> buttons;
				IReadOnlyCollection<IWebElement> paymentMethods;

				WebDriver.Navigate().GoToUrl(Url);
				miniCart = WebDriver.FindElement(By.Id("mini-cart"));
				miniCart.FindElement(By.TagName("a")).Click();

				links = WebDriver.FindElements(By.TagName("a"));
				findLink(links, "[Cc][Hh][Ee][Cc][Kk][Oo][Uu][Tt]", WebDriver).Click();

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

				if(tryAssignElement(WebDriver, "SpecialInstructions") != null)
				{
					//This applies to the checkout process of templates like KE and similar
					new SelectElement(WebDriver.FindElement(By.Id("SpecialInstructions"))).SelectByIndex(3);
					WebDriver.FindElement(By.Id("accept-terms")).Click();
					buttons = WebDriver.FindElements(By.TagName("button"));
					findButton(buttons, "[Pp][Ll][Aa][Cc][Ee]", WebDriver).Click();

					buttons = WebDriver.FindElements(By.TagName("button"));
					findButton(buttons, "[Cc][Oo][Nn][Ff][Ii][Rr][Mm]", WebDriver).Click();

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
					findButton(buttons, "[Pp][Aa][Yy][Mm][Ee][Nn][Tt]", WebDriver).Click();

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
					findButton(buttons, "[Pp][Aa][Yy]", WebDriver).Click();

					//This test still requires an assertion regarding successful payment, at the time of this writing the 
					//payment process on the vanilla account is still not functional.


				}
				else
				{
					WebDriver.FindElement(By.Id("Delivery")).Click();
					buttons = WebDriver.FindElements(By.TagName("button"));
					findButton(buttons, "[Cc][Oo][Nn][Tt][Ii][Nn][Uu][Ee]", WebDriver).Click();
				}

				WebDriver.FindElement(By.Id("place-order-btn"));
			}
			catch(Exception e)
			{
				assertionErrors.Append(e.Message);
			}
			Console.WriteLine("\nCheckout Test: " + hasErrors(assertionErrors));
		}
	}
}
