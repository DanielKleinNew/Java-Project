using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.InteropServices.WindowsRuntime;

namespace DAL
{
	public interface IDAL
	{

		//Create
		//Add a customer request 
		void AddCustomer(GuestRequest request);
		//Add a hosting unit
		void AddHostingUnit(HostingUnit unit);
		//Add an order
		void AddOrder(Order order);

		//Add host
		void AddHost(Host host);

		//Read
		//Get a list of all accommodation units
		List<HostingUnit> GetListUnits();
		//Receive a list of all customer requests .
		List<GuestRequest> GetListCustomers();
		//Receive a list of all orders
		List<Order> GetListOrders();
		//Receives a list of all existing bank branches in Israel
		List<BankBranch> GetListBanks();

		//Get list of host
		List<Host> GetListHost();
		//Get the configuration 
		Configuration GetConfigurations();

		//Update
		//Customer request update  (status change) .
		void CustomerRequestUpdate(GuestRequest request);
		//Hosting unit update
		void UpdateHostingUnit(HostingUnit unit);
		//Order Update (Status Change)
		void OrderUpdate(Order order);

		//Update host
		void HostUpdate(Host host);
		//Update the configuration be
		void ConfigurationUpdate(Configuration config);

		//Delete 
		//Delete a hosting unit
		void DeleteHostingUnit(int id);

		//Delete host
		void DeleteHost(int id);
		//Delete order
		void DeleteOrder(int id);
		//Delete request
		void DeleteRequest(int id);

		//XML related function 
		//Create root and path fro files
		void CreateGuestRequest();
		void CreateOrder();
		void CreateBankAccount();
		//Load files
		void LoadGuestRequest();
		void LoadOrder();
		void LoadBankAccount();

		//Save/load the config file
		void BeSaveConfigList(Configuration config);
		Configuration BeLoadConfigList();
		void DalSaveConfigList(DalConfiguration config);
		DalConfiguration DalLoadConfigList();
		//Bank save
		void SaveBankeBranches();

		//Add to file
		void FileAddGuestRequest(GuestRequest request);
		void FileAddHostingUnit(HostingUnit unit);
		void FileAddOrder(Order p);
		void FileAddHost(Host p);
		
		//Remove from file
		void FileRemoveGuestRequest(int id);
		void FileRemoveHostingUnitt(int id);
		void FileRemoveOrder(int id);
		void FileRemoveHost(int id);

		//Update file
		void FileUpdateGuestRequest(GuestRequest p);
		void FileUpdateHostingUnit(HostingUnit p);
		void FileUpdateOrder(Order p);
		void FileUpdateHost(Host p);

		//Load all files to cache 
		void LoadFiles();
	}
}
