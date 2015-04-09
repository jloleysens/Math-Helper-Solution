using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Firefox;

namespace MathHelperTests.Test
{
    public class TestClass
    {
		[TestCase, Category("Unit")]
		public void AddTest()
		{
			ConsoleApplication1.MathsHelper helper = new ConsoleApplication1.MathsHelper();
			int result = helper.Add(20, 10);
			Assert.AreEqual(30, result);
		}

		[TestCase, Category("Unit")]
		public void SubtractTest()
		{
			ConsoleApplication1.MathsHelper helper = new ConsoleApplication1.MathsHelper();
			int result = helper.Subtract(20, 10);
			Assert.AreEqual(10, result);
		}
		[TestCase, Category("UI")]
		public void StoreTests()
		{

			Store store = new Store();
			store.Url = "http://mystore.storefront.co.za/";

			store.SetupTest();
			store.loginFormValidation();
			store.SetupTest();
			store.successfulLogin("tester1.warp@gmail.com","hellopeter*1");
			store.SetupTest();
			store.successfulLogOut();

			//This will be used to generate a random hexadecimal 
			var random = new Random();
			var uname = random.Next(0x1000000);

			store.SetupTest();
			store.registrationFormValidation(uname.ToString(), uname.ToString(), uname.ToString() + "@gmail.com", uname.ToString());
			store.SetupTest();
			store.registerUser(uname.ToString(), uname.ToString(), uname.ToString() + "@gmail.com", uname.ToString());
			store.TeardownTest();

			//The forgotten password and reset password tests still have to come in here
			store.SetupTest();
			store.forgotPasswordNonExistentEmail();

			store.SetupTest();
			store.addItemsToBasket();
			store.SetupTest();
			store.addGiftCards();
			store.SetupTest();
			store.reviewBasket();
			store.SetupTest();
			store.Checkout();

			store.TeardownTest();
		}
    }
}
