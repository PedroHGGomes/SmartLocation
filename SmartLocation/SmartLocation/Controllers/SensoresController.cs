using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLocation.Data;
using SmartLocation.Models;

namespace SmartLocation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SensoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetAll()
        {
            return Ok(await _context.Sensores.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sensor>> GetById(int id)
        {
            var sensor = await _context.Sensores.FindAsync(id);
            if (sensor == null) return NotFound();
            return Ok(sensor);
        }

        [HttpPost]
        public async Task<ActionResult<Sensor>> Create(Sensor sensor)
        {
            _context.Sensores.Add(sensor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = sensor.Id }, sensor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Sensor sensor)
        {
            if (id != sensor.Id) return BadRequest();
            _context.Entry(sensor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sensor = await _context.Sensores.FindAsync(id);
            if (sensor == null) return NotFound();
            _context.Sensores.Remove(sensor);
            await _context.SaveChangesAsync();
            return NoContentResult();
        }
    }
}
