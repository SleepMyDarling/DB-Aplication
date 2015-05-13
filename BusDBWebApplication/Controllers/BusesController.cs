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
    
    public class BusesController : Controller
    {
        private Bus_StationEntities db = new Bus_StationEntities();

        // GET: Buses
        public async Task<ActionResult> Index()
        {
            return View(await db.Buses.ToListAsync());
        }

        // GET: Buses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Buses buses = await db.Buses.FindAsync(id);
            if (buses == null)
            {
                return HttpNotFound();
            }
            return View(buses);
        }

        // GET: Buses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Buses/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "bus_id,brand,number_of_seats")] Buses buses)
        {
            if (ModelState.IsValid)
            {
                db.Buses.Add(buses);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(buses);
        }

        // GET: Buses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Buses buses = await db.Buses.FindAsync(id);
            if (buses == null)
            {
                return HttpNotFound();
            }
            return View(buses);
        }

        // POST: Buses/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "bus_id,brand,number_of_seats")] Buses buses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(buses).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(buses);
        }

        // GET: Buses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Buses buses = await db.Buses.FindAsync(id);
            if (buses == null)
            {
                return HttpNotFound();
            }
            return View(buses);
        }

        // POST: Buses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Buses buses = await db.Buses.FindAsync(id);
            db.Buses.Remove(buses);
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
