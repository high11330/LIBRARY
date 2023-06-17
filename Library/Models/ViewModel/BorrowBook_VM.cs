using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Models.ViewModel
{
    public class BorrowBook_VM
    {
        public Book_VM Book { get; set; }
        public List<Book_VM> BookList { get; set; }
    }
}