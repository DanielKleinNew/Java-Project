using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BE
{
	[Serializable]
	public class Calendar
	{
		public Dictionary<int, bool[,]> Matrix
		{
			get;
			set;
		}
		
		public Calendar()
		{
			Matrix = new Dictionary<int, bool[,]>();
		}

	}

}
