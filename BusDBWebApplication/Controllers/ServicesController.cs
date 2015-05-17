using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BusDBWebApplication.DataBase;
using BusDBWebApplication.Models;

namespace BusDBWebApplication.Controllers
{
    [RequireHttps]
    [Authorize]
    
    public class ServicesController : Controller
    {
        private Bus_StationEntities db = new Bus_StationEntities();

        // GET: Services
        public async Task<ActionResult> Index()
        {
            var services = db.Services.Include(s => s.Routes);
            return View(await services.ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<ActionResult> Details(int? service_id, int? route_id, int? from, int? where)
        {
            if (service_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Services services = await db.Services.FindAsync(service_id, route_id, from, where);
            if (services == null)
            {
                return HttpNotFound();
            }
            return View(services);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            
            var service = new ServiceCreateViewModel
            {
                Buses = db.Buses
                    .Select(c => new BusSelectViewModel
                    {
                        bus_id = c.bus_id,
                        brand = c.brand,
                        number_of_seats = c.number_of_seats,
                        IsSelected = false
                    })
                    .ToList()
            };
            ViewBag.route_id = new SelectList(db.Routes, "route_id", "route");
            return View(service);
        }

        // POST: Services/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ServiceCreateViewModel services, [Bind(Include = "route_id")] Tickets tickets)
        {
            if (ModelState.IsValid)
            
            {
                var route = db.Routes.First(x => x.route_id == tickets.route_id);
                
                Services service = new Services
                {
                    route_id = tickets.route_id,
                    from = route.from,
                    where = route.where,
                    service_number = services.service_number,
                    departure_time = services.departure_time,
                    arrival_time = services.arrival_time,
                    Buses = new List<Buses>()
                };

                foreach (var selectedBus
                    in services.Buses.Where(c => c.IsSelected))
                {
                    Buses bus = new Buses { bus_id = selectedBus.bus_id };
                    db.Buses.Attach(bus);

                    service.Buses.Add(bus);
                }

                db.Services.Add(service);
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ViewBag.route_id = new SelectList(db.Routes, "route_id", "route", services.route_id);
            return View(services);
        }

       

        // GET: Services/Edit/5
        
        public async Task<ActionResult> Edit(int service_id)
        {
            var data = db.Services
        .Where(s => s.service_id == service_id)
        .Select(s => new
        {
            ViewModel = new ServiceEditViewModel
            {
                service_id = s.service_id,
                route_id = s.route_id,
                from = s.from,
                where = s.where,
                service_number = s.service_number,
                departure_time = s.departure_time,
                arrival_time = s.arrival_time
            },
            Buses = s.Buses.Select(c => c.bus_id)
        })
        .SingleOrDefault();

    if (data == null)
        return HttpNotFound();

    // Load all companies from the DB
    data.ViewModel.Buses = db.Buses
        .Select(c => new BusSelectViewModel
        {
            bus_id = c.bus_id,
            brand = c.brand,
            number_of_seats = c.number_of_seats
        })
        .ToList();

    // Set IsSelected flag: true (= checkbox checked) if the company
    // is already related with the subscription; false, if not
    foreach (var c in data.ViewModel.Buses)
        c.IsSelected = data.Buses.Contains(c.bus_id);

    return View(data.ViewModel);
        }

        // POST: Services/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<ActionResult> Edit(ServiceEditViewModel services)
        {

            if (ModelState.IsValid)
            {
                var service = db.Services.Include(s => s.Buses)
                    .SingleOrDefault(s => s.service_id == services.service_id);

                if (service != null)
                {
                    // Update scalar properties like "Amount"
                    service.service_number = services.service_number;
                    service.departure_time = services.departure_time;
                    service.arrival_time = service.arrival_time;
                    // or more generic for multiple scalar properties
                    // _context.Entry(subscription).CurrentValues.SetValues(viewModel);
                    // But this will work only if you use the same key property name
                    // in ViewModel and entity

                    foreach (var bus in services.Buses)
                    {
                        if (bus.IsSelected)
                        {
                            if (!service.Buses.Any(
                                c => c.bus_id == bus.bus_id))
                            {
                                // if company is selected but not yet
                                // related in DB, add relationship
                                var addedBus = new Buses { bus_id = bus.bus_id };
                                db.Buses.Attach(addedBus);
                                service.Buses.Add(addedBus);
                            }
                        }
                        //else
                        //{
                        //    var removedBus = service.Buses
                        //       .SingleOrDefault(c => c.bus_id == bus.bus_id);
                        //    if (removedBus != null)
                        //        // if company is not selected but currently
                        //        // related in DB, remove relationship
                        //        service.Buses.Remove(removedBus);
                        //}
                    }

                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            

            return View(services);
        }

        // GET: Services/Delete/5
        public async Task<ActionResult> Delete(int? service_id, int? route_id, int? from, int? where)
        {
            if (service_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Services services = await db.Services.FindAsync(service_id, route_id, from, where);
            if (services == null)
            {
                return HttpNotFound();
            }
            return View(services);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int service_id, int route_id, int from, int where)
        {
            Services services = await db.Services.FindAsync(service_id, route_id, from, where);
            db.Services.Remove(services);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
