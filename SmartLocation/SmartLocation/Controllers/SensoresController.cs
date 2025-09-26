using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLocation.Models;
using SmartLocation.Api.Infrastructure;

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

        // GET: api/Sensores (com paginação + HATEOAS)
        [HttpGet]
        public async Task<ActionResult<PagedResult<Sensor>>> GetSensores(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

            var result = await _contexto.Sensor
                .AsNoTracking()
                .OrderBy(s => s.Id)
                .ToPagedResultAsync(page, pageSize, baseUrl);

            return Ok(result);
        }

        // GET: api/Sensores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sensor>> GetSensorPorId(int id)
        {
            var sensor = await _contexto.Sensor.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

            if (sensor == null)
            {
                return NotFound();
            }

            return Ok(sensor);
        }

        // GET: api/Sensores/search?numero=123
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Sensor>>> BuscarPorNumero([FromQuery] int numero)
        {
            if (numero <= 0)
            {
                return BadRequest("O parâmetro 'numero' deve ser maior que zero.");
            }

            var sensores = await _contexto.Sensor
                .Where(s => s.Numero == numero)
                .AsNoTracking()
                .ToListAsync();

            if (!sensores.Any())
            {
                return NotFound();
            }

            return Ok(sensores);
        }

        // POST: api/Sensores
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

        // PUT: api/Sensores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarSensor(int id, Sensor sensor)
        {
            if (id != sensor.Id)
            {
                return BadRequest("ID da URL diferente do objeto enviado.");
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

        // DELETE: api/Sensores/5
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


