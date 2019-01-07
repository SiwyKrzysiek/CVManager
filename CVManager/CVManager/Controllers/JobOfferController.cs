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
            new JobOffer {Id = 1, JobTitle = "C# Programmer", MonthlySalayr = 6800, Location="Warszawa", Requirements="Knowlege of C#"},
            new JobOffer {Id = 2, JobTitle = "Backend Developer", MonthlySalayr= 5 495, Location="Gdańsk", Requirements="Expirience in PHP"},
            new JobOffer {Id = 3, JobTitle = "Android Developer", Requirements="Java and Kotlin"},
            new JobOffer {Id = 4, JobTitle = "Teacher", Location="Piaseczno", Requirements="Degree in history"},
            new JobOffer {Id = 5, JobTitle = "Firefighter", Location="Warszawa"}
        };

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View(_jobOffers);
        }

        public IActionResult Details(int id)
        {
            return View(_jobOffers[id]);
        }
    }
}
