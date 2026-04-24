using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncidentsDbController : ControllerBase
    {
        private readonly IncidentsDbContext _context;

        public IncidentsDbController(IncidentsDbContext context)
        {
            _context = context;
        }

        // GET: api/IncidentsDb
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Incident>>> GetIncidents()
        {
            return await _context.Incidents.ToListAsync();
        }

        // GET: api/IncidentsDb/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Incident>> GetIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);

            if (incident == null)
            {
                return NotFound();
            }

            return incident;
        }

        // PUT: api/IncidentsDb/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncident(int id, Incident incident)
        {
            if (id != incident.Id)
            {
                return BadRequest();
            }

            _context.Entry(incident).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IncidentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/IncidentsDb
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Incident>> PostIncident(Incident incident)
        //{
        //    _context.Incidents.Add(incident);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetIncident", new { id = incident.Id }, incident);
        //}
        [HttpPost]
        public async Task<ActionResult<Incident>> PostIncident(Incident incident)
        {
            incident.Status = "IN_PROGRESS";

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            incident.Status = "OPEN";
            incident.CreatedAt = DateTime.Now;

            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIncident", new { id = incident.Id }, incident);
        }
        // DELETE: api/IncidentsDb/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.Id == id);
        }


        [HttpGet("filter-by-status/{status}")]
        public IActionResult FilterByStatus(string s)
        {
            var l = from inc in _context.Incidents where inc.Status == s select inc;
            return Ok(l);
        }


        [HttpGet("filter-by-severity/{severity}")]
        public IActionResult FilterBySeverity(string severity)
        {
            var l = from inc in _context.Incidents where inc.Severity == severity select inc;

            return Ok(l);
        }

        [HttpGet("getbyseverityasync/{severity}")]
        public async Task<IActionResult> FilterBySeverityAsync(string severity)
        {
            var incidents = await _context.Incidents
                .Where(i => i.Severity.Contains(severity))
                .ToListAsync();

            return Ok(incidents);
        }

        [HttpGet("statusAsync/{status}")]
        public async Task<IActionResult> FilterByStatusAsync(string status)
        {
            var result = await _context.Incidents
                .Where(i => i.Status.Contains(status))
                .ToListAsync();

            return Ok(result);
        }
        private static readonly string[] AllowedSeverities = { "LOW", "MEDIUM", "HIGH", "CRITICAL" };
        private static readonly string[] AllowedStatuses = { "OPEN", "IN_PROGRESS", "RESOLVED" };


    }

}
