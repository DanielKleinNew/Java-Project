using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BE;

namespace DS
{
	public static class DataSource
	{
		public static List<Host> host_list = new List<Host>();
		public static List<HostingUnit> hosting_unit_list = new List<HostingUnit>();
		public static List<Order> order_list = new List<Order>();
		public static List<BankBranch> bank_branch_list = new List<BankBranch>();
		public static List<GuestRequest> guest_request_list = new List<GuestRequest>();

	}
}
