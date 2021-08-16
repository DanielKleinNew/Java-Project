using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
namespace BE
{
	[Serializable]
	public class BankBranch
	{
		//Bank Number
		public int BankNumber
		{
			get;
			set;
		}
		//Bank Name
		public string BankName
		{
			get;
			set;
		}
		//Branch number
		public int BranchNumber
		{
			get;
			set;
		}
		//Branch Address
		public string BranchAddress
		{
			get;
			set;
		}
		//Branch City
		public string BranchCity
		{
			get;
			set;
		}
		//Print Method
	}	
}
