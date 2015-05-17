﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusDBWebApplication.Models
{
    public class ServiceCreateViewModel
    {
        public int route_id;
        //public int from;
        //public int where;
        public int service_number;
        public DateTime departure_time;
        public DateTime arrival_time;
        public IEnumerable<BusSelectViewModel> Buses { get; set; }
    }
}