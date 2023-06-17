using Library.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class BookRecordController : BaseController
    {
        #region 借閱紀錄畫面
        public ActionResult Index()
        {
            //僅能看見自己的借閱紀錄
            BorrowBook_VM Model = new BorrowBook_VM();
            Model.Book = new Book_VM();
            Model.BookList = db.Database.SqlQuery<Book_VM>("EXEC GetBorrowingRecord @UserId, @Status",
                                                           new SqlParameter("@UserId", UserId),
                                                           new SqlParameter("@Status", DBNull.Value)).ToList();
            return View(Model);
        }
        #endregion
    }
}