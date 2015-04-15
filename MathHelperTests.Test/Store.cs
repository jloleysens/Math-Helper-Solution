using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MathHelperTests.Test
{
	class Store : TestBase
	{
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
		public IWebElement findForgotPasswordLink(IReadOnlyCollection<IWebElement> links, IWebDriver driver)
		{
			foreach(IWebElement link in links)
			{
				if(Regex.IsMatch(link.Text, ".*[Ff][Oo][Rr][Gg][Oo][Tt].*"))
				{
					return link;
				}
				if(Regex.IsMatch(link.Text, ".*[Rr][Ee][Ss][Ee][Tt].*"))
				{
					return link;
				}
			}
			return null;
		}
		public IWebElement findLoginButton(IReadOnlyCollection<IWebElement> buttons, IWebDriver driver)
		{
			foreach (IWebElement button in buttons)
			{
				if (Regex.IsMatch(button.Text, "[Ll][Oo][Gg][Ii][Nn]"))
				{
					return button;
				}
				if (Regex.IsMatch(button.Text, ".*[Ss][Ii][Gg][Nn].*"))
				{
					return button;
				}
			}
			return null;
		}
	}
}
