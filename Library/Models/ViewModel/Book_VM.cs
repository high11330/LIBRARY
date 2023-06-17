using Library.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Models.ViewModel
{
    public class Book_VM : Book
    {
        public string Status { get; set; }
        public int InventoryId { get; set; }
        public DateTime StoreTime { get; set; }
        public DateTime BorrowingTime { get; set; }
        public DateTime? ReturnTime { get; set; }
    }
}