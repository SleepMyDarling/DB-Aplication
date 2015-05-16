using BusDBWebApplication.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusDBWebApplication.Models
{
    public class ServiceEditViewModel
    {
        public Services Service { get; set; }
        public List<Buses> Buses { get; set; }
    }
}