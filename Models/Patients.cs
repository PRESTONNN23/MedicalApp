using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models
{
    public class Patient
    {
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? MiddleName { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [MaxLength(15)]
        public string Phone { get; set; } = string.Empty;
    }
}