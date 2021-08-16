using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BE
{
	[Serializable]
	public class GuestRequest
	{

		//Hosting Request Number - Unique ID (8 digits running code)
		public int GuestRequestKey
		{
			get;
			set;
		}

		//Customer first name
		public string GuestPrivateName
		{
			get;
			set;
		}

		//The family name
		public string GuestFamilyName
		{
			get;
			set;
		}

		//Email address
		public string GuestMailAddress
		{
			get;
			set;
		}

		//Hosting Application Status
		public Statuses RequestStatus
		{
			get;
			set;
		}

		//Registration Date
		public DateTime RegistrationDate
		{
			get;
			set;
		}

		//A desirable date for starting the holiday
		public DateTime RequestEntryDate
		{
			get;
			set;
		}

		//A desirable date for ending the holiday
		public DateTime RequestReleaseDate
		{
			get;
			set;
		}

		//The desired region in Israel
		public Areas RequestArea
		{
			get;
			set;
		}

		//Type of accommodation unit desired
		public Types RequestType
		{
			get;
			set;
		}
		//Number of adults
		public int RequestAdult
		{
			get;
			set;
		}

		//Number of children
		public int RequestChildren
		{
			get;
			set;
		}

		//Is interested in the pool
		public Decision RequestPool
		{
			get;
			set;
		}

		//Are you interested in Jacuzzi (not required)
		public Decision RequestJacuzzi
		{
			get;
			set;
		}

		//Would you like a garden (not required)
		public Decision RequestGarden
		{
			get;
			set;
		}
	
		//Is interested in children's attractions
		public Decision RequestChildAttraction
		{
			get;
			set;
		}
	}
}

	
