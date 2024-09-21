using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Topscholars.Models;

namespace Topscholars.Controllers
{
    public class AdminController : Controller
    {
        topscholarsEntities db = new topscholarsEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageFaculty()
        {
            try
            {
                IEnumerable<Faculty> faculty = db.Faculty;
                return View(faculty);
            }
            catch (Exception e)
            {
                return Redirect(Url.Action("Error", "Error"));
            }
        }

        public ActionResult ViewFaculty(int id)
        {
            try
            {
                var faculty = (from x in db.Faculty
                              where x.FacultyId == id
                              select new FacultyModel
                              {
                                  ID = x.FacultyId,
                                  Department = x.Department,
                                  Email = x.Users.Email,
                                  Name = x.Users.FullName,
                                  JoiningDate = (DateTime)x.JoiningDate
                              }).FirstOrDefault();
                return View(faculty);
            }
            catch (Exception e)
            {
                return Redirect(Url.Action("Error", "Error"));
            }
        }

        public ActionResult EditFaculty(int? id)
        {
            try
            {
                if (id != null)
                {
                    var faculty = (from x in db.Faculty
                                  where x.FacultyId == id
                                  select new FacultyModel
                                  {
                                      ID = x.FacultyId,
                                      Department = x.Department,
                                      Email = x.Users.Email,
                                      Name = x.Users.FullName,
                                      JoiningDate = (DateTime)x.JoiningDate
                                  }).FirstOrDefault();
                    return View(faculty);
                }
                return View();
            }
            catch (Exception e)
            {
                return Redirect(Url.Action("Error", "Error"));
            }
        }

        [HttpPost]
        public ActionResult AddFaculty(FacultyModel model)
        {
            try
            {
                var faculty = db.Faculty.Find(model.ID);
                if (faculty != null)
                {
                    faculty.Department = model.Department;
                    db.Entry(faculty).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    var users = db.Users.Find(faculty.UserId);
                    users.Email = model.Email;
                    users.FullName = model.Name;
                    db.Entry(users).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Updated";
                }
                else
                {
                    Users newusers = new Users
                    {
                        FullName = model.Name,
                        Email = model.Email,
                        Role = "Faculty",
                        Password = "",
                    };
                    db.Users.Add(newusers);
                    db.Entry(newusers).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    var users = db.Users.Where(x => x.Email == newusers.Email).FirstOrDefault();
                    Faculty newfaculty = new Faculty
                    {
                        Department = model.Department,
                        JoiningDate = DateTime.Now,
                        UserId = users.UserId,
                    };
                    db.Faculty.Add(newfaculty);
                    db.Entry(newfaculty).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    TempData["Message"] = "Created";
                }
                return RedirectToAction("ManageFaculty");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error","Error",e);
            }
        }

        public ActionResult DeleteFaculty(int id)
        {
            try
            {
                var faculty = db.Faculty.Find(id);
                var users = db.Users.Find(faculty.UserId);
                db.Faculty.Remove(faculty);
                db.Entry(faculty).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                db.Users.Remove(users);
                db.Entry(users).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                TempData["Message"] = "Deleted";
                return RedirectToAction("ManageFaculty");
            }
            catch (Exception e)
            {
                return Redirect(Url.Action("Error", "Error"));
            }

        }

        public ActionResult Timetable()
        {
            try
            {
                IEnumerable<Topscholars.Models.Faculty> faculty = db.Faculty;
                return View(faculty);
            }
            catch (Exception e)
            {
                return Redirect(Url.Action("Error", "Error"));
            }
        }
        public ActionResult AddEntry() { return View(); }
        public ActionResult EditEntry(int timetable_id) { return View(); }
        public ActionResult DeleteEntry() { return View(); }

        public ActionResult ManageStudent()
        {
            try
            {
                IEnumerable<Topscholars.Models.Faculty> faculty = db.Faculty;
                return View(faculty);
            }
            catch (Exception e)
            {
                return Redirect(Url.Action("Error", "Error"));
            }
        }
        public ActionResult ViewStudents() { return View(); }
        public ActionResult EditStudent() { return View(); }
        public ActionResult DeleteStudent() { return View(); }

        public ActionResult Feedback()
        {
            try
            {
                IEnumerable<Feedback> feedback = db.Feedback;
                return View(feedback);
            }
            catch (Exception e)
            {
                return Redirect(Url.Action("Error", "Error"));
            }
        }
        public ActionResult ProcessedFeedback() { return View(); }
        public ActionResult DeleteFeedback() { return View(); }
    }
}