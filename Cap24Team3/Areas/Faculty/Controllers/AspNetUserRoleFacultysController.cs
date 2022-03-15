using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cap24Team3.Models;

namespace Cap24Team3.Areas.Faculty.Controllers
{
    public class AspNetUserRoleFacultysController : Controller
    {
        private Cap24 db = new Cap24();
        // GET: AspNetRoleUserAdmin/Create
        public ActionResult Create(string roleId)
        {
            ViewBag.Role = db.AspNetRoles.Find(roleId);
            ViewBag.Users = new SelectList(db.AspNetUsers, "Id", "UserName");
            return View();
        }

        [HttpPost]
    //    [ValidateAntiForgeryToken]
        public ActionResult Create(string RoleId, string UserId)
        {
            var role = db.AspNetRoles.Find(RoleId);
            var user = db.AspNetUsers.FirstOrDefault(u => u.Email == UserId);
            role.AspNetUsers.Add(user);
            db.Entry(role).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "AspNetRoleFacultys");
        }


        // GET: AspNetRolesAdmin/Delete/5
        public ActionResult Delete(string RoleId, string UserId)
        {
            var role = db.AspNetRoles.Find(RoleId);
            var user = db.AspNetUsers.Find(UserId);

            role.AspNetUsers.Remove(user);
            db.Entry(role).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", "AspNetRoleFacultys");
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