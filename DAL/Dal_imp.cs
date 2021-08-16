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
	/*
	public class Dal_imp : IDAL
	{
		//One instance of the Dal_imp
		private static Dal_imp instance;
		private Dal_imp()
		{
			//t++
		}
		//return one instance of the DAL
		public static Dal_imp Instance
		{
			get
			{
				if(instance == null)
				{
					instance = new Dal_imp();
				}
				return instance;
			}
		}


		//Create
		//Use at least 4 Linq to object expressions for the purposes of queries.
		//Add a customer request 
		public void AddCustomer(GuestRequest request)
		{

			//Determines whether any element of a sequence exists or satisfies a condition.
			//https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.any?view=netcore-3.1
			//check if the id already exist
			if (DS.DataSource.guest_request_list.Any(item => item.GuestRequestKey == request.GuestRequestKey))
			{
				throw new ArgumentException("Hosting");
			}
			//assign the item a unique id
			request.GuestRequestKey = ++DS.DsConfiguration.st_request_key;
			//add new item to list
			DataSource.guest_request_list.Add(request);
			return;

		}

		//Add a hosting unit
		public void AddHostingUnit(HostingUnit unit)
		{
			//check if the id already exist
			if (DataSource.hosting_unit_list.Any(item => item.HostingUnitKey == unit.HostingUnitKey))
			{
				throw new ArgumentException("Hosting");
			}
			//assign the item a unique id
			unit.HostingUnitKey = ++DS.DsConfiguration.st_unit_key;
			//add new item to list
			DataSource.hosting_unit_list.Add(unit);
			return;
		}

		//Add an order
		public void AddOrder(Order order)
		{
			//check if order 1d already exist
			if (DataSource.order_list.Any(item => item.OrderKey == order.OrderKey))
			{
				throw new ArgumentException("Hosting");
			}
			//assign the item a unique id
			order.OrderKey = ++DS.DsConfiguration.st_order_key;
			//add new item to list
			DataSource.order_list.Add(order);
			return;
		}

		//add host
		public void AddHost(Host host)
		{
			//Search if Host id already exist
			if (DataSource.host_list.Any(item => item.HostKey == host.HostKey))
			{
				throw new ArgumentException("Hosting");
			}
			//assign the item a unique id
			host.HostKey = ++DS.DsConfiguration.st_host_key;
			//add new item to list
			DataSource.host_list.Add(host);
			return;
		}

		//Read Deep copy
		//Get a list of all accommodation units
		public List<HostingUnit> GetListUnits()
		{
			//https://stackoverflow.com/questions/14007405/how-create-a-new-deep-copy-clone-of-a-listt
			//How to create a new deep copy (clone) of a List<T>
			//whatever item was in the original list place it into a new list deep copy
			return DataSource.hosting_unit_list.Select(item => item).ToList();
		}
		//Receive a list of all customer requests .
		public List<GuestRequest> GetListCustomers()
		{
			//whatever item was in the original list place it into a new list deep copy
			return DataSource.guest_request_list.Select(item => item).ToList();
		}
		//Receive a list of all orders
		public List<Order> GetListOrders()
		{
			//whatever item was in the original list place it into a new list deep copy
			return DataSource.order_list.Select(item => item).ToList();
		}
		//Receives a list of all existing bank branches in Israel
		public List<BankBranch> GetListBanks()
		{
			//whatever item was in the original list place it into a new list deep copy
			return DataSource.bank_branch_list.Select(item => item).ToList();
		}
		//get list of host
		public List<Host> GetListHost()
		{
			//whatever item was in the original list place it into a new list deep copy
			return DataSource.host_list.Select(item => item).ToList();
		}
		//get the configuration 
		public Configuration GetConfigurations()
		{
			//whatever item was in the original list place it into a new list deep copy
			return DataSource.be_configurations;
		}

		//update
		//Customer request update  (status change) .
		public void CustomerRequestUpdate(GuestRequest request)
		{
			//https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/linq-to-objects
			//Linq to objects return null if item is not found
			//find the obj if it exist if not return null
			var matchQuery = (from item in DataSource.guest_request_list
							  where item.GuestRequestKey == request.GuestRequestKey
							  select item).FirstOrDefault();
			//update the item in the list
			if(matchQuery != null)
			{
				matchQuery = request;
				return;
			}
			throw new ArgumentException("Hosting");
		}
		//Hosting unit update
		public void UpdateHostingUnit(HostingUnit unit)
		{
			//find the obj if it exist if not return null
			var matchQuery = (from item in DataSource.hosting_unit_list
							  where item.HostingUnitKey == unit.HostingUnitKey
							  select item).FirstOrDefault();
			//update the item in the list
			if (matchQuery != null)
			{
				matchQuery = unit;
				return;
			}
			throw new ArgumentException("Hosting");
		}
		//Order Update (Status Change)
		public void OrderUpdate(Order order)
		{
			//find the obj if it exist if not return null
			var matchQuery = (from item in DataSource.order_list
							  where item.OrderKey == order.OrderKey
							  select item).FirstOrDefault();
			//update the item in the list
			if (matchQuery != null)
			{
				matchQuery = order;
				return;
			}
			throw new ArgumentException("Hosting");
		}
		//update host
		public void HostUpdate(Host host)
		{
			//find the obj if it exist if not return null
			var matchQuery = (from item in DataSource.host_list
							  where item.HostKey == host.HostKey
							  select item).FirstOrDefault();
			//update the item in the list
			if (matchQuery != null)
			{
				matchQuery = host;
				return;
			}
			throw new ArgumentException("Hosting");
		}
		
		//update the configuration 
		public void ConfigurationUpdate(Configuration config)
		{
			//need to figure out how to update the configuration  
			throw new ArgumentException("Hosting");
		}
		

		//Delete 
		//Delete a hosting unit
		public void DeleteHostingUnit(int id)
		{
			//search for item to remove if it exist 
			int count = DataSource.hosting_unit_list.RemoveAll(obj => obj.HostingUnitKey == id);
			if(count == 0)
			{
				throw new ArgumentException ("Hosting Unit ", id.ToString());
			}	
		}
		//delete host
		public void DeleteHost(int id)
		{
			//search for item to remove if it exist 
			int count = DataSource.host_list.RemoveAll(obj => obj.HostKey == id);
			if (count == 0)
			{
				throw new ArgumentException("Host ", id.ToString());
			}
		}
		//delete order
		public void DeleteOrder(int id)
		{
			//search for item to remove if it exist 
			int count = DataSource.order_list.RemoveAll(obj => obj.OrderKey == id);
			if (count == 0)
			{
				throw new ArgumentException("Order ", id.ToString());
			}
		}
		//delete request
		public void DeleteRequest(int id)
		{
			//search for item to remove if it exist 
			int count = DataSource.guest_request_list.RemoveAll(obj => obj.GuestRequestKey == id);
			if (count == 0)
			{
				throw new ArgumentException("Request ", id.ToString());
			}
		}

		
	}*/
}
