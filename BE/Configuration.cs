using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
//to add
using System.Xml.Serialization;
using System.IO;

namespace BE
{
	//Configuration to include all global variables as static fields
	[Serializable]
	public class Configuration
	{

		//Fix fee for closing a reservation * number of days
		[XmlIgnore]
		public static double st_commission;
		[XmlIgnore]
		//Number of days to expire orders
		public static int st_expire_days;
		[XmlElement("Commission")]
		public double st_commission_serialize
		{
			get { return st_commission; }
			set { st_commission = value; }
		}
		[XmlElement("Date")]
		public int st_expire_days_serialize
		{
			get { return st_expire_days; }
			set { st_expire_days = value; }
		}
	}
}
