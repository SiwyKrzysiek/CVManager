using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CVManager.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CVManager.Controllers
{
    public class JobOfferController : Controller
    {
        private static List<JobOffer> _jobOffers = new List<JobOffer>
        {
            new JobOffer {Id = 1, JobTitle = "C# Programmer"},
            new JobOffer {Id = 2, JobTitle = "Backend Developer"},
            new JobOffer {Id = 3, JobTitle = "Android Developer"},
            new JobOffer {Id = 4, JobTitle = "Teacher"},
            new JobOffer {Id = 5, JobTitle = "Firefighter"}
        };

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_jobOffers);
        }
    }
}
