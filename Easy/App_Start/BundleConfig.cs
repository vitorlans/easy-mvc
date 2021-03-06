﻿using System.Web;
using System.Web.Optimization;

namespace Easy
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/easy").Include(                       
                       "~/Scripts/bootstrap.js",
                       "~/Scripts/material.js",
                        "~/Scripts/ripples.js",
                        "~/Scripts/snackbar.js",
                        "~/Scripts/jquery.nouislider.js"));

             bundles.Add(new ScriptBundle("~/bundles/bootdt").Include(                       
                        "~/Scripts/bootstrap-datetimepicker.js",
                        "~/Scripts/bootstrap-datetimepicker.pt-BR.js",
                        "~/Scripts/bootstrap-modal.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/calendariojs").Include(
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/calendariojs/jquery-ui.custom.js",
                        "~/Scripts/calendariojs/bootstrapmodal.js",
                        "~/Scripts/calendariojs/moment.js",
                        "~/Scripts/calendariojs/fullcalendar.js",
                        "~/Scripts/calendariojs/lang-all.js",
                        "~/Scripts/calendariojs/lang/pt-br.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css/calendario").Include(
                                    "~/Content/themes/base/jquery-ui-calendar.css",
                                    "~/Content/css/fullcalendar.css",
                                    "~/Content/css/calendar.css"
                                     ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                                         "~/Content/css/bootstrap.css",
                                         "~/Content/css/sb-admin.css",
                                         "~/Content/css/sidebar.css",
                                         "~/Content/css/ripples.css",
                                         "~/Content/css/material-wfont.css",
                                         "~/Content/css/material-v1.css",
                                         "~/Content/css/snackbar.css",
                                         "~/Content/css/component.css",
                                         "~/Content/css/demo.css",
                                         "~/Content/css/normalize.css",
                                         "~/Content/css/bootstrap-datetimepicker.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}