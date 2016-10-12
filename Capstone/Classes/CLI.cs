using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Capstone.Classes
{
    public class CLI
    {
        VendingMachine vm = new VendingMachine();


        public void Run()
        {
            vm.Stock();
            vm.InitSalesReport();
            string input = "";
            string actionStatus = "";
            double dollarBalance;
            Console.Clear();
            while (input != "99")
            {
                Console.Clear();
                this.DisplayMenu();
                Console.WriteLine();
                Console.WriteLine(actionStatus);
                actionStatus = "";
                Console.WriteLine();
                dollarBalance = vm.CurrentBalance / 100.0;
                Console.WriteLine("Your current balance is " + dollarBalance.ToString("C"));
                Console.WriteLine("1) Add Money");
                Console.WriteLine("2) Purchase");
                Console.WriteLine("3) End Transaction");
                Console.WriteLine("99) I'm Finished");
                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        actionStatus = this.InsertMoney();
                        break;
                    case "2":
                        actionStatus = this.UserPurchase();
                        break;
                    case "3":
                        actionStatus = this.EndTransaction();
                        break;
                    case "0":
                        actionStatus = PrintSalesReport();
                        break;
                    case "99":
                        break;
                    default:
                        Console.WriteLine("Please select a valid option");
                        break;
                }
            }

        }
        public void DisplayMenu()
        {
            Dictionary<string, Inventory> vmInventory = vm.GetInventory();

            foreach (KeyValuePair<string, Inventory> kvp in vmInventory)
            {
                double dollarprice = kvp.Value.Price / 100.0;
                if (kvp.Value.Quantity == 0)
                {
                    Console.WriteLine(kvp.Key.PadLeft(25) + "-" + kvp.Value.Name + "-" + dollarprice.ToString("C") + "---" + "Sold Out!");
                }
                else
                {
                    Console.WriteLine(kvp.Key.PadLeft(25) + "-" + kvp.Value.Name + "-" + dollarprice.ToString("C") + "---" + kvp.Value.Quantity + " remaining");
                }
            }
        }

        public string UserPurchase()
        {
            Console.WriteLine("What Would you like to purchase? ");
            string customerSelection = Console.ReadLine().ToUpper();
            string purchaseStatus = vm.Purchase(customerSelection);
            return purchaseStatus;
        }

        public string InsertMoney()
        {
            try
            {
                Console.WriteLine("Please insert money - <$1,$5,$10,$20>");
                string insertedMoneyString = Console.ReadLine();
                int insertedMoney;
                if (insertedMoneyString.First() == '$')
                {
                    insertedMoney = int.Parse(insertedMoneyString.Substring(1));
                }
                else
                {
                    insertedMoney = int.Parse(insertedMoneyString);
                }
                if (insertedMoney == 1 || insertedMoney == 2 || insertedMoney == 5 || insertedMoney == 10 || insertedMoney == 20)
                {
                    vm.AddMoney(insertedMoney);
                    return $"${insertedMoney} added to your balance";
                }
                else if (insertedMoney == 50 || insertedMoney == 100)
                {
                    return "Sorry Big Spender, this machine only accepts bill up to $20";
                }
                return "Sorry, I do recognized the denomination of that bill";
            }
            catch
            {
                return "Could not complete transaction";
            }
        }

        public string EndTransaction()
        {
            Change userChange = new Change(vm.CurrentBalance);
            double changeToBeReturned = (double)vm.CurrentBalance / 100;
            vm.SubractMoney(vm.CurrentBalance);
            return $"You are returned {changeToBeReturned.ToString("C")} as {userChange.Quarters()} quarters, {userChange.Dimes()} dimes, and {userChange.Nickels()} nickels";
        }

        public string PrintSalesReport()
        {
            try
            {
                double grossDollars;
                string directory = Environment.CurrentDirectory;
                string filename = "Vendo-Matic-Sales_" + DateTime.Now.ToString("MM-dd-yy_HH-mm-sstt")+ ".csv";
                string fullPath = Path.Combine(directory, filename);

                using (StreamWriter sw = new StreamWriter(fullPath, false))
                {
                    sw.WriteLine("Item, Qty");
                    foreach (KeyValuePair<string, int> kvp in vm.GetSalesDict())
                    {
                        sw.WriteLine(kvp.Key + "," + kvp.Value.ToString());
                    }
                    grossDollars = vm.GrossSales / 100.00;
                    sw.WriteLine($"Gross sales = {grossDollars.ToString("C")}");
                }
                return "";
            }
            catch (Exception e)
            {
                return "Error writing report" + e.Message;
            }

        }

    }
}

