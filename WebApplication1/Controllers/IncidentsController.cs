using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentsController : Controller
    {
        private static readonly List<Incident> _incidents = new();
        private static int _nextId = 1;
        private static readonly string[] AllowedSeverities =
         { "LOW", "MEDIUM", "HIGH", "CRITICAL" };
        private static readonly string[] AllowedStatuses =
         { "OPEN", "IN_PROGRESS", "RESOLVED" };
        [HttpPost("create-incident")]
        public IActionResult CreateIncident([FromBody] Incident incident)
        {
            if (!AllowedSeverities.Contains(incident.Severity))
                return BadRequest("Severity invalide.");

            incident.Id = _nextId++;
            incident.Status = "OPEN";
            incident.CreatedAt = DateTime.Now;
            _incidents.Add(incident);
            return Ok(incident);
        }
        [HttpGet("get-all")]
        public IActionResult GetAllIncidents()
        {
            return Ok(_incidents);
        }
        [HttpGet("getbyid/{id}")]
        public IActionResult GetIncidentById(int id)
        {
            try
            {
                var incident = _incidents.First(i => i.Id == id);
                return Ok(incident);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
        [HttpPut("update-status/{id}")]
        public IActionResult UpdateIncidentStatus(int id, [FromBody] string status)
        {
            var incident = _incidents.FirstOrDefault(i => i.Id == id);
            if (incident == null)
                return NotFound();

            if (!AllowedStatuses.Contains(status))
                return BadRequest("Status invalide.");

            incident.Status = status;
            return Ok(incident);
        }
        [HttpDelete("delete-incident/{id}")]
        public IActionResult DeleteIncident(int id)
        {
            var incident = _incidents.FirstOrDefault(i => i.Id == id);

            if (incident == null)
                return NotFound();

            if (incident.Severity == "CRITICAL" && incident.Status == "OPEN")
                return BadRequest("Cannot delete an OPEN CRITICAL incident");

            _incidents.Remove(incident);
            return NoContent();
        }

        [HttpGet("filter-by-status")]
        public IActionResult FilterByStatus([FromQuery] string status)
        {
            var result = _incidents
                .Where(i => i.Status.Contains(status, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(result);
        }


        [HttpGet("filter-by-severity")]
        public IActionResult FilterBySeverity([FromQuery] string severity)
        {
            var result = _incidents
                .Where(i => i.Severity.Contains(severity, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(result);
        }
    }
}
































































