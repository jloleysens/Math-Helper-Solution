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
		String Url;
		private String uname = randomUname();

		[Test]
		public void successfulLogin()
		{
			store.Url = Url;
			store.SetupTest();
			store.successfulLogin("tester1.warp@gmail.com", "hellopeter*1");
		}

		[Test]
		public void successfulLogOut()
		{
			store.Url = Url;
			store.SetupTest();
			store.successfulLogOut();
		}

		[Test]
		public void registrationFormValidation()
		{
			//var random = new Random();
			//var uname = random.Next(0x1000000);
			store.Url = Url;
			store.SetupTest();
			store.registrationFormValidation(uname.ToString(), uname.ToString(), uname.ToString() + "@gmail.com", uname.ToString());
		}

		[Test]
		public void forgotPasswordNonExistentEmail()
		{
			store.Url = Url;
			store.SetupTest();
			store.forgotPasswordNonExistentEmail();
		}

		[Test]
		public void registerUser()
		{
			store.Url = Url;
			store.SetupTest();
			store.registerUser(uname, uname, uname + "@gmail.com", uname);
		}


		[Test]
		public void addItemsToBasket()
		{
			store.Url = Url;
			store.SetupTest();
			store.addItemsToBasket();
		}

		[Test]
		public void addGiftCards()
		{
			store.Url = Url;
			store.SetupTest();
			store.addGiftCards();
		}

		[Test]
		public void reviewBasket()
		{
			store.Url = Url;
			store.SetupTest();
			store.reviewBasket();
		}

		[Test]
		public void Checkout()
		{
			store.Url = Url;
			store.SetupTest();
			store.Checkout();
		}

		[Test]
		public void Teardown()
		{
			store.TeardownTest();
		}
		public static String randomUname()
		{
			Random random = new Random();
			String uname = random.Next(0x1000000).ToString();

			return uname;
		}
	}
}
