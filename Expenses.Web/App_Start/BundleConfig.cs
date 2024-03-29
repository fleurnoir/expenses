﻿using System.Web;
using System.Web.Optimization;

namespace Expenses.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.fix.validate.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/calculator").Include(
                "~/Scripts/calculator.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/operation-edit").Include(
                "~/Views/Operation/edit.js"));

            bundles.Add(new ScriptBundle("~/bundles/operation-index").Include(
                "~/Views/Operation/index.js"));

            bundles.Add(new ScriptBundle("~/bundles/exchange-edit").Include(
                "~/Views/Exchange/edit.js"));

            bundles.Add(new ScriptBundle("~/bundles/debt-details").Include(
                "~/Views/Debt/details.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/calculator").Include(
                "~/Content/calculator.css"));

            bundles.Add (new StyleBundle ("~/Content/jquery-ui").Include (
                "~/Content/jquery-ui.css"
            ));

            bundles.Add (new StyleBundle ("~/Content/pagedlist").Include (
                "~/Content/PagedList.css"
            ));


            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
