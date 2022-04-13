using System.ComponentModel.DataAnnotations;

namespace A101CallCenter.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Range(0, 200)]
        public int? Age { get; set; }
        public int? MonthlyIncome { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Address { get; set; }
        //Müşterinin A101 hakkındaki görüşleri
        public string? Review { get; set; }
    }
}
