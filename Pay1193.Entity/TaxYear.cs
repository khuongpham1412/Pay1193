using System.ComponentModel.DataAnnotations;

namespace Pay1193.Entity
{
    public class TaxYear
    {
        [Key]
        public int Id { get; set; }
        public string YearOfTax { get; set; }
    }
}