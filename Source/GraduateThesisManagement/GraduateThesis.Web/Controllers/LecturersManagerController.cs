﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraduateThesis.Web.Controllers
{
    public class LecturersManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
