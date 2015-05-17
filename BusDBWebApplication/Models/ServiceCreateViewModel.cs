using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BusDBWebApplication.Models
{
    public class ServiceCreateViewModel
    {
        public int route_id{get;set;}
        //public int from;
        //public int where;
        public int service_number { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime departure_time { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTime arrival_time { get; set; }
        public IEnumerable<BusSelectViewModel> Buses { get; set; }
    }
}