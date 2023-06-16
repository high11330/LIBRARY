using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class LibraryController : BaseController
    {
        #region 登入後首頁
        public ActionResult Index()
        {
            return View();
        }
        #endregion
    }
}