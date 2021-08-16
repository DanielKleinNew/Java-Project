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
using System.Net;

namespace DAL 
{
	public class Dal_XML_imp : IDAL
	{
		
		//One instance of the Dal_imp
		private static Dal_XML_imp instance;
		private Dal_XML_imp()
		{
			//t++
		}
		
		//Return one instance of the DAL
		public static Dal_XML_imp Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Dal_XML_imp();
				}
				return instance;
			}
		}

		public XElement GuestRequestRoot;
		private static string GuestRequestPath = @"GuestRequestXML.xml";
		private static string HostingUnitPath = @"HostingUnitXML.xml";
		private static string HostPath = @"HostPathXML.xml";
		private static string BeConfigPath = @"BeConfigPathXML.xml";
		private static string DalConfigPath = @"DalConfigPathXML.xml";

		// public XElement HostingUnitRoot;

		public XElement OrderRoot;
		private static string OrderPath = @"OrderXML.xml";

		public XElement BankAccountRoot;
		private static string BankAccountPath = @"BankAccountsPathXML.xml";

		public void CreateGuestRequest()
		{
			try
			{
				if (!File.Exists(GuestRequestPath))
				{
					GuestRequestRoot = new XElement("GuestRequests");
					GuestRequestRoot.Save(GuestRequestPath);
				}
				else
					LoadGuestRequest();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
		public void CreateOrder()
		{
			try
			{
				if (!File.Exists(OrderPath))
				{
					OrderRoot = new XElement("Orders");
					OrderRoot.Save(OrderPath);
				}
				else
					LoadOrder();
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
		
		public void CreateBankAccount()
		{
			try
			{
				if (!File.Exists(BankAccountPath))
				{
					BankAccountRoot = new XElement("BankAccounts");
					BankAccountRoot.Save(BankAccountPath);
				}
				else
					LoadBankAccount();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void LoadGuestRequest()
		{
			try
			{
				GuestRequestRoot = XElement.Load(GuestRequestPath);
			}
			catch
			{
				throw new Exception("File upload problem");
			}
		}
		
		public void LoadOrder()
		{
			try
			{
				OrderRoot = XElement.Load(OrderPath);
			}
			catch
			{
				throw new Exception("File upload problem");
			}
		}
		
		public void LoadBankAccount()
		{
			try
			{
				BankAccountRoot = XElement.Load(BankAccountPath);
			}
			catch
			{
				throw new Exception("File upload problem");
			}
		}

		//Create
		//Use at least 4 Linq to object expressions for the purposes of queries.
		//Add a customer request 
		public void AddCustomer(GuestRequest request)
		{
			//If this was a network base application then update this system 
			//current list and configuration to match main databases 
			DataSource.guest_request_list.Clear();
			DataSource.guest_request_list = LoadGuestRequestList();
			DalConfiguration tempConfig = DalLoadConfigList();

			//Determines whether any element of a sequence exists or satisfies a condition.
			//https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.any?view=netcore-3.1
			
			//Check if list is empty 
			if (DS.DataSource.guest_request_list == null)
			{
				DS.DataSource.guest_request_list = new List<GuestRequest>();
			}
			
			//Check if the id already exist
			if (DS.DataSource.guest_request_list.Any(item => item.GuestRequestKey == request.GuestRequestKey))
			{
				throw new ArgumentException("Request exist already");
			}
			
			//Assign the item a unique id
			request.GuestRequestKey = ++DalConfiguration.st_request_key;

			//Add new item to list
			DataSource.guest_request_list.Add(request);
			
			//Add request to file
			FileAddGuestRequest(request);
			
			//Update the config in the file
			DalSaveConfigList(tempConfig);
			return;
		}

		//Add a hosting unit
		public void AddHostingUnit(HostingUnit unit)
		{
			Configuration temp = BeLoadConfigList();
			BeSaveConfigList(temp);

			//If this was a network base application then update this system 
			//current list and configuration to match main databases 
			DataSource.hosting_unit_list.Clear();
			DataSource.hosting_unit_list = LoadHostingUnitList();
			DalConfiguration tempConfig = DalLoadConfigList();

			//Check if the id already exist
			if (DataSource.hosting_unit_list.Any(item => item.HostingUnitKey == unit.HostingUnitKey))
			{
				throw new ArgumentException("Hosting unit exist already");
			}
			
			//Assign the item a unique id
			unit.HostingUnitKey = ++DalConfiguration.st_unit_key;
			
			//Add new item to list
			DataSource.hosting_unit_list.Add(unit);
			
			//Add item to file
			FileAddHostingUnit(unit);
			
			//Update config
			DalSaveConfigList(tempConfig);
			return;
		}

		//Add an order
		public void AddOrder(Order order)
		{
			//If this was a network base application then update this system
			//current list and configuration to match main databases 
			DataSource.order_list.Clear();
			DataSource.order_list = LoadOrderList();
			DalConfiguration tempConfig = DalLoadConfigList();

			//Check if order id already exist
			if (DataSource.order_list.Any(item => item.OrderKey == order.OrderKey))
			{
				throw new ArgumentException("Order exist already");
			}
			
			//Assign the item a unique id
			order.OrderKey = ++DalConfiguration.st_order_key;
			
			//Add new item to list
			DataSource.order_list.Add(order);
			
			//Add item to file
			FileAddOrder(order);
			
			//Update config
			DalSaveConfigList(tempConfig);
			return;
		}

		//Add host
		public void AddHost(Host host)
		{
			//If this was a network base application then update this system 
			//current list and configuration to match main databases 
			DataSource.host_list.Clear();
			DataSource.host_list = LoadHostList();
			DalConfiguration tempConfig = DalLoadConfigList();

			//Search if Host id already exist
			if (DataSource.host_list.Any(item => item.HostKey == host.HostKey))
			{
				throw new ArgumentException("Host exist already");
			}
			
			//Assign the item a unique id
			host.HostKey = ++DalConfiguration.st_host_key;
			
			//Add new item to list
			DataSource.host_list.Add(host);
			
			//Add item to file
			FileAddHost(host);
			
			//Update config
			DalSaveConfigList(tempConfig);
			return;
		}

		//Read Deep copy
		//Get a list of all accommodation units
		public List<HostingUnit> GetListUnits()
		{
			//https://stackoverflow.com/questions/14007405/how-create-a-new-deep-copy-clone-of-a-listt
			//How to create a new deep copy (clone) of a List<T>
			//Whatever item was in the original list place it into a new list deep copy
			return DataSource.hosting_unit_list.Select(item => item).ToList();
		}
		
		//Receive a list of all customer requests .
		public List<GuestRequest> GetListCustomers()
		{
			//Whatever item was in the original list place it into a new list deep copy
			return DataSource.guest_request_list.Select(item => item).ToList();
		}
		
		//Receive a list of all orders
		public List<Order> GetListOrders()
		{
			//Whatever item was in the original list place it into a new list deep copy
			return DataSource.order_list.Select(item => item).ToList();
		}
		
		//Receives a list of all existing bank branches in Israel
		public List<BankBranch> GetListBanks()
		{
			//Whatever item was in the original list place it into a new list deep copy
			return DataSource.bank_branch_list.Select(item => item).ToList();
		}
		
		//Get list of host
		public List<Host> GetListHost()
		{
			//Whatever item was in the original list place it into a new list deep copy
			return DataSource.host_list.Select(item => item).ToList();
		}
		
		//Get the configuration 
		public Configuration GetConfigurations()
		{
			return BeLoadConfigList();
		}

		//Update
		//Customer request update  (status change) .
		public void CustomerRequestUpdate(GuestRequest request)
		{
			//https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/linq-to-objects
			//Linq to objects return null if item is not found
			//Find the obj if it exist if not return null
			var matchQuery = (from item in DataSource.guest_request_list
							  where item.GuestRequestKey == request.GuestRequestKey
							  select item).FirstOrDefault();
			//Update the item in the list
			if (matchQuery != null)
			{
				matchQuery = request;
				FileUpdateGuestRequest(matchQuery);
				return;
			}
			throw new ArgumentException("Hosting");
		}
		
		//Hosting unit update
		public void UpdateHostingUnit(HostingUnit unit)
		{
			//Find the obj. If it exist if not return null
			var matchQuery = (from item in DataSource.hosting_unit_list
							  where item.HostingUnitKey == unit.HostingUnitKey
							  select item).FirstOrDefault();
			//Update the item in the list
			if (matchQuery != null)
			{
				matchQuery = unit;
				FileUpdateHostingUnit(matchQuery);
				return;
			}
			throw new ArgumentException("Hosting");
		}
		
		//Order Update (Status Change)
		public void OrderUpdate(Order order)
		{
			//Find the obj. If it exist if not return null
			var matchQuery = (from item in DataSource.order_list
							  where item.OrderKey == order.OrderKey
							  select item).FirstOrDefault();
			//Update the item in the list
			if (matchQuery != null)
			{
				matchQuery = order;
				FileUpdateOrder(matchQuery);
				return;
			}
			throw new ArgumentException("Hosting");
		}
		
		//Update host
		public void HostUpdate(Host host)
		{
			//Find the obj. If it exist if not return null
			var matchQuery = (from item in DataSource.host_list
							  where item.HostKey == host.HostKey
							  select item).FirstOrDefault();
			//Update the item in the list
			if (matchQuery != null)
			{
				matchQuery = host;
				FileUpdateHost(matchQuery);
				return;
			}
			throw new ArgumentException("Hosting");
		}

		//Update the configuration 
		public void ConfigurationUpdate(Configuration config)
		{
			BeSaveConfigList(config);	
		}

		//Delete 
		//Delete a hosting unit
		public void DeleteHostingUnit(int id)
		{
			//search for item to remove if it exist 
			int count = DataSource.hosting_unit_list.RemoveAll(obj => obj.HostingUnitKey == id);
			if (count == 0)
			{
				throw new ArgumentException("Hosting Unit ", id.ToString());
			}
			FileRemoveHostingUnitt(id);
		}
		//delete host
		public void DeleteHost(int id)
		{
			//Search for item to remove if it exist 
			int count = DataSource.host_list.RemoveAll(obj => obj.HostKey == id);
			if (count == 0)
			{
				throw new ArgumentException("Host ", id.ToString());
			}
			FileRemoveHost(id);
		}
		
		//Delete order
		public void DeleteOrder(int id)
		{
			//Search for item to remove if it exist 
			int count = DataSource.order_list.RemoveAll(obj => obj.OrderKey == id);
			if (count == 0)
			{
				throw new ArgumentException("Order ", id.ToString());
			}
			FileRemoveOrder(id);
		}
		
		//Delete request
		public void DeleteRequest(int id)
		{
			//Search for item to remove if it exist 
			int count = DataSource.guest_request_list.RemoveAll(obj => obj.GuestRequestKey == id);
			if (count == 0)
			{
				throw new ArgumentException("Request ", id.ToString());
			}
			FileRemoveGuestRequest(id);
		}
		
		public void SaveBankeBranches()
		{	
			WebClient wc = new WebClient();
			try
			{	
				string xmlServerPath = @"https://www.boi.org.il/en/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/snifim_en.xml";
				wc.DownloadFile(xmlServerPath, BankAccountPath);

				List<BankBranch> bankList = new List<BankBranch>();
				try
				{
					bankList = (from p in BankAccountRoot.Elements()
								 select new BankBranch()
								 {
									BankNumber = Convert.ToInt32(p.Element("Bank_Code").Value),
									 BankName = p.Element("Bank_Name").Value,
									 BranchNumber = Convert.ToInt32(p.Element("Branch_Code").Value),
									 BranchAddress = p.Element("Address").Value,
									 BranchCity = p.Element("City").Value,
								 }).ToList();
				}
				catch
				{
					bankList.Clear();
				}
				DS.DataSource.bank_branch_list = bankList;	
			}
			catch (Exception)
			{
				string xmlServerPath = @"http://www.jct.ac.il/~coshri/atm.xml";
				wc.DownloadFile(xmlServerPath, BankAccountPath);

				List<BankBranch> bankList = new List<BankBranch>();
				try
				{
					bankList = (from p in BankAccountRoot.Elements()
								select new BankBranch()
								{
									BankNumber = Convert.ToInt32(p.Element("קוד_בנק").Value),
									BankName = p.Element("שם_בנק").Value,
									BranchNumber = Convert.ToInt32(p.Element("קוד_סניף").Value),
									BranchAddress = p.Element("כתובת_ה-ATM").Value,
									BranchCity = p.Element("ישוב").Value,
								}).ToList();
				}
				catch
				{
					bankList.Clear();
				}
				DS.DataSource.bank_branch_list = bankList;
			}
			finally
			{
				wc.Dispose();
			}
		}

		public void SaveGuestRequestList(List<GuestRequest> GuestRequestsList)
		{
			GuestRequestRoot = new XElement("GuestRequests", from p in GuestRequestsList
									 select new XElement("GuestRequests",
									 new XElement("GuestRequestKey", p.GuestRequestKey),
									 new XElement("PrivateName", p.GuestPrivateName),
									 new XElement("FamilyName", p.GuestFamilyName),
									 new XElement("MailAddress", p.GuestMailAddress),
									 new XElement("RequestStatus", p.RequestStatus),
									 new XElement("RegistrationDate", p.RegistrationDate),
									 new XElement("EntryDate", p.RequestEntryDate),
									 new XElement("ReleaseDate", p.RequestReleaseDate),
									 new XElement("Area", p.RequestArea),
									 new XElement("HostingType", p.RequestType),
									 new XElement("NumAdults", p.RequestAdult),
									 new XElement("NumChildren", p.RequestChildren),
									 new XElement("Pool", p.RequestPool),
									 new XElement("Jacuzzi", p.RequestJacuzzi),
									 new XElement("Garden", p.RequestGarden),
									 new XElement("CildrenAttraction", p.RequestChildAttraction)
									));
			GuestRequestRoot.Save(GuestRequestPath);
		}
		public List<GuestRequest> LoadGuestRequestList()
		{
			List<GuestRequest> guestList;
			try
			{
				guestList = (from p in GuestRequestRoot.Elements()
							select new GuestRequest()
							{
								GuestRequestKey = Convert.ToInt32(p.Element("GuestRequestKey").Value),
								GuestPrivateName = p.Element("PrivateName").Value,
								GuestFamilyName = p.Element("FamilyName").Value,
								GuestMailAddress = p.Element("MailAddress").Value,
								RequestStatus = (BE.Statuses)Enum.Parse(typeof(BE.Statuses), p.Element("RequestStatus").Value),
								RegistrationDate =  DateTime.Parse(p.Element("RegistrationDate").Value),
								RequestEntryDate = DateTime.Parse(p.Element("EntryDate").Value),
								RequestReleaseDate = DateTime.Parse(p.Element("ReleaseDate").Value),
								RequestArea = (BE.Areas)Enum.Parse(typeof(BE.Areas), p.Element("Area").Value),
								RequestType = (BE.Types)Enum.Parse(typeof(BE.Types), p.Element("HostingType").Value),
								RequestAdult = Convert.ToInt32(p.Element("NumAdults").Value),
								RequestChildren = Convert.ToInt32(p.Element("NumChildren").Value),
								RequestPool = (BE.Decision)Enum.Parse(typeof(BE.Decision), p.Element("Pool").Value),
								RequestJacuzzi = (BE.Decision)Enum.Parse(typeof(BE.Decision), p.Element("Jacuzzi").Value),
								RequestGarden = (BE.Decision)Enum.Parse(typeof(BE.Decision), p.Element("Garden").Value),
								RequestChildAttraction = (BE.Decision)Enum.Parse(typeof(BE.Decision), p.Element("CildrenAttraction").Value),
								
							}).ToList();
			}
			catch
			{
				guestList = null;
			}
			return guestList;
		}
		
		public void SaveHostingUnitList(List<HostingUnit> HostingUnitList)
		{
			//Check if file exist or if the file is empty 
			if (!File.Exists(HostingUnitPath) || new FileInfo(HostingUnitPath).Length == 0)
			{
				XmlSerializer x = new XmlSerializer(HostingUnitList.GetType());
				FileStream fs = new FileStream(HostingUnitPath, FileMode.CreateNew);
				x.Serialize(fs, HostingUnitList);
				fs.Close();
				return;
			}
			else
			{ 	//Erase old list and append new list
				XmlSerializer Tx = new XmlSerializer(HostingUnitList.GetType());
				FileStream Tfs = new FileStream(HostingUnitPath, FileMode.Create);
				Tx.Serialize(Tfs, HostingUnitList);
				Tfs.Close();
			}
		}
		
		public List<HostingUnit> LoadHostingUnitList()
		{
			List<HostingUnit> list = new List<HostingUnit>();
			try
			{
				XmlSerializer x = new XmlSerializer(typeof(List<HostingUnit>));
				FileStream fs = new FileStream(HostingUnitPath, FileMode.Open);
				list = (List<HostingUnit>)x.Deserialize(fs);
				fs.Close();
				return list;
			}
			catch 
			{
				return list;
			}	
		}
		
		public void SaveOrderList(List<Order> OrderList)
		{
			OrderRoot = new XElement("Orders", from p in OrderList
							   select new XElement("Orders",
							   new XElement("HostingUnitKey", p.OrderUnitKey),
							   new XElement("GuestRequestKey", p.OrderRequestKey),
							   new XElement("OrderKey", p.OrderKey),
							   new XElement("Status", p.OrderStatus),
							   new XElement("OrderDate", p.OrderCreation),
							   new XElement("EmailDate", p.EmailDelivery)
							   ));
			OrderRoot.Save(OrderPath);
		}
		
		public List<Order> LoadOrderList()
		{
			List<Order> orderList;
			try
			{
				orderList = (from p in OrderRoot.Elements()
							select new Order()
							{
								OrderUnitKey = Convert.ToInt32(p.Element("HostingUnitKey").Value),
								OrderRequestKey = Convert.ToInt32(p.Element("GuestRequestKey").Value),
								OrderKey = Convert.ToInt32(p.Element("OrderKey").Value),
								OrderStatus = (BE.Statuses)Enum.Parse(typeof(BE.Statuses), p.Element("Status").Value),
								OrderCreation = DateTime.Parse(p.Element("OrderDate").Value),
								EmailDelivery = DateTime.Parse(p.Element("EmailDate").Value),
								
							}).ToList();
			}
			catch
			{
				orderList = null;
			}
			return orderList;
		}
		
		public void SaveHostList(List<Host> HostList)
		{
			//Check if file exist or if the file is empty 
			if (!File.Exists(HostPath) || new FileInfo(HostPath).Length == 0)
			{
				XmlSerializer x = new XmlSerializer(HostList.GetType());
				FileStream fs = new FileStream(HostPath, FileMode.Create);
				x.Serialize(fs, HostList);
				fs.Close();
				return;
			}
			else
			{ 
				XmlSerializer Tx = new XmlSerializer(HostList.GetType());
				FileStream Tfs = new FileStream(HostPath, FileMode.Create);
				Tx.Serialize(Tfs, HostList);
				Tfs.Close();
			}
		}
		
		public List<Host> LoadHostList()
		{
			List<Host> list = new List<Host>(); 
			try
			{		
				XmlSerializer x = new XmlSerializer(typeof(List<Host>));
				FileStream fs = new FileStream(HostPath, FileMode.Open);
				list = (List<Host>)x.Deserialize(fs);
				fs.Close();
				return list;
			}
			catch 
			{
				return list;
			}
		}
		
		//Save config
		public void BeSaveConfigList(Configuration config)
		{
			XmlSerializer x = new XmlSerializer(config.GetType());
			FileStream fs = new FileStream(BeConfigPath, FileMode.Create);
			x.Serialize(fs, config);
			fs.Close();
			return;
			
		}
		
		public Configuration BeLoadConfigList()
		{
			Configuration config = new Configuration();
			try
			{
				XmlSerializer x = new XmlSerializer(typeof(Configuration));
				FileStream fs = new FileStream(BeConfigPath, FileMode.Open);
				config = (Configuration)x.Deserialize(fs);
				fs.Close();
				return config;
			}
			catch
			{
				return config;
			}
			
		}
		
		public void DalSaveConfigList(DalConfiguration config)
		{	
			XmlSerializer x = new XmlSerializer(config.GetType());
			FileStream fs = new FileStream(DalConfigPath, FileMode.Create);
			x.Serialize(fs, config);
			fs.Close();
			return;
		}
		
		public DalConfiguration DalLoadConfigList()
		{
			DalConfiguration config = new DalConfiguration();
			try
			{
				XmlSerializer x = new XmlSerializer(typeof(DalConfiguration));
				FileStream fs = new FileStream(DalConfigPath, FileMode.Open);
				config = (DalConfiguration)x.Deserialize(fs);
				fs.Close();
				return config;
			}
			catch
			{
				return config;
			}
		}
		
		public void FileAddGuestRequest(GuestRequest request)
		{
			XElement a1 = new XElement("GuestRequestKey", request.GuestRequestKey);
			XElement a2 = new XElement("PrivateName", request.GuestPrivateName);
			XElement a3 = new XElement("FamilyName", request.GuestFamilyName);
			XElement a4 = new XElement("MailAddress", request.GuestMailAddress);
			XElement a5 = new XElement("RequestStatus", request.RequestStatus);
			XElement a6 = new XElement("RegistrationDate", request.RegistrationDate);
			XElement a7 = new XElement("EntryDate", request.RequestEntryDate);
			XElement a8 = new XElement("ReleaseDate", request.RequestReleaseDate);
			XElement a9 = new XElement("Area", request.RequestArea);
			XElement a10 = new XElement("HostingType", request.RequestType);
			XElement a11 = new XElement("NumAdults", request.RequestAdult);
			XElement a12 = new XElement("NumChildren", request.RequestChildren);
			XElement a13 = new XElement("Pool", request.RequestPool);
			XElement a14 = new XElement("Jacuzzi", request.RequestJacuzzi);
			XElement a15 = new XElement("Garden", request.RequestGarden);
			XElement a16 = new XElement("CildrenAttraction", request.RequestChildAttraction);
			XElement st = new XElement("GuestRequests", a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16);
			GuestRequestRoot.Add(st);
			GuestRequestRoot.Save(GuestRequestPath);
		}
		
		public void FileAddHostingUnit(HostingUnit unit)
		{
			List<HostingUnit> unitList;
			unitList = LoadHostingUnitList();
			unitList.Add(unit);
			SaveHostingUnitList(unitList);	
		}
		
		public void FileAddOrder(Order p)
		{
			XElement a1 = new XElement("HostingUnitKey", p.OrderUnitKey);
			XElement a2 = new XElement("GuestRequestKey", p.OrderRequestKey);
			XElement a3 = new XElement("OrderKey", p.OrderKey);
			XElement a4 = new XElement("Status", p.OrderStatus);
			XElement a5 = new XElement("OrderDate", p.OrderCreation);
			XElement a6 = new XElement("EmailDate", p.EmailDelivery);
			XElement st = new XElement("Orders", a1, a2, a3, a4, a5, a6);
			OrderRoot.Add(st);
			OrderRoot.Save(OrderPath);
		}
		
		public void FileAddHost(Host p)
		{
			List<Host> hostList;
			hostList = LoadHostList();
			hostList.Add(p);
			SaveHostList(hostList);
		}
		
		public void FileRemoveGuestRequest(int id)
		{
			XElement RequestElement;
			try
			{
				RequestElement = (from p in GuestRequestRoot.Elements()
								  where Convert.ToInt32(p.Element("GuestRequestKey").Value) == id
								  select p).FirstOrDefault();
				RequestElement.Remove();
				GuestRequestRoot.Save(GuestRequestPath);		
			}
			catch
			{
				throw new ArgumentException("Request ", id.ToString());
			}
		}
		
		public void FileRemoveHostingUnitt(int id)
		{
			List<HostingUnit> unitList;
			unitList = LoadHostingUnitList();
			int count = unitList.RemoveAll(obj => obj.HostingUnitKey == id);
			if (count == 0)
			{
				throw new ArgumentException("Hosting Unit ", id.ToString());
			}
			SaveHostingUnitList(unitList);
		}
		
		public void FileRemoveOrder(int id)
		{
			XElement OrderElement;
			try
			{
				OrderElement = (from p in OrderRoot.Elements()
								  where Convert.ToInt32(p.Element("OrderKey").Value) == id
								  select p).FirstOrDefault();
				OrderRoot.Remove();
				OrderRoot.Save(OrderPath);		
			}
			catch
			{
				throw new ArgumentException("Order ", id.ToString());
			}
		}
		
		public void FileRemoveHost(int id)
		{
			List<Host> hostList;
			hostList = LoadHostList();
			int count = hostList.RemoveAll(obj => obj.HostKey == id);
			if (count == 0)
			{
				throw new ArgumentException("Host ", id.ToString());
			}
			SaveHostList(hostList);	
		}
		
		public void FileUpdateGuestRequest(GuestRequest p)
		{
			XElement RequestElement = (from item in GuestRequestRoot.Elements()
									   where Convert.ToInt32(item.Element("GuestRequestKey").Value) == p.GuestRequestKey
									   select item).FirstOrDefault();
									   
			RequestElement.Element("GuestRequestKey").Value = p.GuestRequestKey.ToString();
			RequestElement.Element("PrivateName").Value = p.GuestPrivateName;
			RequestElement.Element("FamilyName").Value = p.GuestFamilyName;
			RequestElement.Element("MailAddress").Value = p.GuestMailAddress;
			RequestElement.Element("RequestStatus").Value = p.RequestStatus.ToString();
			RequestElement.Element("RegistrationDate").Value = p.RegistrationDate.ToString();
			RequestElement.Element("EntryDate").Value = p.RequestEntryDate.ToString();
			RequestElement.Element("ReleaseDate").Value = p.RequestReleaseDate.ToString();
			RequestElement.Element("Area").Value = p.RequestArea.ToString();
			RequestElement.Element("HostingType").Value = p.RequestType.ToString();
			RequestElement.Element("NumAdults").Value = p.RequestAdult.ToString();
			RequestElement.Element("NumChildren").Value = p.RequestChildren.ToString();
			RequestElement.Element("Pool").Value = p.RequestPool.ToString();
			RequestElement.Element("Jacuzzi").Value = p.RequestJacuzzi.ToString();
			RequestElement.Element("Garden").Value = p.RequestGarden.ToString();
			RequestElement.Element("CildrenAttraction").Value = p.RequestChildAttraction.ToString();
			GuestRequestRoot.Save(GuestRequestPath);
		}
		
		public void FileUpdateHostingUnit(HostingUnit p)
		{
			FileRemoveHostingUnitt(p.HostingUnitKey);
			FileAddHostingUnit(p);
		}
		
		public void FileUpdateOrder(Order p)
		{
			XElement ordertElement = (from item in OrderRoot.Elements()
									   where Convert.ToInt32(item.Element("OrderKey").Value) == p.OrderKey
									  select item).FirstOrDefault();
			ordertElement.Element("GuestRequestKey").Value = p.OrderRequestKey.ToString();
			ordertElement.Element("OrderKey").Value = p.OrderKey.ToString();
			ordertElement.Element("Status").Value = p.OrderStatus.ToString();
			ordertElement.Element("OrderDate").Value = p.OrderCreation.ToString();
			ordertElement.Element("EmailDate").Value = p.EmailDelivery.ToString();
			OrderRoot.Save(OrderPath);
		}
		
		public void FileUpdateHost(Host p)
		{
			FileRemoveHost(p.HostKey);
			FileAddHost(p);
		}
		
		public void LoadFiles()
		{
			DS.DataSource.guest_request_list = LoadGuestRequestList();
			DS.DataSource.hosting_unit_list = LoadHostingUnitList();
			DS.DataSource.order_list = LoadOrderList();
			//DS.DataSource.bank_branch_list = 
			DS.DataSource.host_list = LoadHostList();
			DalConfiguration dalConfig = new DalConfiguration();
			dalConfig = DalLoadConfigList();
			Configuration beConfig = new Configuration();
			beConfig = BeLoadConfigList();
		}
	}
}
