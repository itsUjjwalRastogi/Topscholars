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
                return RedirectToAction("Error", "Error", e);
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
                return RedirectToAction("Error", "Error", e);
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
                return RedirectToAction("Error", "Error", e);
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
                return RedirectToAction("Error", "Error", e);
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
                return RedirectToAction("Error", "Error", e);
            }

        }

        public ActionResult Timetable()
        {
            try
            {
                var timetable = (from x in db.Timetable
                                 select new TimetableModel
                                 {
                                     ID = x.TimetableId,
                                     FacultyID = x.FacultyId,
                                     FacultyName = x.Faculty.Users.FullName,
                                     CourseID = x.CourseId,
                                     CourseName = x.Courses.CourseName,
                                     Day = x.DayOfWeek,
                                     StartTime = x.StartTime.ToString(),
                                     EndTime = x.EndTime.ToString(),
                                 }).ToList();
                return View(timetable);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        [HttpPost]
        public ActionResult AddEntry(TimetableModel model)
        {
            try
            {
                var timetable = db.Timetable.Find(model.ID);
                if (timetable != null)
                {
                    timetable.FacultyId = model.FacultyID;
                    timetable.CourseId = model.CourseID;
                    timetable.DayOfWeek = model.Day;
                    timetable.StartTime = TimeSpan.Parse(model.StartTime);
                    db.Entry(timetable).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Updated";
                }
                else
                {
                    var newtimetable = new Timetable
                    {
                        CourseId = model.CourseID,
                        DayOfWeek = model.Day,
                        FacultyId = model.FacultyID,
                        StartTime = TimeSpan.Parse(model.StartTime),
                        EndTime = TimeSpan.Parse(model.StartTime).Add(TimeSpan.FromHours(1))
                    };
                    db.Timetable.Add(newtimetable);
                    db.Entry(timetable).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    TempData["Message"] = "Created";
                }
                return RedirectToAction("Timetable");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult EditEntry(int? id)
        {
            try
            {
                List<SelectListItem> faculty = (from f in db.Faculty
                                                select new SelectListItem
                                                {
                                                    Text = f.Users.FullName,
                                                    Value = f.FacultyId.ToString()
                                                }).ToList();
                List<SelectListItem> courses = (from c in db.Courses
                                                select new SelectListItem
                                                {
                                                    Value = c.CourseId.ToString(),
                                                    Text = c.CourseName,
                                                }).ToList();

                List<string> daysofweek = new List<string>(new string[] { "Monday", "Tuesday", "Wednesday", "Thrusday", "Friday" });
                List<SelectListItem> days = (from d in daysofweek
                                             select new SelectListItem
                                             {
                                                 Text = d,
                                                 Value = d
                                             }).ToList();
                ViewBag.Faculty = faculty;
                ViewBag.Courses = courses;
                ViewBag.days = days;
                var timetable = (from x in db.Timetable
                                 where x.TimetableId == id
                                 select new TimetableModel
                                 {
                                     ID = x.TimetableId,
                                     FacultyID = x.FacultyId,
                                     FacultyName = x.Faculty.Users.FullName,
                                     CourseID = x.CourseId,
                                     CourseName = x.Courses.CourseName,
                                     Day = x.DayOfWeek,
                                     StartTime = x.StartTime.ToString(),
                                     EndTime = x.EndTime.ToString(),
                                 }).FirstOrDefault();
                if (timetable != null)
                {
                    return View(timetable);
                }                
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }

        }

        public ActionResult DeleteEntry(int id)
        {
            try
            {
                var timetable = db.Timetable.Find(id);
                db.Timetable.Remove(timetable);
                db.Entry(timetable).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                TempData["Message"] = "Deleted";
                return RedirectToAction("Timetable");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult ManageStudent()
        {
            try
            {
                IEnumerable<Topscholars.Models.Faculty> faculty = db.Faculty;
                return View(faculty);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
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
                return RedirectToAction("Error", "Error", e);
            }
        }
        public ActionResult ProcessedFeedback() { return View(); }
        public ActionResult DeleteFeedback() { return View(); }
    }
}