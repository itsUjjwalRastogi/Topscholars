using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Topscholars.Models;
using WebGrease;

namespace Topscholars.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        topscholarsEntities db = new topscholarsEntities();
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Assignment()
        {
            try
            {
                var userid = Convert.ToInt16(Session["UserID"].ToString());
                var assignment = (from a in db.Assignments
                                  from s in db.Students
                                  from c in db.Courses
                                  where s.UserId == userid
                                  where c.ProgrammeId == s.ProgrammeId
                                  where a.CourseId == c.CourseId
                                  select a
                                  ).ToList();
                return View(assignment);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        [HttpPost]
        public ActionResult SubmitAssignment()
        {
            try
            {
                var userid = Convert.ToInt16(Session["UserID"].ToString());
                if (Request.Files.Count > 0)
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        byte[] fileData = null;
                        using (Stream fs = file.InputStream)
                        {
                            var binaryReader = new BinaryReader(fs);
                            fileData = binaryReader.ReadBytes((int)fs.Length);
                        }
                        Documents documents = new Documents
                        {
                            Document = fileData,
                            UserId = userid,
                            DocumentName = file.FileName,
                            DocumentType = file.ContentType,
                        };
                        db.Documents.Add(documents);
                        db.Entry(documents).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                    }
                    return Json("File Uploaded Successfully!");
                }
                else
                {
                    return Json("No files selected.");
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult Fees()
        {
            try
            {
                var userid = Convert.ToInt16(Session["UserID"].ToString());
                var fees = (from f in db.Fees
                            from s in db.Students
                            where s.UserId == userid
                            where f.StudentId == s.StudentId
                            where f.Status != "Paid"
                            select f).FirstOrDefault();
                return View(fees);
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        [HttpPost]
        public ActionResult SubmitFees(Fees model)
        {
            try
            {
                if (model != null && model.Amount > 0)
                {
                    var userid = Convert.ToInt16(Session["UserID"].ToString());
                    var fees = db.Fees.Where(x => x.Students.UserId == userid).FirstOrDefault();
                    fees.Amount = model.Amount;
                    fees.Status = "Paid";
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Success";
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public ActionResult ViewResult()
        {
            try
            {
                var userID = Convert.ToInt16(Session["UserId"].ToString());
                var result = (from r in db.Results
                              from s in db.Students
                              where s.UserId == userID
                              where s.StudentId == r.StudentId
                              select r
                              ).ToList();
                
                return View(result);
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
                var userid = Convert.ToInt16(Session["UserID"].ToString());
                var timetable = (from t in db.Timetable
                                 from s in db.Students
                                 from c in db.Courses
                                 where s.UserId == userid
                                 where c.Programme == s.Programme
                                 where t.CourseId == c.CourseId
                                 select new TimetableModel
                                 {
                                     ID = t.TimetableId,
                                     CourseID = t.CourseId,
                                     FacultyID = t.FacultyId,
                                     CourseName = t.Courses.CourseName,
                                     FacultyName = t.Faculty.Users.FullName,
                                     Day = t.DayOfWeek,
                                     StartTime = t.StartTime,
                                     EndTime = t.EndTime,
                                 }
                                 ).ToList();
                return View(timetable);
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
                return View();
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        [HttpPost]
        public ActionResult SubmitFeedback(Feedback model)
        {
            try
            {
                model.DateSubmitted = DateTime.Now;
                model.Status = "Pending";
                db.Feedback.Add(model);
                db.Entry(model).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
                TempData["Message"] = "Created";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("Error", "Error", e);
            }
        }

        public FileResult DownloadFile(int id)
        {
            var file = db.Documents.Find(id);            
            return File(file.Document, file.DocumentType, file.DocumentName);
        }
    }
}