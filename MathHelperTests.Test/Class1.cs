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
		[TestCase]
		public void AddTest()
		{
			ConsoleApplication1.MathsHelper helper = new ConsoleApplication1.MathsHelper();
			int result = helper.Add(20, 10);
			Assert.AreEqual(30, result);
		}

		[TestCase]
		public void SubtractTest()
		{
			ConsoleApplication1.MathsHelper helper = new ConsoleApplication1.MathsHelper();
			int result = helper.Subtract(20, 10);
			Assert.AreEqual(10, result);
		}
		[TestCase]
		public void LoginTests()
		{
			Store store = new Store();
			store.Url = "http://mystore.storefront.co.za/";

			store.SetupTest();
			store.loginFormValidation();
			store.SetupTest();
			store.successfulLogin("tester1.warp@gmail.com","hellopeter*1");
		}
    }
}
