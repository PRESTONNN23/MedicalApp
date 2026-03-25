using Microsoft.AspNetCore.Mvc;
using MedicalApp.Data;
using MedicalApp.Models;
using System.Linq;

namespace MedicalApp.Controllers
{
    public class PatientsController : Controller
    {
        private readonly AppDbContext _context;

        public PatientsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetPatients()
        {
            var patients = _context.Patients
                .Select(p => new {
                    p.Id,
                    p.LastName,
                    p.FirstName,
                    p.MiddleName,
                    p.BirthDate,
                    p.Phone
                })
                .ToList();

            return Json(patients);
        }

        [HttpPost]
        public IActionResult Create([FromForm] Patient model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Проверьте корректность данных");

            model.Id = Guid.NewGuid();
            model.LastName ??= string.Empty;
            model.FirstName ??= string.Empty;
            model.Phone ??= string.Empty;

            _context.Patients.Add(model);
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IActionResult ExportXml(Guid id)
        {
            var patient = _context.Patients.Find(id);
            if (patient == null) return NotFound();

            var visits = _context.Visits.Where(v => v.PatientId == id).ToList();

            var xml = new System.Xml.Linq.XElement("Patient",
                new System.Xml.Linq.XElement("LastName", patient.LastName),
                new System.Xml.Linq.XElement("FirstName", patient.FirstName),
                new System.Xml.Linq.XElement("MiddleName", patient.MiddleName ?? ""),
                new System.Xml.Linq.XElement("BirthDate", patient.BirthDate.ToString("yyyy-MM-dd")),
                new System.Xml.Linq.XElement("Phone", patient.Phone),
                new System.Xml.Linq.XElement("Visits",
                    visits.Select(v =>
                        new System.Xml.Linq.XElement("Visit",
                            new System.Xml.Linq.XElement("VisitDate", v.VisitDate.ToString("yyyy-MM-dd")),
                            new System.Xml.Linq.XElement("DiagnosisCode", v.DiagnosisCode)
                        )
                    )
                )
            );

            return Content(xml.ToString(), "application/xml");
        }
    }
}