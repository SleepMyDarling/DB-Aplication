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
    
    public class RoutesController : Controller
    {
        private Bus_StationEntities db = new Bus_StationEntities();

        // GET: Routes
        public async Task<ActionResult> Index()
        {
            var routes = db.Routes.Include(r => r.FromWhence).Include(r => r.WhereAbouts);
            return View(await routes.ToListAsync());
        }

        // GET: Routes/Details/5
        public async Task<ActionResult> Details(int? route_id, int? from, int? where)
        {
            if (route_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Routes routes = await db.Routes.FindAsync(route_id, from, where);
            if (routes == null)
            {
                return HttpNotFound();
            }
            return View(routes);
        }

        // GET: Routes/Create
        public ActionResult Create()
        {
            ViewBag.from = new SelectList(db.Cities, "city_id", "title");
            ViewBag.where = new SelectList(db.Cities, "city_id", "title");
            return View();
        }

        // POST: Routes/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "route_id,from,where")] Routes routes)
        {
            if (ModelState.IsValid)
            {
                db.Routes.Add(routes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.from = new SelectList(db.Cities, "city_id", "title", routes.from);
            ViewBag.where = new SelectList(db.Cities, "city_id", "title", routes.where);
            return View(routes);
        }

        // GET: Routes/Edit/5
        public async Task<ActionResult> Edit(int? route_id, int? from, int? where)
        {
            if (route_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Routes routes = await db.Routes.FindAsync(route_id, from, where);
            if (routes == null)
            {
                return HttpNotFound();
            }
            ViewBag.from = new SelectList(db.Cities, "city_id", "title", routes.from);
            ViewBag.where = new SelectList(db.Cities, "city_id", "title", routes.where);
            return View(routes);
        }

        // POST: Routes/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "route_id,from,where")] Routes routes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(routes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.from = new SelectList(db.Cities, "city_id", "title", routes.from);
            ViewBag.where = new SelectList(db.Cities, "city_id", "title", routes.where);
            return View(routes);
        }

        // GET: Routes/Delete/5
        public async Task<ActionResult> Delete(int? route_id, int? from, int? where)
        {
            if (route_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Routes routes = await db.Routes.FindAsync(route_id, from, where);
            if (routes == null)
            {
                return HttpNotFound();
            }
            return View(routes);
        }

        // POST: Routes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int route_id, int from, int where)
        {
            Routes routes = await db.Routes.FindAsync(route_id, from, where);
            db.Routes.Remove(routes);
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
