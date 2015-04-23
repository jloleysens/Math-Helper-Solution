using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MathHelperTests.Test
{
	class Portal : TestBase
	{
		public String randomStoreNameGenerator()
		{
			String path = Path.GetRandomFileName();
			path = path.Replace(".", "");
			return path;
		}
	}
}
