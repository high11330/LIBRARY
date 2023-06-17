using Library.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class RetrunBookController : BaseController
    {
        #region 還書畫面
        public ActionResult Index()
        {
            //僅能看見自己的紀錄且書本出借中
            BorrowBook_VM Model = new BorrowBook_VM();
            Model.Book = new Book_VM();
            Model.BookList = db.Database.SqlQuery<Book_VM>("EXEC GetBorrowingRecord @UserId, @Status",
                                                           new SqlParameter("@UserId", UserId),
                                                           new SqlParameter("@Status", "出借中")).ToList();
            return View(Model);
        }
        #endregion

        #region 執行還書
        [HttpPost]
        public JsonResult Retrun(string inventoryid)
        {
            string error = "";
            try
            {
                db.Database.ExecuteSqlCommand("EXEC RetrunBook @UserId, @InventoryId",
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