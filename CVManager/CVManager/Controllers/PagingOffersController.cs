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

        /// <summary>
        /// Returns page of job offers records. Can be searched.
        /// </summary>
        /// <param name="pageNumber">Number of page to be returned</param>
        /// <param name="searchString">Case sensitive fragment of job offer that will be searched for.
        /// If left empty then all offers are returned.</param>
        /// <returns>One page off job offers</returns>
        // GET: api/PagingOffers/5
        [HttpGet("{pageNumber}", Name = "Get")]
        //[HttpGet]
        public IActionResult Get([FromRoute] int pageNumber = 1, [FromQuery(Name = "search")] string searchString = "")
        {
            const int pageSize = 3;

            var offers = LoadJobOffers();
            if (!string.IsNullOrEmpty(searchString))
                offers = offers.Where(o => o.JobTitle.Contains(searchString)).ToList();

            int recordCount = offers.Count();
            if (recordCount == 0)
                return Ok(new JobOffersPagingView() { JobOffers = offers, PagesCount = 1 });

            int pagesCount = (int)Math.Ceiling((double) recordCount / pageSize);

            if (pageNumber > pagesCount || pageNumber < 1)
                return BadRequest();

            offers = offers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


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
