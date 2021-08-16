using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Mail;
using DAL;
using BE;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.ComponentModel;

namespace BL
{
	public interface IBL
	{
		//Create
		//add guest request
		//The holiday start date is at least one day earlier than the holiday ending date
		void AddCustomer(GuestRequest request);
		//Add a hosting unit
		void AddHostingUnit(HostingUnit unit);
		//check if days are available 
		//When creating a customer order, make sure that the requested dates are available in the hosting unit offered to them.
		void AddOrder(Order order);
		//add host
		void AddHost(Host host);

		//Read
		//list of all hosting unit
		List<HostingUnit> GetListUnits();
		//list of hosting unit of owner
		List<HostingUnit> GetHostListUnits(int id);
		//Receive a list of all customer requests .
		List<GuestRequest> GetListCustomers();
		//Receive a list of all customer requests .
		List<GuestRequest> GetListOpenCustomers();
		//Receive a list of all orders
		List<Order> GetListOrders();
		//Receives a list of all existing bank branches in Israel
		List<BankBranch> GetListBanks();
		//get list of host
		List<Host> GetListHost();
		//get the configuration 
		Configuration GetConfigurations();
		//get data of a host
		Host GetHostData(string email, int password);
		//get hosting unit data
		HostingUnit GetUnitData(int id);
		//get guest request info
		GuestRequest GetRequestData(int id);
		//get host orders
		List<Order> GetHostOrders(int id);
		//get unit of order
		HostingUnit GetUnitOfOrder(Order data, int id);
		//get host 
		Host GetHost(Order data);

		//update
		//Customer request update  (status change) .
		void CustomerRequestUpdate(GuestRequest request);
		//Hosting unit update
		void UpdateHostingUnit(HostingUnit unit);
		//Order Update (Status Change)
		void OrderUpdate(Order order);
		//update host
		void HostUpdate(Host host);
		//change configuration data 
		//
		void ConfigurationUpdate(double com, int day);

		//Delete 
		//delete hosting unit
		//You cannot delete a hosting unit as long as there is an offer associated with it in open mode .
		bool DeleteHostingUnit(HostingUnit unit);
		//delete host
		bool DeleteHost(Host host);
		//delete order
		bool DeleteOrder(Order order);
		//delete request
		bool DeleteRequest(GuestRequest request);


		//misc
		//check if host exist
		bool LoginHost(string email, int password);
		//check if bank branch is legal
		bool CheckBankInfo(BankBranch branch);
		//check for occupancy
		bool CheckForOccupancy(HostingUnit unit);
		
		

		
		
		//The owner of a hosting unit will be able to send a customer order (changing the status to "Sent Mail"), only if he have signed a bank account debit authorization.
		bool CheckForStatus(int id);
		//Once the order status has changed to close a transaction , its status can no longer be changed.
		bool CheckOrderStatus(Order data);
		//When the order status changes to "Sent Mail" - the system will automatically send a mail to the customer with the order details.
		void SendGuestEmail(HostingUnit unit, GuestRequest guest);
		//When booking status changes due to closing of transaction - a fee of NIS 10 per accommodation day must be calculated. (See note below)
		void ChargeHost(Order data);
		//When order status changes due to transaction closing - the relevant dates must be marked in the matrix.
		void MarkCaledar(Order data);
		//When an order status changes due to a transaction closing - the customer order status must be changed accordingly, as well as the status of all other customer orders.
		void ChangeOrderStatus(Order data);
		//Account billing authorization cannot be revoked when there is an offer associated with it in open mode.
		bool CheckBillingStatus(Host data);
		//•	A function that accepts a date and number of vacation days and returns the list of all available accommodation units on that date.
		List<HostingUnit> GetUnitDate(DateTime date, int days);
		//•	A function that accepts one or two dates. The function returns the number of days that have passed from the first date to the second, or if only one date has been received - from the first date to the present day.
		int CheckNumberDay(DateTime first, DateTime second = default);

		

		//combines unit and request into a hosting unit into a order
		void MakeOrder(HostingUnit unit, GuestRequest guest);
		//check for if day are available on calendar
		bool CheckCalendarDay(HostingUnit unit, DateTime first, DateTime second);
		//add new calendar year
		//Calendar AddCalendarYear(Calendar cal, int year);
		//check valid emails
		bool CheckValidEmail(string email);
		//check if demands are met
		bool CheckDemands(HostingUnit unit, GuestRequest guest);
		//check if other person close an order
		bool CheckOrderForClose(GuestRequest guest);
		//check if there is unclosed order with unit
		bool CheckForOrder(HostingUnit unit);
		//change all status of other orders
		void ChangeAllStatus(Order data);
		//send host info and password
		void SendHostPassword(Host host);

		//•	A function that can return all customer requirements that fit a particular condition 
		//(meaning that the function accepts a delegate that corresponds to a method that works on customer demand and returns a bool and that's how the condition is defined)
		List<GuestRequest> FilterGuestRequests(Func<GuestRequest, bool> filter);

		//•	A function that accepts several days, and returns all orders that have elapsed since they were created / since the email was sent to a customer greater than or equal to the number of days the function received.
		List<Order> OrderStat(int days);
		//•	A function that accepts customer demand and returns the number of orders sent to it him.
		int RequestStat(GuestRequest guestRequest);
		//•	Function takes accommodation unit and returns the number of orders that were sent  / number of closed order through the site.
		int UnitStatOrders(HostingUnit hostingUnit);
		int UnitStatClosed(HostingUnit hostingUnit);
		//•	Clustered Customer Requirements List (Grouping) According to the required area .
		IEnumerable<IGrouping<Areas, GuestRequest>> GroupRequestsByArea();
		//•	Clustered Customer Requirements List (Grouping) According to the number of vacationers.
		IEnumerable<IGrouping<int, GuestRequest>> GroupRequestByAccupants();
		//•	Grouped host list (Grouping) According to the number of accommodation units they hold
		IEnumerable<IGrouping<int, HostingUnit>> GroupUnitByAccupants();
		//•	List of hosted units (Grouping) According to the required area.
		IEnumerable<IGrouping< Areas, HostingUnit>> GroupUnitByArea();
		//get commission calculations 
		double GetCommission(int id);
		//•	A function that accepts several days, and returns all orders that have elapsed since they were created / since the email was sent to a customer greater than or equal to the number of days the function received.
		List<Order> OrderFunction(int day);

		//thread related 
		void StartThreads();
		//get bank info
		void BankworkerAction(object sender, DoWorkEventArgs e);
		//update the cache
		void StartUp(object sender, DoWorkEventArgs e);
		//update old order and request
		void UpdateExpire();
	}	
}
