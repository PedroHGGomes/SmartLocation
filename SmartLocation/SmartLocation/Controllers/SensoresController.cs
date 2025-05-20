using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLocation.Models;

namespace SmartLocation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensoresController : ControllerBase
    {
        private readonly Contexto _contexto;

        public SensoresController(Contexto contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sensor>>> GetSensores()
        {
            return Ok(await _contexto.Sensor.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Sensor>> GetSensorPorId(int id)
        {
            var sensor = await _contexto.Sensor.FindAsync(id);

            if (sensor == null)
            {
                return NotFound();
            }

            return Ok(sensor);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Sensor>>> BuscarPorNumero([FromQuery] int numero)
        {
            var sensores = await _contexto.Sensor
                .Where(s => s.Numero == numero)
                .ToListAsync();

            if (sensores == null || sensores.Count == 0)
            {
                return NotFound();
            }

            return Ok(sensores);
        }

        [HttpPost]
        public async Task<ActionResult<Sensor>> CriarSensor(Sensor sensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _contexto.Sensor.Add(sensor);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSensorPorId), new { id = sensor.Id }, sensor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarSensor(int id, Sensor sensor)
        {
            if (id != sensor.Id)
            {
                return BadRequest();
            }

            _contexto.Entry(sensor).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_contexto.Sensor.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirSensor(int id)
        {
            var sensor = await _contexto.Sensor.FindAsync(id);

            if (sensor == null)
            {
                return NotFound();
            }

            _contexto.Sensor.Remove(sensor);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}

