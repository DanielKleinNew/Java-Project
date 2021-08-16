using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BE
{
    [Serializable]
    public class Order
    {
        //The accommodation unit ID number
        public int OrderUnitKey
        {
            get;
            set;
        }
        
        //Dr. identification number of the Seth accommodation
        public int OrderRequestKey
        {
            get;
            set;
        }
        
        //Order ID number
        public int OrderKey
        {
            get;
            set;
        }
        
        //Order status
        public Statuses OrderStatus
        {
            get;
            set;
        }
        
        //Date of order creation
        public DateTime OrderCreation
        {
            get;
            set;
        }
        
        //Email Delivery Date to Customer
        public DateTime EmailDelivery
        {
            get;
            set;
        }
        
        //Order Constructor
        public Order()
        {

        }
    }
}
