using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay1193.Entity
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string EmployeeNo { get; set; }
        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required, MaxLength(50)]
        public string LatName { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string ImageUrl { get; set; }
        [DataType(DataType.Date)]
        public DateTime DoB { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateJoined { get; set; }
        public string Designation { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string NationalInsuranceNo { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public StudentLoan StudentLoan { get; set; }
        public UnionMember UnionMember { get; set; }
        [Required, MaxLength(250)]
        public string  Address { get; set; }
        [Required]
        public string City { get; set; }

        [Required, MaxLength(20)]
        public string PostCode { get; set; }
    }
}
