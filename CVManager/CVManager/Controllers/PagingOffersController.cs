using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVManager.EntityFramework;
using CVManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CVManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagingOffersController : ControllerBase
    {
        private DataContext _context;

        public PagingOffersController(DataContext context)
        {
            _context = context;
        }

        // GET: api/PagingOffers/5
        [HttpGet("{pageNumber}", Name = "Get")]
        public IActionResult Get(int pageNumber = 1)
        {
            const int pageSize = 2;
            int recordCount = _context.JobOffers.Count();
            int pagesCount = (int)Math.Ceiling((double) recordCount / pageSize);

            if (pageNumber > pagesCount || pageNumber < 1)
                return BadRequest();

            var offers = LoadJobOffers().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


            return Ok(new JobOffersPagingView() {JobOffers = offers, PagesCount = pagesCount});
        }

        private List<JobOffer> LoadJobOffers()
        {
            var jobOffers = _context.JobOffers.ToList();
            var companies = _context.Companies.ToList();
            foreach (var offer in jobOffers)
            {
                offer.Company = companies.FirstOrDefault(c => c.Id == offer.CompanyId);
            }

            return jobOffers;
        }
    }
}
