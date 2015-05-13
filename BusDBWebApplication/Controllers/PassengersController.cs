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
    
    public class PassengersController : Controller
    {
        private Bus_StationEntities db = new Bus_StationEntities();

        // GET: Passengers
        public async Task<ActionResult> Index()
        {
            return View(await db.Passengers.ToListAsync());
        }

        // GET: Passengers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Passengers passengers = await db.Passengers.FindAsync(id);
            if (passengers == null)
            {
                return HttpNotFound();
            }
            return View(passengers);
        }

        // GET: Passengers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Passengers/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "passenger_id,surname,name,patronymic,passport_series,passport_number")] Passengers passengers)
        {
            if (ModelState.IsValid)
            {
                db.Passengers.Add(passengers);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(passengers);
        }

        // GET: Passengers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Passengers passengers = await db.Passengers.FindAsync(id);
            if (passengers == null)
            {
                return HttpNotFound();
            }
            return View(passengers);
        }

        // POST: Passengers/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "passenger_id,surname,name,patronymic,passport_series,passport_number")] Passengers passengers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(passengers).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(passengers);
        }

        // GET: Passengers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Passengers passengers = await db.Passengers.FindAsync(id);
            if (passengers == null)
            {
                return HttpNotFound();
            }
            return View(passengers);
        }

        // POST: Passengers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Passengers passengers = await db.Passengers.FindAsync(id);
            db.Passengers.Remove(passengers);
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
