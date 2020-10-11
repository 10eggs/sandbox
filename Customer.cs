using System;
using System.Collections.Generic;
using System.Text;

namespace DelegatesLambdasEvents
{
    class Customer
    {

        public string CustomerID { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public List<Order> Orders { get; set; }
    }


}
