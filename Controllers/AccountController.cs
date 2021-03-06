﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP.Controllers
{
    public class AccountController : Controller
    {
        private Login curUser()
        {
            Login cUser = HttpContext.Session.GetObject<Login>("Al_Lecturer");
            return cUser;
        }
        [HttpGet]
        public IActionResult Authenticate()
        {
            
            ViewData["layout"] = "_Layout";

            return View("_Login");
        }
        [HttpPost]
        public IActionResult Authenticate(Login login)
        {
            if (curUser() == null)
            {
                string sql = @"SELECT * FROM Al_Lecturer WHERE Name = '{0}' AND Password = HASHBYTES('SHA1', '{1}')";
                var result = DBUtl.GetList(sql, login.UserId, login.Password);
                if (result.Count > 0)
                {
                    dynamic user = result[0];
                    login.Name = user.Name;
                    login.Password = null;
                    login.Id = user.Id;
                    HttpContext.Session.SetObject("Al_lecturer", login);
                    //return RedirectToAction("Index");
                    return View("Index");
                }
                ViewData["layout"] = "_Layout";
                ViewData["msg"] = "Login failed";
                return View("Index");
            }
            else
                return RedirectToAction("Index");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    }
}
