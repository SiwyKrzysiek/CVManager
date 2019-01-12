using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVManager.EntityFramework;
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
            return Ok(new {id = pageNumber, name = "Stefan"});
        }
    }
}
