using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Classes
{
    public class VendingMachine
    {
        private Dictionary<string, Inventory> inventory = new Dictionary<string, Inventory>();
        public Dictionary<string, int> salesDictionary = new Dictionary<string, int>();


        private int currentBalance;
        public int CurrentBalance
        {
            get { return currentBalance; }
        }

        private int grossSales = 0;

        public int GrossSales
        {
            get { return grossSales; }
        }


        public void Stock()
        { 
            string directory = Environment.CurrentDirectory;
            string filename = "vendingmachine.csv";
            string fullPath = Path.Combine(directory, filename);
            string key;
            int price;
            string productName;

            using (StreamReader sr = new StreamReader(fullPath))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] item = line.Split('|');
                    key = item[0];
                    productName = item[1];
                    price = (int)(double.Parse(item[2]) * 100);
                    Inventory currentInventory = new Inventory();
                    currentInventory.Name = productName;
                    currentInventory.Price = price;
                    currentInventory.Quantity = 5;
                    inventory[key] = currentInventory;
                }
            }
        }
        public void InitSalesReport()
        {
            foreach (KeyValuePair<string, Inventory> kvp in inventory)
            {
                salesDictionary[kvp.Value.Name] = 0;
            }
        }

        LogList auditLog = new LogList();
        public string Purchase(string SlotID)
        {
            if (inventory.ContainsKey(SlotID))
            {
                if (currentBalance >= inventory[SlotID].Price)
                {
                    if (inventory[SlotID].Quantity > 0)
                    {
                        inventory[SlotID].Quantity = inventory[SlotID].Quantity - 1;
                        this.SubractMoney(inventory[SlotID].Price);
                        auditLog.TimeStamp = DateTime.Now.ToString();
                        auditLog.Name = inventory[SlotID].Name;
                        auditLog.Price = inventory[SlotID].Price;
                        auditLog.Slot = SlotID;
                        auditLog.RunningBalance = currentBalance;
                        this.Audit(auditLog);
                        grossSales = grossSales + inventory[SlotID].Price;
                        salesDictionary[inventory[SlotID].Name] = salesDictionary[inventory[SlotID].Name] + 1;
                        return $"Enjoy your {inventory[SlotID].Name}!";
                    }
                    else
                    {
                        return "Sorry, that item is SOLD OUT!";
                    }
                }
                return "Insufficient balance to purchase item";
            }
            else
            {
                return "Invalid Selection";
            }
        }




        public void Audit(LogList newAuditLog)
        {
            string directory = Environment.CurrentDirectory;
            string filename = "TransactionLog.txt";
            string fullPath = Path.Combine(directory, filename);
            using (StreamWriter sw = new StreamWriter(fullPath, true))
            {
                sw.WriteLine(newAuditLog.TimeStamp + " " + newAuditLog.Name + " " + newAuditLog.Slot + " " + newAuditLog.Price + " " + newAuditLog.RunningBalance);
            }
        }


        int centsBalanceToAdd;
        public void AddMoney(int dollars)
        {
            centsBalanceToAdd = dollars * 100;
            currentBalance = currentBalance + centsBalanceToAdd;
        }

        public void SubractMoney(int cents)
        {
            currentBalance = currentBalance - cents;
        }

        public Dictionary<string, Inventory> GetInventory()
        {
            return inventory;
        }

        public Dictionary<string, int> GetSalesDict()
        {
            return salesDictionary;
        }

        public VendingMachine()
        {
            string directory = Environment.CurrentDirectory;
            string filename = "TransactionLog.txt";
            string fullPath = Path.Combine(directory, filename);

            currentBalance = 0;
            using(StreamWriter sw= new StreamWriter(fullPath,false))
            {
                sw.WriteLine("DateTime    Product   Slot   AmountAccepted    ChangeTendered");
            }
        }



        public int GetQuantityForSlot(string slotID)
        {
            return inventory[slotID].Quantity;
        }

        
    }   
}
