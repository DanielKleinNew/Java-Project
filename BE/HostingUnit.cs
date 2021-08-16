using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
//to add
using System.Xml.Serialization;
using System.IO;
namespace BE
{
	[Serializable]
	public class HostingUnit
	{

		//Hosting Unit Number - Unique ID(8 digit unique run code)
		[XmlElement("HostingUnitKey")]
		public int HostingUnitKey
		{
			get;
			set;
		}
		//Unit owner
		
		public Host UnitOwner
		{
			get;
			set;
		}
		//Hosting unit name
		public string HostingUnitName
		{
			get;
			set;
		}
		//Calendar
		[XmlIgnore]
		public bool[,] UnitDiary 
		{ 
			get;
			set; 
		}
		[XmlArray("Calendar")]
		public bool[] UnitDiaryTo
		{
			get { return UnitDiary.Flatten(); }
			set { UnitDiary = value.Expand(12); }
		}
		
		//Indexer
		public bool this[int i, int j]
		{
			get { return UnitDiary[i, j]; }
			set { UnitDiary[i, j] = value; }
		}
		
		//Price
		public float UnitPrice
		{
			get;
			set;
		}
		
		//Area
		public Areas UnitArea
		{
			get;
			set;
		}
		
		//Type of accommodation unit desired
		public Types UnitType
		{
			get;
			set;
		}
		
		//Number of adults
		public int UnitAdult
		{
			get;
			set;
		}

		//Number of children
		public int UnitChildren
		{
			get;
			set;
		}

		//Is interested in the pool
		public DecisionUnit UnitPool
		{
			get;
			set;
		}

		//Are you interested in Jacuzzi (not required)
		public DecisionUnit UnitJacuzzi
		{
			get;
			set;
		}

		//Would you like a garden (not required)
		public DecisionUnit UnitGarden
		{
			get;
			set;
		}
		
		//Is interested in children's attractions
		public DecisionUnit UnitChildAttraction
		{
			get;
			set;
		}

		public HostingUnit()
		{
			UnitDiary = new bool[12, 31];
		}

		public string Location
		{
			get;
			set;
		}


	}
}
