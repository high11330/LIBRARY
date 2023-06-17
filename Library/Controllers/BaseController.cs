using Library.Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    [SessionTimeout]
    public class BaseController : Controller
    {
        public LibraryEntities db = new LibraryEntities();
        public string UserId => Session["UserId"] == null ? "" : Session["UserId"].ToString();
        public string UserName => Session["UserName"] == null ? "" : Session["UserName"].ToString();
    }

    #region SessionTimeout相關
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["UserId"] == null)
            {
                //因為規劃放在BaseController, 所有Action都會執行SessionTimeout檢查
                //如果有要略過的,請增加IgnoreSessionTimeout屬性
                if (ShouldRun(filterContext))
                {
                    filterContext.Controller.TempData["SessionTimeout"] = "NoSession";

                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        //因為ajax通常只能由各Client端處理, 增加表頭讓Ajax在得到response時判斷是否要回首頁
                        filterContext.HttpContext.Response.AppendHeader("sessionstatus", "timeout");
                        filterContext.HttpContext.Response.End();
                    }
                    filterContext.Result = new RedirectResult("~/Login/Index");
                }
                else
                    base.OnActionExecuting(filterContext);
            }
            else
                base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// 是否忽略Session Timeout檢查
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        private bool ShouldRun(ActionExecutingContext filterContext)
        {
            var ignoreAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(IgnoreSessionTimeoutAttribute), false);
            if (ignoreAttributes.Length > 0)
                return false;

            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class IgnoreSessionTimeoutAttribute : ActionFilterAttribute
    {

    }
    #endregion
}