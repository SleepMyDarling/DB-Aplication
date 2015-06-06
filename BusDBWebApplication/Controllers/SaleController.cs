using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusDBWebApplication.DataBase;
using System.Net;
using System.Threading.Tasks;

namespace BusDBWebApplication.Controllers
{
    public class SaleController : Controller
    {
        private Bus_StationEntities db = new Bus_StationEntities();
        // GET: Sale
        public async Task<ActionResult> Index(DateTime date, int? from, int? where)
        {
            if (date == null|| from==null||where==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var nextDay = date.AddDays(1);
            IEnumerable<Services> services = db.Services.Where(x => x.from == from && x.where == where && x.departure_time >= date&&x.departure_time<nextDay);
            if (services == null)
            {
                return HttpNotFound();
            }
            return View(services);
        }
        [HttpGet]
        public ActionResult Search()
        {
            var defaultService = new Services() {departure_time = DateTime.Now };
            ViewBag.from = new SelectList(db.Cities, "city_id", "title",defaultService.from);
            ViewBag.where = new SelectList(db.Cities, "city_id", "title", defaultService.where);
            return View(defaultService);
        }
        [HttpPost]
        public ActionResult Search([Bind(Include = "departure_time,from,where")] Services services)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Index", new { date = services.departure_time, from = services.from, where = services.where });
            }
            return View();
        }
        [HttpGet]
        public ActionResult Passenger(int? service_id)
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Passenger([Bind(Include = "passenger_id,surname,name,patronymic,passport_series,passport_number")]Passengers passengers, int service_id)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<Passengers> passenger = db.Passengers.Where(x => x.name == passengers.name && x.surname == passengers.surname && x.patronymic == passengers.patronymic && x.passport_number == passengers.passport_number && x.passport_series == passengers.passport_series);
                if (passenger.Count()==0)
                {
                    db.Passengers.Add(passengers);
                }
                else { passengers.passenger_id = passenger.First(x=>x.name==passengers.name).passenger_id; }
                IEnumerable<Tickets> tickets = db.Tickets.Where(x => x.passenger_id == passengers.passenger_id && x.service_id == service_id);
                if (tickets.Count() == 0)
                {
                    Tickets ticket = new Tickets() { passenger_id = passengers.passenger_id, service_id = service_id, purchase_date = DateTime.Now };
                    db.Tickets.Add(ticket);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Search");

                }
                else passengers.message = "Вы уже купили билет на этот рейс, введите другие данные";
                
            }
            
            return View(passengers);
        }
    }
}