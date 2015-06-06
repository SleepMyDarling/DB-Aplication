using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusDBWebApplication.Models
{
    public class BusSelectViewModel
    {
        public int bus_id { get; set; }
        public string brand { get; set; }
        public int number_of_seats { get; set; }
        public bool IsSelected { get; set; }
        public string BusInfo { get { return String.Format("{0}-{1}", bus_id, brand); } }
    }
}