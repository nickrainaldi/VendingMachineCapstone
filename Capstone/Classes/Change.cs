using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Classes
{
    public class Change
    {
        private int quarters;
        private int dimes;
        private int nickels;
        private int amountInCents;

        public Change(int changeInput)
        {
            amountInCents = changeInput;

            quarters = amountInCents / 25;
            amountInCents = amountInCents % 25;

            dimes = amountInCents / 10;
            amountInCents = amountInCents % 10;

            nickels = amountInCents / 5;
            amountInCents = amountInCents % 5;
          }

        public int Quarters()
        {
            return quarters;


        }

        public int Dimes()
        {
            return dimes;
        }

        public int Nickels()
        {
            return nickels;

        }

    }

}


