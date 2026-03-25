using Microsoft.AspNetCore.Mvc;
using MedicalApp.Data;
using MedicalApp.Models;
using System.Linq;

namespace MedicalApp.Controllers
{
    public class VisitsController : Controller
    {
        private readonly AppDbContext _context;

        public VisitsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult GetVisits(Guid patientId)
        {
            var visits = _context.Visits
                .Where(v => v.PatientId == patientId)
                .Select(v => new {
                    v.Id,
                    v.VisitDate,
                    v.DiagnosisCode
                })
                .ToList();

            return Json(visits);
        }

        [HttpPost]
        public IActionResult Create([FromForm] Visit model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Ошибка данных");

            model.Id = Guid.NewGuid();
            model.DiagnosisCode ??= string.Empty;

            _context.Visits.Add(model);
            _context.SaveChanges();

            return Ok();
        }
    }
}