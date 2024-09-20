using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
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
            var faculty = db.Faculty.Find(id);
            return View(faculty);
        }
        public ActionResult EditFaculty(int id)
        {
            var faculty = db.Faculty.Find(id);
            return View(faculty);
        }
        public ActionResult DeleteFaculty(int id)
        {
            var faculty = db.Faculty.Find(id);
            return View(faculty);
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