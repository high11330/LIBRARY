using Library.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class SharedController : BaseController
    {
        #region 書本明細共用
        [HttpGet]
        public ActionResult BookDetail(string isbn)
        {
            Book_VM model = db.Database.SqlQuery<Book_VM>("EXEC GetBookList @ISBN",
                                                           new SqlParameter("@ISBN", isbn)).FirstOrDefault();
            return PartialView(model);
        }
        #endregion
    }
}