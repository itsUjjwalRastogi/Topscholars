using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Topscholars.Models;

namespace Topscholars.Controllers
{
    public class FacultyController : Controller
    {
        topscholarsEntities db = new topscholarsEntities();

        public ActionResult Index()
        {
            return View();
        }

        #region Manage Assignments

        public ActionResult ManageAssignment()
        {
            try
            {
                var userId = Convert.ToInt16(Session["UserID"].ToString());
                var assignment = (from a in db.Assignments
                                  from f in db.Faculty
                                  where f.UserId == userId
                                  where a.FacultyId == f.FacultyId
                                  select a
                                  ).ToList();
                return View(assignment);
}
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult EditAssignment(int? id)
        {
            try
            {
                var assignment = db.Assignments.Find(id);
                if (id == null)
                {
                    return View(assignment);
                }
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult ViewAssignment(int id)
        {
            try
            {
                var assignment = db.Assignments.Find(id);
                return View(assignment);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult DeleteAssignment(int id)
        {
            try
            {
                var assignment = db.Assignments.Find(id);
                db.Assignments.Remove(assignment);
                db.Entry(assignment).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                TempData["Message"] = "Deleted";
                return RedirectToAction("ManageAssignment");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        [HttpPost]
        public ActionResult AddAssignment(Assignments model)
        {
            try
            {
                var assignment = db.Assignments.Find(model.AssignmentId);
                if(assignment != null)
                {
                    assignment.Title = model.Title;
                    assignment.Description = model.Description;
                    assignment.DueDate = model.DueDate;
                    db.Entry(assignment).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Updated";
                }
                else
                {
                    var newassignment = new Assignments
                    {
                        Description = model.Description,
                        CourseId = model.CourseId,
                        DueDate = model.DueDate,
                        FacultyId = model.FacultyId,
                        Title = model.Title,
                    };
                    db.Assignments.Add(newassignment);
                    db.Entry(assignment).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    TempData["Message"] = "Created";
                }
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }
        #endregion

        #region View Timetable

        public ActionResult ViewTimetable()
        {
            var userId = Convert.ToInt16(Session["UserID"].ToString());
            var timetable = (from t in db.Timetable
                             from f in db.Faculty
                             where f.UserId == userId
                             where t.FacultyId == f.FacultyId
                             select new TimetableModel
                             {
                                 ID = t.TimetableId,
                                 CourseID = t.CourseId,
                                 FacultyID = t.FacultyId,
                                 CourseName = t.Courses.CourseName,
                                 FacultyName = t.Faculty.Users.FullName,
                                 Day = t.DayOfWeek,
                                 StartTime = t.StartTime.ToString(),
                                 EndTime = t.EndTime.ToString(),
                             }).ToList();
            return View(timetable);
        }

        #endregion

        #region Provide Feedback

        // GET: Faculty/ProvideFeedback
        public ActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitFeedback(Feedback model)
        {
            if (ModelState.IsValid)
            {
                var feedback = new Feedback
                {
                    FeedbackText = model.FeedbackText,
                    DateSubmitted = DateTime.Now,
                    Status = "Pending"
                };                
                db.Feedback.Add(feedback);
                db.Entry(feedback).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                TempData["Message"] = "Submited";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        #endregion
    }
}

