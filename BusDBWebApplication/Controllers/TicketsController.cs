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

namespace BusDBWebApplication.Controllers
{
    [RequireHttps]
    [Authorize]
    
    public class TicketsController : Controller
    {
        private Bus_StationEntities db = new Bus_StationEntities();

        // GET: Tickets
        public async Task<ActionResult> Index()
        {
            var tickets = db.Tickets.Include(t => t.Passengers).Include(t => t.Services);
            return View(await tickets.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = await db.Tickets.FindAsync(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            ViewBag.passenger_id = new SelectList(db.Passengers, "passenger_id", "surname");
            ViewBag.service_id = new SelectList(db.Services, "service_id", "service_id");
            return View();
        }

        // POST: Tickets/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ticket_id,service_id,route_id,from,where,passenger_id,purchase_date")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(tickets);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.passenger_id = new SelectList(db.Passengers, "passenger_id", "surname", tickets.passenger_id);
            ViewBag.service_id = new SelectList(db.Services, "service_id", "service_id", tickets.service_id);
            return View(tickets);
        }

        // GET: Tickets/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = await db.Tickets.FindAsync(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            ViewBag.passenger_id = new SelectList(db.Passengers, "passenger_id", "surname", tickets.passenger_id);
            ViewBag.service_id = new SelectList(db.Services, "service_id", "service_id", tickets.service_id);
            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ticket_id,service_id,route_id,from,where,passenger_id,purchase_date")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tickets).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.passenger_id = new SelectList(db.Passengers, "passenger_id", "surname", tickets.passenger_id);
            ViewBag.service_id = new SelectList(db.Services, "service_id", "service_id", tickets.service_id);
            return View(tickets);
        }

        // GET: Tickets/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = await db.Tickets.FindAsync(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tickets tickets = await db.Tickets.FindAsync(id);
            db.Tickets.Remove(tickets);
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
