using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Net.Mail;
using BE;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Data;
using System.Threading;
using System.Data.SqlTypes;
using System.Globalization;

namespace BL
{
	class BL_imp : IBL
	{

		DAL.IDAL dal = DAL.DAL_Factory.GetDAL();
		//One instance of the Dal_imp
		private static BL_imp instance;
		private BL_imp()
		{
			//t++
		}
		
		//Return one instance of the DAL
		public static BL_imp Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new BL_imp();
				}
				return instance;
			}
		}
		
		//Create
		//Add a customer request 
		public void AddCustomer(GuestRequest request)
		{

			//Checks if day and month are valid
			if (request.RequestEntryDate.Month < 1 || request.RequestEntryDate.Month > 12)
			{
				throw new ApplicationException("ERROR: Months are represented as numbers 1-12 chronologically.");
			}
			
			if (request.RequestEntryDate.Day < 1 || request.RequestEntryDate.Day > DateTime.DaysInMonth(request.RequestEntryDate.Year, request.RequestEntryDate.Month))
			{
				throw new ApplicationException("ERROR: Not a valid day of this month.");
			}
			
			//Checks if day and month are valid
			if (request.RequestReleaseDate.Month < 1 || request.RequestReleaseDate.Month > 12)
			{
				throw new ApplicationException("ERROR: Months are represented as numbers 1-12 chronologically.");
			}
			
			//Check if the first month is greater than the second month
			if (request.RequestEntryDate.Month > request.RequestReleaseDate.Month)
			{
				throw new ApplicationException("ERROR: Ending month must be the same or later than the starting month.");
			}
			
			//Check for correct amount of days in a month
			if (request.RequestReleaseDate.Day < 1 || request.RequestReleaseDate.Day > DateTime.DaysInMonth(request.RequestReleaseDate.Year, request.RequestReleaseDate.Month))
			{
				throw new ApplicationException("ERROR: Not a valid day for this query.");
			}
			
			//Guest must stay at least one night
			if ((request.RequestEntryDate.Day == request.RequestReleaseDate.Day) && (request.RequestEntryDate.Month == request.RequestReleaseDate.Month))
			{
				throw new ApplicationException("ERROR: Gust must stay at least one night.");
			}

			try
			{
				GuestRequest newRuquest = null;
				newRuquest = request.Copy();
				dal.AddCustomer(newRuquest);
			}
			catch (Exception)
			{
				throw new Exception("exception");
			}
		}
		
		//Add a hosting unit
		public void AddHostingUnit(HostingUnit unit)
		{
			try
			{
				HostingUnit newUnit = null;
				newUnit = unit.Copy();
				dal.AddHostingUnit(newUnit);
			}
			catch (Exception)
			{

				throw new Exception("exception");
			}
		}
		
		//Add an order
		public void AddOrder(Order order)
		{
			try
			{
				Order newOrder = null;
				newOrder = order.Copy();
				dal.AddOrder(newOrder);
			}
			catch (Exception)
			{
				throw new Exception("exception");
			}
		}
		
		//Add host
		public void AddHost(Host host)
		{
			try
			{
				Host newHost = null;
				newHost = host.Copy();
				dal.AddHost(newHost);
				//Send email with host info and password
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				//Throw new Exception("exception");
			}
		}
		
		//Send host info and password
		public void SendHostPassword(Host host)
		{
			//Send host all his info and password
		}

		//Read
		public List<HostingUnit> GetListUnits()
		{
			return dal.GetListUnits();
		}
		
		//List of hosting unit of owner
		public List<HostingUnit> GetHostListUnits(int id)
		{
			List<HostingUnit> data = dal.GetListUnits();
			return data.Where(item => item.UnitOwner.HostKey == id).ToList();
		}

		//Receive a list of all customer requests .
		public List<GuestRequest> GetListCustomers()
		{
			return dal.GetListCustomers();
		}
		
		//Receive a list of all customer requests .
		public List<GuestRequest> GetListOpenCustomers()
		{
			List<GuestRequest> guestList = dal.GetListCustomers();
			List<GuestRequest> returnList = dal.GetListCustomers();
			List<Order> orderList = dal.GetListOrders();
			if (orderList.Count == 0)
			{
				return guestList;
			}
			
			//Add request that have orders but are not closed
			foreach (var item in guestList)
			{
				foreach (var thing in orderList)
				{
					if (returnList.Contains(item))
					{
						//Remove item that has a closed order
						if(item.GuestRequestKey == thing.OrderRequestKey && (thing.OrderStatus == Statuses.ClosedForCustomerResponse || thing.OrderStatus == Statuses.ClosedForCustomerUnresponsiveness))
						{
							returnList.Remove(item);
						}
					}
				}
			}
			return returnList;
		}
		
		//Receive a list of all orders
		public List<Order> GetListOrders()
		{
			return dal.GetListOrders();
		}
		
		//Receives a list of all existing bank branches in Israel
		public List<BankBranch> GetListBanks()
		{
			return dal.GetListBanks();
		}
		
		//Get list of host
		public List<Host> GetListHost()
		{
			return dal.GetListHost();
		}
		
		//Get the configuration 
		public Configuration GetConfigurations()
		{
			return dal.GetConfigurations();
		}
		
		//Get host data
		public Host GetHostData(string email, int password)
		{
			List<Host> host_list = new List<Host>();
			host_list = dal.GetListHost();
			//find the obj if it exist if not return null
			var matchQuery = (from item in host_list
						where ((item.HostEmail == email) && (item.HostKey == password))
						select item).FirstOrDefault();
			
			//Return true if info matches
			if (matchQuery != null)
			{
				return matchQuery;
			}
			return null;
		}
		
		//Get host order data
		public List<Order> GetHostOrders(int unitId)
		{
			List<Order> orderList = dal.GetListOrders();
			List<HostingUnit> unitList = dal.GetListUnits();
			var matchQuery = (from item in unitList
						where item.UnitOwner.HostKey == unitId
						select item).DefaultIfEmpty();
			return orderList.Where(p1 => matchQuery.Any(p2 => p2.HostingUnitKey == p1.OrderUnitKey)).ToList();
		}
		
		//Get host 
		public Host GetHost(Order data)
		{
			List<Host> hostList = dal.GetListHost();
			List<HostingUnit> unitList = dal.GetListUnits();
			var unitQuery = (from item in unitList
						where item.HostingUnitKey == data.OrderUnitKey
						select item).FirstOrDefault();
			if (unitQuery == null)
			{
				return null;
			}
			var matchQuery = (from item in hostList
							  where item.HostKey == unitQuery.UnitOwner.HostKey
							  select item).FirstOrDefault();

			//Return true if info matches
			if (matchQuery != null)
			{
				return matchQuery;
			}
			return null;
		}

		//Update
		//Customer request update  (status change) .
		public void CustomerRequestUpdate(GuestRequest request)
		{
			try
			{
				GuestRequest newRuquest = null;
				newRuquest = request.Copy();
				dal.AddCustomer(newRuquest);
			}
			catch (Exception)
			{
				throw new Exception("exception");
			}
		}
		
		//Hosting unit update
		public void UpdateHostingUnit(HostingUnit unit)
		{
			try
			{
				HostingUnit newUnit = null;
				newUnit = unit.Copy();
				dal.UpdateHostingUnit(newUnit);
			}
			catch (Exception)
			{

				throw new Exception("exception");
			}
		}
		
		//Order Update (Status Change)
		public void OrderUpdate(Order order)
		{
			try
			{
				Order newOrder = null;
				newOrder = order.Copy();
				dal.AddOrder(newOrder);
			}
			catch (Exception)
			{
				throw new Exception("exception");
			}
		}
		
		//Update host
		public void HostUpdate(Host host)
		{
			try
			{
				Host newHost = null;
				newHost = host.Copy();
				dal.HostUpdate(newHost);
			}
			catch (Exception)
			{
				throw new Exception("exception");
			}
		}
		
		//Update the configuration 
		public void ConfigurationUpdate(double com, int day)
		{
			try
			{
				Configuration.st_commission = com;
				Configuration.st_expire_days = day;
				Configuration newConfig = new Configuration();
				dal.ConfigurationUpdate(newConfig);
			}
			catch (Exception)
			{
				throw new Exception("exception");
			}
		}

		//Delete 
		//Delete a hosting unit
		public bool DeleteHostingUnit(HostingUnit unit)
		{
			//Delete only if there are no reservations 
			if (CheckForOccupancy(unit))
			{
				try
				{
					dal.DeleteHostingUnit(unit.HostingUnitKey);
				}
				catch (Exception)
				{
					throw;
				}
				return true;
			}
			
			else
			{
				return false;
			}
		}
		
		//Delete host
		public bool DeleteHost(Host host)
		{
			//First try deleting the hosting units then delete the host
			List<HostingUnit> hostList = dal.GetListUnits();
			foreach (HostingUnit item in hostList.FindAll(e => e.UnitOwner.HostKey == host.HostKey))
			{
				if (CheckForOccupancy(item) == false)
				{
					return false;
				}
			}
			
			//Delete all the hosting unit then delete the host
			foreach (HostingUnit item in hostList.FindAll(e => e.UnitOwner.HostKey == host.HostKey))
			{
				try
				{
					dal.DeleteHostingUnit(item.HostingUnitKey);
				}
				catch (Exception)
				{
					throw;
				}
			}
			try
			{
				dal.DeleteHost(host.HostKey);
			}
			catch (Exception)
			{
				throw;
			}

			return true;
		}
		
		//Delete order
		public bool DeleteOrder(Order order)
		{
			//Only delete the order after the date 
			List<GuestRequest> requestList = dal.GetListCustomers();
			foreach (GuestRequest item in requestList.FindAll(e => e.GuestRequestKey == order.OrderRequestKey))
			{
				if (DeleteRequest(item) == false)
				{
					return false;
				}
			}
			
			//Delete all the hosting unit then delete the host
			foreach (GuestRequest item in requestList.FindAll(e => e.GuestRequestKey == order.OrderRequestKey))
			{
				try
				{
					dal.DeleteRequest(item.GuestRequestKey);
				}
				catch (Exception)
				{
					throw;
				}
			}
			try
			{
				dal.DeleteOrder(order.OrderKey);
			}
			catch (Exception)
			{
				throw;
			}

			return true;
		}
		
		//Delete request
		public bool DeleteRequest(GuestRequest request)
		{
			//Delete after the dates have past 
			if (request.RequestReleaseDate < DateTime.Now)
			{
				try
				{
					dal.DeleteRequest(request.GuestRequestKey);
				}
				catch (Exception)
				{
					throw;
				}

				return true;
			}
			return false;
		}

		//Check if host exist
		public bool LoginHost(string email, int password)
		{
			List<Host> host_list = new List<Host>();
			host_list = dal.GetListHost();
			//Find the obj if it exist if not return null
			var matchQuery = (from item in host_list
						where ((item.HostEmail == email) && (item.HostKey == password))
						select item).FirstOrDefault();
			//Return true if info matches
			if (matchQuery != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		//Check if this is a valid bank branch
		public bool CheckBankInfo(BankBranch branch)
		{
			List<BankBranch> bank_branch_list = new List<BankBranch>();
			bank_branch_list = dal.GetListBanks();
			//Find the obj if it exist if not return null
			var matchQuery = (from item in bank_branch_list
							  where ((item.BankNumber == branch.BankNumber) && (item.BranchNumber == branch.BranchNumber))
							  select item).FirstOrDefault();
			//Return true if info matches
			if (matchQuery != null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		//Check for occupancy
		public bool CheckForOccupancy(HostingUnit unit)
		{	
			if (CheckThisYear(unit.UnitDiary) || CheckForOrder(unit))
			{
				//Don't delete calendar
				return false;
			}
			
			//Delete calendar
			return true;
		}
		
		//Check if there is unclosed order with unit
		public bool CheckForOrder(HostingUnit unit)
		{
			List<Order> orderList = new List<Order>();
			orderList = dal.GetListOrders();
			foreach (var item in orderList)
			{
				if(item.OrderUnitKey == unit.HostingUnitKey)
				{
					if(item.OrderStatus == Statuses.SentEmail)
					{
						return true;
					}			
				}
			}
			return false;
		}

		//Check if this year has any occupancies 
		public bool CheckThisYear(bool[,] calendar)
		{
			DateTime date = DateTime.Today;
			int daysInYear = DateTime.IsLeapYear(date.Year) ? 366 : 365;
			int daysLeftInYear = daysInYear - date.DayOfYear; // Result is in range 0-365.
			int j = DateTime.Now.Month;
			int k = DateTime.Now.Day;
			//Check the rest of the days in the year
			for (int i = 0; i < daysLeftInYear; i++)
			{
				//Go to next month
				if (k > System.DateTime.DaysInMonth(DateTime.Now.Year, j))
				{
					k = 1;
					j++;
				}
				
				//Check that day
				if (calendar[(j - 1), (k - 1)] == true)
				{
					return true;
				}
				k++;
			}
			return false;
		}

		//The owner of a hosting unit will be able to send a customer order 
		//	(changing the status to "Sent Mail"), only if he have signed a bank 
		//	account debit authorization.
		public bool CheckForStatus(int id)
		{
			List<Host> hostList = new List<Host>();
			hostList = dal.GetListHost();
			//Find the obj if it exist if not return null
			var matchQuery = (from item in hostList
							  where (item.HostKey == id)
							  select item).FirstOrDefault();
			//Return true if info matches
			if (matchQuery != null)
			{
				if ("Yes" == matchQuery.HostCollectionClearance.ToString())
				{
					//You can charge Host
					return true;
				}
				//You can't charge host
				return false;
			}
			else
			{
				return false;
			}
		}

		//Once the order status has changed to close a transaction, its status can no longer be changed.
		public bool CheckOrderStatus(Order data)
		{
			if (data.OrderStatus == Statuses.ClosedForCustomerResponse || data.OrderStatus == Statuses.ClosedForCustomerUnresponsiveness)
			{
				//Status can not be change 
				return true;
			}
			else
			{
				//Status can be change
				return false;
			}
		}

		//When the order status changes to "Sent Mail" 
		//	- the system will automatically send a mail to the customer 
		//	  with the order details.
		public void SendGuestEmail(HostingUnit unit, GuestRequest guest)
		{
			//Send email need to implement 
			MailMessage mail = new MailMessage();
			mail.To.Add(guest.GuestMailAddress);
			mail.From = new MailAddress(unit.UnitOwner.HostEmail);
			mail.Subject = "Unit Has Been Found";
			mail.Body = ("Dear " + guest.GuestFamilyName + "\n We have Found you a hosting unit for all you needs. Please email " + unit.UnitOwner.HostEmail +
						" for further information about your stay. We like to thank you for using our services and have a nice day.\n" + "Unit information:\n" +
						"Unit Location: " + unit.Location + "\n" + "Unit area: " + unit.UnitArea.ToString() + "\n" + "Unit Type: " + unit.UnitType.ToString() + "\n" +
						"Unit Price " + unit.UnitPrice.ToString() + "$\n" + "Amount of Adults: " + unit.UnitAdult.ToString() + "\n" + "Amount of children: " + unit.UnitChildren.ToString() + "\n" +
						"Pool: " + unit.UnitPool.ToString() + "\n" + "Jacuzzi: " + unit.UnitJacuzzi.ToString() + "\n" + "Garden: " + unit.UnitGarden.ToString() + "\n" + "Children Attraction: " +
						unit.UnitChildAttraction.ToString() + "\n" + "Thank You for you time," + "\n" + "Upper management");


			// HTMLDefinition that the message content is in format
			mail.IsBodyHtml = true;
			
			// Smtp Create object type
			SmtpClient smtp = new SmtpClient();
			
			// gmailConfigure server of
			smtp.Host = "smtp.gmail.com";
			
			//gmailConfigure login information ( user name and password ) for account e
			smtp.Credentials = new System.Net.NetworkCredential("NotMyEmail@gmail.com", " 12345 ");
			
			// SSLBy " P. Minister requirement , an obligation to allow in this case
			smtp.EnableSsl = true;
			try
			{
				//Send message
				smtp.Send(mail);
			}
			catch (Exception ex)
			{
				//Handling errors and catching exceptions 
				throw ex;
			}
		}
		
		//When booking status changes due to closing of transaction 
		//	- a fee of NIS 10 per accommodation day must be calculated. 
		//	  (See note below)
		public void ChargeHost(Order data)
		{
			double price;
			if (data.OrderStatus == Statuses.ClosedForCustomerResponse)
			{
				List<GuestRequest> guestList = new List<GuestRequest>();
				guestList = dal.GetListCustomers();
				//Find the obj if it exist if not return null
				var requestQuery = (from item in guestList
							where (item.GuestRequestKey == data.OrderRequestKey)
							select item).FirstOrDefault();
				//Check if query exist
				if (requestQuery == null)
				{
					throw new Exception("exception");
				}
				DateTime first = requestQuery.RequestEntryDate;
				DateTime last = requestQuery.RequestReleaseDate;
				//Get number of days for vacation 
				int totalDays = 0;
				TimeSpan span = last - first;
				totalDays = span.Days;

				//Number of days time fixed rate for charge
				price = totalDays * Configuration.st_commission;
				//Send email to host that he is being charged 
			}
		}
		
		//When order status changes due to transaction closing 
		//	- the relevant dates must be marked in the matrix.
		public void MarkCaledar(Order data)
		{
			List<GuestRequest> guestList = new List<GuestRequest>();
			guestList = dal.GetListCustomers();
			//Find the obj if it exist if not return null
			var requestQuery = (from item in guestList
						where (item.GuestRequestKey == data.OrderRequestKey)
						select item).FirstOrDefault();
			//Check if query exist
			if (requestQuery == null)
			{
				throw new Exception("exception");
			}
			List<HostingUnit> unitList = new List<HostingUnit>();
			unitList = dal.GetListUnits();
			//Find the obj if it exist if not return null
			var unitQuery = (from item in unitList
							 where (item.HostingUnitKey == data.OrderUnitKey)
							 select item).FirstOrDefault();
			//Check if query exist
			if (unitQuery == null)
			{
				throw new Exception("exception");
			}
			
			//Mark days could not have mad order in the first place if the days were not available
			DateTime first = requestQuery.RequestEntryDate;
			DateTime last = requestQuery.RequestReleaseDate;
			//Get number of days for vacation 
			int totalDays = 0;
			TimeSpan span = last - first;
			totalDays = span.Days;

			//Update
			int j = first.Month;
			int k = first.Day;
			for (int i = 0; i < totalDays; i++)
			{
				if (k > DateTime.DaysInMonth(DateTime.Today.Year, j))
				{
					k = 1;
					j++;
				}
				
				unitQuery.UnitDiary[(j - 1), (k - 1)] = true;
				k++;
			}
			
			//Update the hosting unit
			HostingUnit newUnit = null;
			newUnit = unitQuery.Copy();
			try
			{
				dal.UpdateHostingUnit(newUnit);
				dal.LoadFiles();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		//When an order status changes due to a transaction closing 
		//	- the customer order status must be changed accordingly, 
		//	  as well as the status of all other customer orders.
		public void ChangeOrderStatus(Order data)
		{
			//Check if it was already closed
			if (CheckOrderStatus(data))
			{
				//Order was already marked
				throw new Exception("order has already been closed");
			}

			List<GuestRequest> guestList = dal.GetListCustomers();
			
			//Find the obj if it exist if not return null
			var requestQuery = (from item in guestList
						where (item.GuestRequestKey == data.OrderRequestKey)
						select item).FirstOrDefault();

			//Check if someone else closed on the request
			if(requestQuery != null)
			{
				if (CheckOrderForClose(requestQuery))
				{
					data.OrderStatus = Statuses.ClosedForCustomerUnresponsiveness;
					Order newOrder1 = null;
					newOrder1 = data.Copy();
					try
					{
						dal.OrderUpdate(newOrder1);
					}
					catch (Exception ex)
					{
						throw ex;
					}
					throw new Exception("Order Was closed elsewhere");
				}
			}

			//Update status and order
			data.OrderStatus = Statuses.ClosedForCustomerResponse;
			
			//make days on calendar
			try
			{
				MarkCaledar(data);
				Order newOrder = null;
				newOrder = data.Copy();
				dal.OrderUpdate(newOrder);
				ChargeHost(data);
				ChangeAllStatus(data);
				dal.LoadFiles();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		//Change all status of other orders
		public void ChangeAllStatus(Order data)
		{
			List<Order> orderList = new List<Order>();
			orderList = dal.GetListOrders();
			foreach (var item in orderList)
			{
				if(item.OrderUnitKey != data.OrderUnitKey && item.OrderRequestKey == data.OrderRequestKey)
				{
					try
					{
						item.OrderStatus = Statuses.ClosedForCustomerUnresponsiveness;
						dal.OrderUpdate(item);
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
			}
		}
		
		//Account billing authorization cannot be revoked when
		// there is an offer associated with it in open mode.
		public bool CheckBillingStatus(Host data)
		{
			List<Order> orderList = new List<Order>();
			orderList = dal.GetListOrders();
			List<HostingUnit> unitList = new List<HostingUnit>();
			unitList = dal.GetListUnits();
			List<HostingUnit> unit = unitList.FindAll(item => (item.UnitOwner == data));
			List<Order> order = new List<Order>();
			foreach (var item in orderList)
			{
				foreach (var thing in unit)
				{
					if (item.OrderUnitKey == thing.HostingUnitKey)
					{
						order.Add(item);
					}
				}
			}
			foreach (var item in order)
			{
				if (item.OrderStatus == Statuses.SentEmail)
				{
					//Can't update status
					return true;
				}
			}

			//Can change status
			return false;
		}
		//•	A function that accepts a date and number of vacation days 
		//	and returns the list of all available accommodation units on that date.
		public List<HostingUnit> GetUnitDate(DateTime date, int days)
		{
			if (days < 0)
			{
				return null;
			}
			DateTime endDate = date.AddDays(days);
			List<HostingUnit> unit = dal.GetListUnits();
			//Check for days between the two dates
			return unit.FindAll(item => (CheckCalendarDay(item, date, endDate) == false));
		}
		//•	A function that accepts one or two dates. The function returns 
		//	the number of days that have passed from the first date to the 
		//	second, or if only one date has been received - from the first 
		//	date to the present day.
		public int CheckNumberDay(DateTime first, DateTime second = default(DateTime))
		{
			int totalDays = 0;
			if (second != default(DateTime))
			{
				TimeSpan span = second - first;
				totalDays = span.Days;
				return totalDays;
			}
			else
			{
				second = DateTime.Now;
				TimeSpan span = first - second;
				totalDays = span.Days;
				return totalDays;
			}
		}

		//Combines unit and request into a hosting unit into a order
		public void MakeOrder(HostingUnit unit, GuestRequest guest)
		{
			//Check if order exist already;
			List<HostingUnit> listUnit = dal.GetListUnits();
			List<Order> listOrder = dal.GetListOrders();
			List<GuestRequest> listRequest = dal.GetListCustomers();
			var matchQuery = (from item in listOrder
						where (item.OrderRequestKey == guest.GuestRequestKey) &&
						      (item.OrderUnitKey == unit.HostingUnitKey)
						       select item).FirstOrDefault();
			//Order exist already
			if (matchQuery != null)
			{
				throw new Exception("order exist already");
			}
			
			//Check if unit meet request demands
			if (!(CheckDemands(unit, guest)))
			{
				throw new Exception("unit does not meet demands of request");
			}
			
			//Check if demands are met
			if (!CheckDemands(unit, guest))
			{
				throw new Exception("Unit doesn't meet demands");
			}
			//Check if it possible to make order
			if (CheckForStatus(unit.UnitOwner.HostKey) == false)
			{
				throw new Exception("Status does not allow for making unit orders");
			}
			
			//Check if days are available in hosting unit
			if (CheckCalendarDay(unit, guest.RequestEntryDate, guest.RequestReleaseDate))
			{
				throw new Exception("Days are not available in unit");
			}
			
			//Make new order
			Order order = new Order();
			Order newOrder = null;
			order.OrderUnitKey = unit.HostingUnitKey;
			order.OrderRequestKey = guest.GuestRequestKey;
			order.OrderStatus = (BE.Statuses)Enum.GetValues(typeof(BE.Statuses)).GetValue(1);
			order.OrderCreation = DateTime.Now;
			order.EmailDelivery = DateTime.Now;
			newOrder = order.Copy();
			try
			{
				dal.AddOrder(newOrder);
			}
			catch (Exception)
			{
				throw new Exception("Unable to add Order");
			}
			
			//Send email is commented out because we do not wish to have our email information on the program
			/*
			try
			{
				Thread thread = new Thread(() => SendGuestEmail(unit, guest));
				thread.Start();
				//SendGuestEmail(unit, guest);
			}
			catch (Exception)
			{
				throw;
			}*/
		}
		
		//Check if days are available on calendar
		public bool CheckCalendarDay(HostingUnit unit, DateTime first, DateTime second)
		{
			if(second.Year > DateTime.Today.Year || first.Year > DateTime.Today.Year)
			{
				return true;
			}

			//Get number of days for vacation 
			int totalDays = 0;
			TimeSpan span = second - first;
			totalDays = span.Days;
			int j = first.Month;
			int k = first.Day;
			for (int i = 0; i < totalDays; i++)
			{
				if (k > System.DateTime.DaysInMonth(DateTime.Today.Year, j))
				{
					k = 1;
					j++;
				}
				if (unit.UnitDiary[(j - 1), (k - 1)] == true)
				{
					return false;
				}
				k++;
			}
			
			//Days are not occupied 
			return false;
		}
		
		//C# code to validate email address
		//https://stackoverflow.com/questions/1365407/c-sharp-code-to-validate-email-address/1374644#1374644
		//Check valid emails
		public bool CheckValidEmail(string email)
		{
			try
			{
				var addr = new MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}
		
		//Check if demands are met
		public bool CheckDemands(HostingUnit unit, GuestRequest guest)
		{
			//https://stackoverflow.com/questions/943398/get-int-value-from-enum-in-c-sharp#:~:text=You%20can%20use%3A,Parse(e.
			//Convert enum into int
			//Convert.ToInt32(unit.UnitArea)
			//if all areas then skip
			if (Convert.ToInt32(guest.RequestArea) != 0)
			{
				//Check if the areas are the same
				if (Convert.ToInt32(guest.RequestArea) != Convert.ToInt32(unit.UnitArea))
				{
					return false;
				}
			}
			
			//Check if areas are the same
			if (guest.RequestType != unit.UnitType)
			{
				return false;
			}
			//Check if the occupancy number matches
			if (guest.RequestAdult > unit.UnitAdult || guest.RequestChildren > unit.UnitChildren)
			{
				return false;
			}
			
			//Check if there is a pool
			if (Convert.ToInt32(guest.RequestPool) != 2)
			{
				//Check if the ares are the same
				if (Convert.ToInt32(guest.RequestPool) != Convert.ToInt32(unit.UnitPool))
				{
					return false;
				}
			}
			
			//Check if there is a jacuzzi
			if (Convert.ToInt32(guest.RequestJacuzzi) != 2)
			{
				//Check if the areas are the same
				if (Convert.ToInt32(guest.RequestJacuzzi) != Convert.ToInt32(unit.UnitJacuzzi))
				{
					return false;
				}
			}
			
			//Check if there is a garden
			if (Convert.ToInt32(guest.RequestGarden) != 2)
			{
				//Check if the areas are the same
				if (Convert.ToInt32(guest.RequestGarden) != Convert.ToInt32(unit.UnitGarden))
				{
					return false;
				}
			}
			
			//Check if there is a attraction for children
			if (Convert.ToInt32(guest.RequestChildAttraction) != 2)
			{
				//Check if the areas are the same
				if (Convert.ToInt32(guest.RequestChildAttraction) != Convert.ToInt32(unit.UnitChildAttraction))
				{
					return false;
				}
			}
			return true;
		}
		
		//Check if another person closed an order
		public bool CheckOrderForClose(GuestRequest guest)
		{
			List<Order> listOrder = dal.GetListOrders();
			dal.LoadFiles();
			List<GuestRequest> listRequest = dal.GetListCustomers();
			var matchQuery = (from item in listOrder
					  where (item.OrderRequestKey == guest.GuestRequestKey) &&
					((item.OrderStatus == Statuses.ClosedForCustomerResponse) ||(item.OrderStatus == Statuses.ClosedForCustomerUnresponsiveness))
					  select item).FirstOrDefault();
			//Order exist already
			if (matchQuery != null)
			{
				return true;
			}
			return false;
		}
		
		//Get hosting unit data
		public HostingUnit GetUnitData(int id)
		{
			List<HostingUnit> listUnit = dal.GetListUnits();
			var matchQuery = (from item in listUnit
					  where item.UnitOwner.HostKey == id
					  select item).FirstOrDefault();

			if (matchQuery != null)
			{
				return matchQuery;
			}
			return null;
		}
		
		//Get unit of order
		public HostingUnit GetUnitOfOrder(Order data, int id)
		{
			List<HostingUnit> listUnit = dal.GetListUnits();
			var matchQuery = (from item in listUnit
					  where ((item.UnitOwner.HostKey == id) &&( item.HostingUnitKey == data.OrderUnitKey))
					  select item).FirstOrDefault();

			if (matchQuery != null)
			{
				return matchQuery;
			}
			return null;
		}
		
		//Get guest request info
		public GuestRequest GetRequestData(int id)
		{
			List<GuestRequest> listRequest = dal.GetListCustomers();
			var matchQuery = (from item in listRequest
					  where item.GuestRequestKey == id
					  select item).FirstOrDefault();

			if (matchQuery != null)
			{
				return matchQuery;
			}
			return null;
		}
		
		//•	A function that can return all customer requirements that fit a particular condition 
		//	(meaning that the function accepts a delegate that corresponds to a method that works
		//	on customer demand and returns e bool and that's how the condition is defined)
		public List<GuestRequest> FilterGuestRequests(Func<GuestRequest, bool> filter)
		{
			List<GuestRequest> requestList = dal.GetListCustomers();
			return requestList.FindAll(item => filter(item));
		}

		//•	List of hosted units (Grouping) According to the required area.
		public IEnumerable<IGrouping<Areas, HostingUnit>> GroupUnitByArea()
		{
			List<HostingUnit> unitList = dal.GetListUnits();
			return unitList.GroupBy(unit => unit.UnitArea);
		}

		//•	Clustered Customer Requirements List (Grouping) According to the required area .
		public IEnumerable<IGrouping<Areas, GuestRequest>> GroupRequestsByArea()
		{
			/*
			List<GuestRequest> requesLists = dal.GetListCustomers();
				return from item in requesLists
				group item by item.RequestArea;*/
			List<GuestRequest> requesLists = dal.GetListCustomers();
			return requesLists.GroupBy(unit => unit.RequestArea);
		}

		//•	Clustered Customer Requirements List (Grouping) According to the number of vacationers.
		public IEnumerable<IGrouping<int, GuestRequest>> GroupRequestByAccupants()
		{
			List<GuestRequest> requesLists = dal.GetListCustomers();
			/*return from item in requesLists
				group item by item.RequestAdult 
				+ item.RequestChildren;*/
			return requesLists.GroupBy(unit => (unit.RequestAdult + unit.RequestChildren));
		}
		
		//•	Grouped host list (Grouping) According to the number of accommodation units they hold
		public IEnumerable<IGrouping<int, HostingUnit>> GroupUnitByAccupants()
		{
			List<HostingUnit> unitList = dal.GetListUnits();
			return unitList.GroupBy(unit => (unit.UnitAdult + unit.UnitChildren));
		}

		//•	Function takes accommodation unit and returns the number of orders that were sent 
		//	divided by the number of closed orders through the site.
		public int UnitStatOrders(HostingUnit hostingUnit)
		{
			//Check if unit exist in case it was deleted 
			if(GetUnitData(hostingUnit.HostingUnitKey) == null)
			{
				return 0;
			}
			List<Order> orderList = dal.GetListOrders();
			return orderList.Count(item => item.OrderUnitKey == hostingUnit.HostingUnitKey && item.OrderStatus == Statuses.SentEmail);
		}
		public int UnitStatClosed(HostingUnit hostingUnit)
		{
			//Check if unit exists in case it was deleted 
			if (GetUnitData(hostingUnit.HostingUnitKey) == null)
			{
				return 0;
			}
			List<Order> orderList = dal.GetListOrders();
			return orderList.Count(item => item.OrderUnitKey == hostingUnit.HostingUnitKey &&
					      (item.OrderStatus == Statuses.ClosedForCustomerResponse || 
					       item.OrderStatus == Statuses.ClosedForCustomerUnresponsiveness));
		}
		
		//•	A function that accepts customer demand and returns the number of orders sent to it him.
		public int RequestStat(GuestRequest guestRequest)
		{
			//Check if unit exist in case it was deleted 
			if (GetRequestData(guestRequest.GuestRequestKey) == null)
			{
				return 0;
			}
			List<Order> orderList = dal.GetListOrders();
			return orderList.Count(item => item.OrderRequestKey == guestRequest.GuestRequestKey);
		}
		
		//•	A function that accepts several days, and returns all orders that
		//	have elapsed since they were created / since the email was sent to 
		//	a customer greater than or equal to the number of days the function received.
		public List<Order> OrderStat(int days)
		{
			if (days < 0)
			{
				return null;
			}
			List<Order> orderList = dal.GetListOrders();
			
			//Email is sent the same day as when the email is create 
			return orderList.FindAll(item => (DateTime.Today - item.OrderCreation).Days >= days);
		}
		
		//Get commission calculations 
		public double GetCommission(int id)
		{
			List<Order> orderList = dal.GetListOrders();
			List<GuestRequest> requestList = dal.GetListCustomers();
			double total = 0;
			List<Order> order = orderList.Where(item => requestList.Any(p2 =>
						(p2.GuestRequestKey == item.OrderRequestKey &&
						 p2.RequestEntryDate.Month == id )) && 
						 item.OrderStatus == Statuses.ClosedForCustomerResponse).ToList();
							    
			//Total = order.Count(p => p.OrderStatus == Statuses.ClosedForCustomerResponse);
			List<GuestRequest> guest = requestList.Where(item1 => order.Any(p3 => p3.OrderRequestKey == item1.GuestRequestKey)).ToList();
			foreach (var item in guest)
			{
				DateTime first = item.RequestEntryDate;
				DateTime last = item.RequestReleaseDate;
				//Get number of days for vacation 
				int totalDays = 0;
				TimeSpan span = last - first;
				totalDays = span.Days;
				total += (totalDays * Configuration.st_commission);
			}
			
			//Return orderList.Where(p1 => matchQuery.Any(p2 => p2.HostingUnitKey == p1.OrderUnitKey)).ToList();
			return total;
		}
		
		//Returns all orders that have elapsed since a day 
		public List<Order> OrderFunction(int day)
		{
			List<Order> orderList = dal.GetListOrders();
			List<Order> order = new List<Order>();
			foreach (var item in orderList)
			{
				TimeSpan span = DateTime.Now - item.EmailDelivery;
				if (span.Days > day)
				{
					order.Add(item);
				}
			}
			return order; ;
		}
		
		public void StartThreads()
		{
			BackgroundWorker backgroundWorker2 = new BackgroundWorker();
			backgroundWorker2.DoWork += StartUp;
			backgroundWorker2.RunWorkerCompleted += null;
			backgroundWorker2.RunWorkerAsync();
			
			BackgroundWorker backgroundWorker1 = new BackgroundWorker();
			backgroundWorker1.DoWork += BankworkerAction;
			backgroundWorker1.RunWorkerCompleted += null;
			backgroundWorker1.RunWorkerAsync(30000);

			Thread updateThread = new Thread(new ThreadStart(UpdateExpire));
			updateThread.Start();
		}
		
		public void BankworkerAction(object sender, DoWorkEventArgs e)
		{
			while(true)
			{
				dal.CreateBankAccount();
				List<BankBranch> branchList = null;
				dal.SaveBankeBranches();
				Thread.Sleep(2000);
				branchList = dal.GetListBanks();
				if (branchList.Count() != 0)
				{
					e.Cancel = true;
					break;
				}
			}
		}
		
		public void StartUp(object sender, DoWorkEventArgs e)
		{
			//Update the list
			dal.CreateGuestRequest();
			dal.CreateOrder();
			
			//Update data from files
			dal.LoadFiles();
			e.Cancel = true;
		}
		
		//Update all expired orders/request
		public void UpdateExpire()
		{
			Thread.Sleep(40000);
			//Update orders that have ended but have not been updated.
			List<Order> orderList = new List<Order>();
			orderList = dal.GetListOrders();
			foreach (var item in orderList)
			{
				if (item.OrderStatus == Statuses.SentEmail || item.OrderStatus == Statuses.NotAddressed)
				{
					DateTime first = item.EmailDelivery;
					DateTime last = DateTime.Now;
					//Get number of days for vacation 
					int totalDays = 0;
					TimeSpan span = last - first;
					totalDays = span.Days;

					if (totalDays > Configuration.st_expire_days )
					{
						try
						{
							item.OrderStatus = Statuses.ClosedForCustomerUnresponsiveness;
							dal.OrderUpdate(item);
						}
						catch (Exception ex)
						{
							//Email admin error
							throw ex;
						}
					}
				}
			}
			
			List<GuestRequest> gustList = dal.GetListCustomers();
			foreach (var item in gustList)
			{
				if(item.RequestEntryDate < DateTime.Now) 
				{		
					try
					{
						item.RequestStatus = Statuses.ClosedForCustomerUnresponsiveness;
						dal.CustomerRequestUpdate(item);
					}
					catch (Exception ex)
					{
						//Email admin bug
						throw ex;
					}
				}
			}
			
			//Refresh the files
			dal.LoadFiles();
		}
	}
}
