using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Capstone.Classes
{
    public class Inventory :VMItem
    {
        private int quantity;
 
        public Inventory()
        {
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
    }
}


