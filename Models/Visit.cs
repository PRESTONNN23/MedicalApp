using System;
using System.ComponentModel.DataAnnotations;

namespace MedicalApp.Models
{
    public class Visit
    {
        public Guid Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime VisitDate { get; set; }

        [MaxLength(36)]
        public string DiagnosisCode { get; set; } = string.Empty;

        public Guid PatientId { get; set; }
    }
}