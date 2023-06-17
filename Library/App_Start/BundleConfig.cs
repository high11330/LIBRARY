using System.Web;
using System.Web.Optimization;

namespace Library
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            //// 準備好可進行生產時，請使用 https://modernizr.com 的建置工具，只挑選您需要的測試。
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            bundles.Add(new Bundle("~/bundles/js").Include(
                                   "~/Scripts/jquery-3.4.1.min.js",
                                   "~/Scripts/jquery.unobtrusive-ajax.min.js",
                                   "~/Scripts/jquery.validate.min.js",
                                   "~/Scripts/bootstrap.bundle.min.js",
                                   "~/Scripts/sweetalert2.all.min.js",
                                   "~/Scripts/all.min.js",
                                   "~/Scripts/axios.js",
                                   "~/Scripts/datatables.min.js",
                                   "~/Scripts/main.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                                        "~/Content/bootstrap.css",
                                        "~/Content/sweetalert2.min.css",
                                        "~/Content/all.min.css",
                                        "~/Content/datatables.min.css",
                                        "~/Content/site.css"));
        }
    }
}
