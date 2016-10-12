using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.Classes;

namespace CapstoneTests
{
    [TestClass]
    public class PurchaseTest
    {
        [TestMethod]
        public void purchase_one_item()
        {
           
            VendingMachine NVM = new VendingMachine();

            NVM.Stock();
            NVM.AddMoney(5);
            NVM.Purchase("A1");
            Assert.AreEqual(4, NVM.GetQuantityForSlot("A1"));
           

        }
    }
}
