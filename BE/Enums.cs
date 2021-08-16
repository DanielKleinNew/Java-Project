using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
	//All Enum (Regions, Status, etc.)
	public enum Areas {All,North,South,Center,Jerusalem, TelAviv};
	public enum Decision { Necessary, Uninterested, Possible};
	public enum DecisionUnit { Yes,No};
	public enum Statuses { NotAddressed, SentEmail, ClosedForCustomerUnresponsiveness, ClosedForCustomerResponse };
	public enum Types { Rooms,Hotel,Camping,Etc};
	public enum CollectionClearance {Yes,No};
	}
