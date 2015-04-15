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
		String Url = "http://mystore.storefront.co.za";
		private String uname = randomUname();

		[Test]
		public void AsuccessfulLogin()
		{
			store.SetupTest();
			Assert.True(store.successfulLogin("tester1.warp@gmail.com", "hellopeter*1"));
		}

		[Test]
		public void BsuccessfulLogOut()
		{
			store.SetupTest();
			Assert.True(store.successfulLogOut());
		}

		[Test]
		public void CregistrationFormValidation()
		{
			//var random = new Random();
			//var uname = random.Next(0x1000000);
			store.SetupTest();
			Assert.True(store.registrationFormValidation(uname.ToString(), uname.ToString(), uname.ToString() + "@gmail.com", uname.ToString()));
		}

		[Test]
		public void DforgotPasswordNonExistentEmail()
		{
			store.SetupTest();
			Assert.True(store.forgotPasswordNonExistentEmail());
		}

		[Test]
		public void EregisterUser()
		{
			store.SetupTest();
			Assert.True(store.registerUser(uname, uname, uname + "@gmail.com", uname));
		}


		[Test]
		public void FaddItemsToBasket()
		{
			store.SetupTest();
			Assert.True(store.addItemsToBasket());
		}

		[Test]
		public void GaddGiftCards()
		{
			store.SetupTest();
			Assert.True(store.addGiftCards());
		}

		[Test]
		public void HreviewBasket()
		{
			store.SetupTest();
			Assert.True(store.reviewBasket());
		}

		[Test]
		public void ICheckout()
		{
			store.SetupTest();
			Assert.True(store.Checkout());
		}

		[Test]
		public void JTeardown()
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
