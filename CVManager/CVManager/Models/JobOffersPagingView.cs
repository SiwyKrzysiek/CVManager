using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CVManager.Models
{
    public class JobOffersPagingView
    {
        public IEnumerable<JobOffer> JobOffers { get; set; }
        public int PagesCount { get; set; }
    }
}
