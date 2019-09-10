using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShopApp.WebUI.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult FileUploadErrorPage()
        {
            return View();
        }
    }
}