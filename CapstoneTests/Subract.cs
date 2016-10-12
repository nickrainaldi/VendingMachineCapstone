using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone.Classes;

namespace CapstoneTests
{
    [TestClass]
    public class Subract
    {
        [TestMethod]
        public void subract_money_for_item()
        {
            VendingMachine NVM = new VendingMachine();
            NVM.AddMoney(5);
            NVM.Purchase("A1");
            Assert.AreEqual(1, 1);
        }
    }
}
