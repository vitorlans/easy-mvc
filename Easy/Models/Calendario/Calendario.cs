using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Easy.Models
{
    public class Calendario
    {
        public string id { get; set;}
         public string title { get; set;}
        public string url { get; set;}
        public string start { get; set;}
        public string end { get; set; }
        public string key { get; set; }
        public string allday { get; set; }


        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
    }
}