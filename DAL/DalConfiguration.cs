using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.InteropServices.WindowsRuntime;

namespace DAL
{
	public class DalConfiguration
	{
		//Field: Hosting Unit Number (a unique 8 digit number)
		[XmlIgnore]
		public static int st_request_key =0;
		
		//ID number Of the host
		[XmlIgnore]
		public static int st_host_key = 0;
		
		//Hosting Unit Number - Unique ID (8 digit unique run code)
		[XmlIgnore]
		public static int st_unit_key = 0;
		
		//Order ID number
		[XmlIgnore]
		public static int st_order_key = 0;
		[XmlElement("Request")]
		public int request_serialize
		{
			get { return st_request_key; }
			set { st_request_key = value; }
		}
		
		[XmlElement("Host")]
		public int host_serialize
		{
			get { return st_host_key; }
			set { st_host_key = value; }
		}
		[XmlElement("Unit")]
		public int unit_serialize
		{
			get { return st_unit_key; }
			set { st_unit_key = value; }
		}
		[XmlElement("Order")]
		public int order_serialize
		{ 
			get { return st_order_key; }
			set { st_order_key = value; }
		}
	}
}
