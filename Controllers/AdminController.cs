using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Topscholars.Models;

namespace Topscholars.Controllers
{
    [Authorize]
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
                if (faculty != null)
                {
                    var users = db.Users.Find(faculty.UserId);
                    db.Faculty.Remove(faculty);
                    db.Entry(faculty).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    db.Users.Remove(users);
                    db.Entry(users).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    TempData["Message"] = "Deleted";
                }
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
                                     StartTime = x.StartTime,
                                     EndTime = x.EndTime,
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
                    timetable.StartTime = model.StartTime;
                    timetable.EndTime = model.StartTime.Add(TimeSpan.FromHours(1));
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
                        StartTime = model.StartTime,
                        EndTime = model.StartTime.Add(TimeSpan.FromHours(1))
                    };
                    db.Timetable.Add(newtimetable);
                    db.Entry(newtimetable).State = System.Data.Entity.EntityState.Added;
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
                //List<SelectListItem> faculty = (from f in db.Faculty
                //                                select new SelectListItem
                //                                {
                //                                    Text = f.Users.FullName,
                //                                    Value = f.FacultyId.ToString()
                //                                }).ToList();
                //List<SelectListItem> courses = (from c in db.Courses
                //                                select new SelectListItem
                //                                {
                //                                    Value = c.CourseId.ToString(),
                //                                    Text = c.CourseName,
                //                                }).ToList();
                List<string> daysofweek = new List<string>(new string[] { "Monday", "Tuesday", "Wednesday", "Thrusday", "Friday" });
                List<SelectListItem> days = (from d in daysofweek
                                             select new SelectListItem
                                             {
                                                 Text = d,
                                                 Value = d
                                             }).ToList();
                ViewBag.Courses = new SelectList(db.Courses, "CourseId", "CourseName");
                ViewBag.Faculty = new SelectList(db.Faculty, "FacultyId", "Users.FullName");
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
                                     StartTime = x.StartTime,
                                     EndTime = x.EndTime,
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
                var student = (from s in db.Students
                               select new StudentModel
                               {
                                   ID = s.StudentId,
                                   Name = s.Users.FullName,
                                   Email = s.Users.Email,
                                   ProgrammeID = s.ProgrammeId,
                                   Programme = s.Programme.ProgrammeName,
                                   RollNumber = s.RollNumber,
                                   ContactNumber = s.ContactNumber,
                                   Address = s.Address,
                                   DOB = s.DateOfBirth,
                                   AdmissionDate = s.AdmissionDate
                               }).ToList();
                return View(student);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult ViewStudents(int id)
        {
            try
            {
                var student = (from s in db.Students
                               where s.StudentId == id
                               select new StudentModel
                               {
                                   ID = s.StudentId,
                                   Name = s.Users.FullName,
                                   Email = s.Users.Email,
                                   ProgrammeID = s.ProgrammeId,
                                   Programme = s.Programme.ProgrammeName,
                                   RollNumber = s.RollNumber,
                                   ContactNumber = s.ContactNumber,
                                   Address = s.Address,
                                   DOB = s.DateOfBirth,
                                   AdmissionDate = s.AdmissionDate
                               }).FirstOrDefault();
                return View(student);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        [HttpPost]
        public ActionResult AddStudents(StudentModel model)
        {
            try
            {
                var student = db.Students.Find(model.ID);
                if (student != null)
                {
                    student.ProgrammeId = model.ProgrammeID;
                    student.Address = model.Address;
                    student.ContactNumber = model.ContactNumber;
                    student.DateOfBirth = model.DOB;
                    db.Entry(student).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    var users = db.Users.Find(student.UserId);
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
                        Role = "Student",
                        Password = "",
                    };
                    db.Users.Add(newusers);
                    db.Entry(newusers).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    var users = db.Users.Where(x => x.Email == newusers.Email).FirstOrDefault();
                    var maxRollNumber = db.Students.Max(x => x.RollNumber);
                    Students newstudent = new Students
                    {
                        UserId = users.UserId,
                        Address = model.Address,
                        ContactNumber = model.ContactNumber,
                        DateOfBirth = model.DOB,
                        ProgrammeId = model.ProgrammeID,
                        AdmissionDate = DateTime.Now,
                        RollNumber = maxRollNumber + 1,
                    };
                    db.Students.Add(newstudent);
                    db.Entry(newstudent).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    TempData["Message"] = "Created";
                }
                return RedirectToAction("ManageStudent");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult EditStudent(int? id)
        {
            try
            {
                //List<SelectListItem> programme = (from p in db.Programme
                //                                  select new SelectListItem
                //                                  {
                //                                      Text = p.ProgrammeName,
                //                                      Value = p.ProgrammeId.ToString(),
                //                                  }).ToList();
                ViewBag.Programme = new SelectList(db.Programme, "ProgrammeId", "ProgrammeName");
                var student = (from s in db.Students
                               where s.StudentId == id
                               select new StudentModel
                               {
                                   ID = s.StudentId,
                                   Name = s.Users.FullName,
                                   Email = s.Users.Email,
                                   ProgrammeID = s.ProgrammeId,
                                   Programme = s.Programme.ProgrammeName,
                                   RollNumber = s.RollNumber,
                                   ContactNumber = s.ContactNumber,
                                   Address = s.Address,
                                   DOB = s.DateOfBirth,
                                   AdmissionDate = s.AdmissionDate
                               }).FirstOrDefault();
                if (student != null)
                {
                    return View(student);
                }
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult DeleteStudent(int id)
        {
            try
            {
                var student = db.Students.Find(id);
                var user = db.Users.Find(student.UserId);
                db.Students.Remove(student);
                db.Entry(student).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                db.Users.Remove(user);
                db.Entry(user).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                TempData["Message"] = "Deleted";
                return RedirectToAction("ManageStudent");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult Feedback()
        {
            try
            {
                IEnumerable<Feedback> feedback = db.Feedback.Where(x => x.Status == "Pending");
                return View(feedback);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult ProcessedFeedback(int id)
        {
            var feedback = db.Feedback.Find(id);
            feedback.Status = "Processed";
            db.Entry(feedback).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            TempData["Message"] = "Updated";
            return RedirectToAction("Feedback");
        }

        public ActionResult DeleteFeedback(int id)
        {
            var feedback = db.Feedback.Find(id);
            db.Feedback.Remove(feedback);
            db.Entry(feedback).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            TempData["Message"] = "Deleted";
            return RedirectToAction("Feedback");
        }

        public ActionResult ManageCourses()
        {
            try
            {
                var courses = (from c in db.Courses
                               orderby c.ProgrammeId
                               select new CoursesModel
                               {
                                   ID = c.CourseId,
                                   Name = c.CourseName,
                                   Description = c.CourseDescription,
                                   ProgrammeID = c.ProgrammeId,
                                   Programme = c.Programme.ProgrammeName,
                                   Credit = c.Credits,
                                   FacultyID = c.FacultyId,
                                   FacultyName = c.Faculty.Users.FullName,
                               }).ToList();
                return View(courses);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        [HttpPost]
        public ActionResult AddCourses(CoursesModel model)
        {
            try
            {
                var course = db.Courses.Find(model.ID);
                if (course != null)
                {
                    course.CourseName = model.Name;
                    course.CourseDescription = model.Description;
                    course.Credits = model.Credit;
                    course.FacultyId = model.FacultyID;
                    db.Entry(course).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Updated";
                }
                else
                {
                    var newcourse = new Courses
                    {
                        CourseName = model.Name,
                        CourseDescription = model.Description,
                        Credits = model.Credit,
                        ProgrammeId = model.ProgrammeID,
                        FacultyId = model.FacultyID
                    };
                    db.Courses.Add(newcourse);
                    db.Entry(newcourse).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    TempData["Message"] = "Created";
                }
                return RedirectToAction("ManageCourses");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult EditCourses(int? id)
        {
            try
            {
                ViewBag.Programme = new SelectList(db.Programme, "ProgrammeId", "ProgrammeName");
                ViewBag.Faculty = new SelectList(db.Faculty, "FacultyId", "Users.FullName");
                var courses = (from c in db.Courses
                               where c.CourseId == id
                               select new CoursesModel
                               {
                                   ID = c.CourseId,
                                   Name = c.CourseName,
                                   ProgrammeID = c.ProgrammeId,
                                   Description = c.CourseDescription,
                                   Programme = c.Programme.ProgrammeName,
                                   Credit = c.Credits,
                                   FacultyID = c.FacultyId,
                                   FacultyName = c.Faculty.Users.FullName,
                               }).FirstOrDefault();
                if (courses != null)
                {
                    return View(courses);
                }
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult DeleteCourses(int id)
        {
            try
            {
                var courses = db.Courses.Find(id);
                db.Courses.Remove(courses);
                db.Entry(courses).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                TempData["Message"] = "Deleted";
                return RedirectToAction("ManageCourses");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult ManageProgramme()
        {
            try
            {
                var programme = db.Programme.ToList();
                return View(programme);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult EditProgramme(int? id)
        {
            try
            {
                var programme = db.Programme.Find(id);
                if (programme != null)
                {
                    return View(programme);
                }
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        [HttpPost]
        public ActionResult AddProgramme(Programme model)
        {
            try
            {
                var programme = db.Programme.Find(model.ProgrammeId);
                if (programme != null)
                {
                    programme.ProgrammeName = model.ProgrammeName;
                    db.Entry(programme).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Updated";
                }
                else
                {
                    var newprogramme = new Programme
                    {
                        ProgrammeName = model.ProgrammeName,
                    };
                    db.Programme.Add(newprogramme);
                    db.Entry(newprogramme).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    TempData["Message"] = "Created";
                }
                return RedirectToAction("ManageProgramme");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult DeleteProgramme(int id)
        {
            try
            {
                var programme = db.Programme.Find(id);
                var courses = db.Courses.Where(x => x.ProgrammeId == id).Count();
                if (programme != null && courses == 0)
                {
                    db.Programme.Remove(programme);
                    db.Entry(programme).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    TempData["Message"] = "Deleted";
                }
                else
                {
                    TempData["Message"] = "DeleteError";
                }
                return RedirectToAction("ManageProgramme");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }
    }
}