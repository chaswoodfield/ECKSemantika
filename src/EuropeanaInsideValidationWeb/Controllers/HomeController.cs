using System.ComponentModel.DataAnnotations;
using Semantika.EuropeanaInside.Eck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Semantika.EuropeanaInside.Eck.Validation;

namespace EuropeanaInsideValidationWeb.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string xmldoc="")
        {
            if (!string.IsNullOrEmpty(xmldoc))
            {
                var validationService = new ValidationService();
                var request = new ValidationRequest() { Name=ValidationProfile.Lido.ToString(), XmlDocument = xmldoc };
                 ViewBag.ValidationResult = validationService.Validate(request);
            }
            else
            {
                ViewBag.Message = "Paste your's xml document!";
            }

            return View();
        }

    }
}
