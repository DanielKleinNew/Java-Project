using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
	public class DAL_Factory
	{
		//using the factory method to return a single instance of the DAL to the BL
		public static IDAL GetDAL()
		{
			return Dal_XML_imp.Instance;
		}
		
	}
}
