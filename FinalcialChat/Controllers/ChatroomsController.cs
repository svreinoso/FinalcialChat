using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FinalcialChat.Models;

namespace FinalcialChat.Controllers
{
    [Authorize(Roles = RolesTypes.Admin)]
    public class ChatroomsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Chatrooms
        public ActionResult Index()
        {
            return View(db.Chatrooms.ToList());
        }

        // GET: Chatrooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chatroom chatroom = db.Chatrooms.Find(id);
            if (chatroom == null)
            {
                return HttpNotFound();
            }
            return View(chatroom);
        }

        // GET: Chatrooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Chatrooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CreatedBy,ModifiedBy,CreatedDate,ModifiedDate")] Chatroom chatroom)
        {
            if (ModelState.IsValid)
            {
                db.Chatrooms.Add(chatroom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chatroom);
        }

        // GET: Chatrooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chatroom chatroom = db.Chatrooms.Find(id);
            if (chatroom == null)
            {
                return HttpNotFound();
            }
            return View(chatroom);
        }

        // POST: Chatrooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CreatedBy,ModifiedBy,CreatedDate,ModifiedDate")] Chatroom chatroom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chatroom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chatroom);
        }

        // GET: Chatrooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chatroom chatroom = db.Chatrooms.Find(id);
            if (chatroom == null)
            {
                return HttpNotFound();
            }
            return View(chatroom);
        }

        // POST: Chatrooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chatroom chatroom = db.Chatrooms.Find(id);
            db.Chatrooms.Remove(chatroom);
            db.SaveChanges();
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
