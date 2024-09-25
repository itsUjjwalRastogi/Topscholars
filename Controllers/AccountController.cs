﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Topscholars.Models;


using Microsoft.Ajax.Utilities;
using System.Web.Security;

namespace Topscholars.Controllers
{
    public class AccountController : Controller
    {
        topscholarsEntities db = new topscholarsEntities();

        public ActionResult Login(Users model)
        {
            model.Email = "johndoe@university.com";
            model.Password = "hashedpassword2";
            if (model.Email != null && model.Password != null)
            {
                var user = db.Users.Where(x => x.Email == model.Email).FirstOrDefault();
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.FullName, false);
                    Session["UserID"] = user.UserId;
                    Session["Role"] = user.Role;
                    return RedirectToAction("Index", "Faculty");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}