using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
	public class BL_Factory
	{
		//Using the factory method to return a single instance of the BL to the UI
		public static IBL GetBL()
		{
			return BL_imp.Instance;
		}
	}
}
