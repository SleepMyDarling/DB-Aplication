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

            ViewBag.route_id = new SelectList(db.Routes, "route_id", "route");
            return View();
        }

        // POST: Services/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "service_id,route_id,from,where,service_number,departure_time,arrival_time")] Services services)
        {
            if (ModelState.IsValid)
            {
                //FixServices(services);
                db.Services.Add(services);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.route_id = new SelectList(db.Routes, "route_id", "route", services.route_id);

            return View(services);
        }

        private void FixServices(Services services)
        {
            var route = db.Routes.First(x => x.route_id == services.route_id);
            if(route!=null){
                services.from = route.from;
                services.where = route.where;
            }
        }

        // GET: Services/Edit/5
        [ImportModelStateFromTempData]
        public async Task<ActionResult> Edit(int? service_id, int? route_id, int? from, int? where)
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

            ICollection<Buses> selectedBuses = new List<Buses>();
            foreach(var bus in db.Buses)
            {
                
                if(services.Buses.FirstOrDefault(x=>x.bus_id == bus.bus_id) != null)
                {
                    bus.IsSelected = true;
                }
                selectedBuses.Add(bus);
                    
            }

            ServiceEditViewModel model = new ServiceEditViewModel()
            {
                Service = services,
                Buses = selectedBuses.ToList()
            };
            ViewBag.route_id = new SelectList(db.Routes, "route_id", "route_id", services.route_id);
            return View(model);
        }

        // POST: Services/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ExportModelStateToTempData]
        public async Task<ActionResult> Edit(ServiceEditViewModel services)
        {

            if (ModelState.IsValid)
            {
                var selectedBuses = services.Buses.Where(x => x.IsSelected);
                
                if(selectedBuses != null)
                    services.Service.Buses = selectedBuses.ToList();

                db.Entry(services.Service).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.route_id = new SelectList(db.Routes, "route_id", "route_id", services.Service.route_id);
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
