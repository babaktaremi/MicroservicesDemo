using System;
using System.Collections.Generic;
using System.Text;

namespace AsyncCommunication.Shared
{
    public class Inventory
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductCount { get; set; }

        private Inventory()
        {

        }

        public static List<Inventory> DummyInventory = new()
        {
            new Inventory() { ProductCount = 0, ProductName = "Electric Board", Id = 1 },
            new() { ProductCount = 10, ProductName = "LCD", Id = 2 }
        };
    }

    public class InventoryQueryModel
    {
        public int Id { get; set; }
    }

    public class InventoryQueryResultModel
    {
        public string ProductName { get; set; }
        public int ProductCount { get; set; }
    }
}
