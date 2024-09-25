using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IO = System.IO;
using context = System.Web.HttpContext;

namespace Topscholars.Controllers
{
    public class ErrorController : Controller
    {
        private static String ErrorlineNo, Errormsg, extype, exurl, hostIp, ErrorLocation, HostAdd;
        
        public ActionResult Error(Exception ex)
        {
            var line = Environment.NewLine + Environment.NewLine;
            ErrorlineNo = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
            Errormsg = ex.GetType().Name.ToString();
            extype = ex.GetType().ToString();
            exurl = context.Current.Request.Url.ToString();
            ErrorLocation = ex.Message.ToString();
            string filepath = context.Current.Server.MapPath("~/Logs/");  //Text File Path

            if (!IO.Directory.Exists(filepath))
            {
                IO.Directory.CreateDirectory(filepath);
            }
            filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
            if (!IO.File.Exists(filepath))
            {
                IO.File.Create(filepath).Dispose();
            }
            using (IO.StreamWriter sw = IO.File.AppendText(filepath))
            {
                string error = "Log Written Date:" + " " + DateTime.Now.ToString() + line + "Error Line No :" + " " + ErrorlineNo + line + "Error Message:" + " " + Errormsg + line + "Exception Type:" + " " + extype + line + "Error Location :" + " " + ErrorLocation + line + " Error Page Url:" + " " + exurl + line + "User Host IP:" + " " + hostIp + line;
                sw.WriteLine("-----------Exception Details on " + " " + DateTime.Now.ToString() + "-----------------");
                sw.WriteLine("-------------------------------------------------------------------------------------");
                sw.WriteLine(line);
                sw.WriteLine(error);
                sw.WriteLine("--------------------------------*End*------------------------------------------");
                sw.WriteLine(line);
                sw.Flush();
                sw.Close();
            }
            return View();
        }
    }
}