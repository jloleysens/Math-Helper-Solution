using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

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
    }
}
