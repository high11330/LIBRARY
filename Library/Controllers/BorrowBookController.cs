using Library.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class BorrowBookController : BaseController
    {
        // 借書畫面
        public ActionResult Index()
        {
            BorrowBook_VM Model = new BorrowBook_VM();
            Model.Book = new Book_VM();
            Model.BookList = db.Database.SqlQuery<Book_VM>("EXEC GetBookList @ISBN",
                                                           new SqlParameter("@ISBN", DBNull.Value)).ToList();
            return View(Model);
        }

        #region 執行借書
        [HttpPost]
        public JsonResult Borrow(string inventoryid)
        {
            string error = "";
            try
            {
                db.Database.ExecuteSqlCommand("EXEC BorrowBook @UserId, @InventoryId",
                                               new SqlParameter("@UserId", UserId),
                                               new SqlParameter("@InventoryId", inventoryid));
                return Json(new { error });
            }
            catch (Exception ex)
            {
                error = ex.Message.ToString();
            }

            return Json(new { error });
        }
        #endregion
    }
}