using System;

namespace CVManager.Models
{
    public class JobOffer
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public decimal MonthlySalary { get; set; }
        public string Requirements { get; set; }
        public string Location { get; set; }
    }
}
