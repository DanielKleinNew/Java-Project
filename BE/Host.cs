using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BE
{
	[Serializable]
	public class Host
	{
		
		//ID number Of the host
		public int HostKey
		{
			get;
			set;
		}
		//First name
		public string HostPrivateName
		{
			get;
			set;
		}
		//Last name
		public string HostFamilyName
		{
			get;
			set;
		}
		//Phone number
		public int HostPhoneNumber
		{
			get;
			set;
		}
		//Email
		public string HostEmail
		{
			get;
			set;
		}
		//Bank branch details
		public BankBranch HostBankBranch
		{
			get;
			set;
		}
		public int HostBankAccount
		{
			get;
			set;
		}
		//OK to collect from the bank account
		public CollectionClearance HostCollectionClearance
		{
			get;
			set;
		}
	}
}
