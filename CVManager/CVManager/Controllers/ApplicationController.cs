using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CVManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace CVManager.Controllers
{
    [Route("[controller]/[action]")]
    public class ApplicationController : Controller
    {
        public IActionResult Apply(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var offer = JobOfferController._jobOffers.Find(o => o.Id == id);
            if (offer == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(offer);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Apply(JobApplication model)
        //{
            
        //}
    }
}