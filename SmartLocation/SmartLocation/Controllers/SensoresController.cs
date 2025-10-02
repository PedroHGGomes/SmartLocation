using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLocation.Models;
using SmartLocation.Api.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(Summary = "Listar sensores")]
        [SwaggerResponse(200, "Lista de sensores retornada com sucesso", typeof(PagedResult<Sensor>))]
        public async Task<ActionResult<PagedResult<Sensor>>> GetSensores([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            var result = await _contexto.Sensor
                .AsNoTracking()
                .OrderBy(s => s.Id)
                .ToPagedResultAsync(page, pageSize, baseUrl);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Buscar sensor por ID")]
        [SwaggerResponse(200, "Sensor encontrado", typeof(Sensor))]
        [SwaggerResponse(404, "Sensor não encontrado")]
        public async Task<ActionResult<Sensor>> GetSensorPorId(int id)
        {
            var sensor = await _contexto.Sensor.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (sensor == null) return NotFound();
            return Ok(sensor);
        }

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Buscar sensores por número")]
        [SwaggerResponse(200, "Sensores encontrados", typeof(IEnumerable<Sensor>))]
        [SwaggerResponse(404, "Nenhum sensor encontrado")]
        public async Task<ActionResult<IEnumerable<Sensor>>> BuscarPorNumero([FromQuery] int numero)
        {
            if (numero <= 0) return BadRequest("O parâmetro 'numero' deve ser maior que zero.");
            var sensores = await _contexto.Sensor
                .Where(s => s.Numero == numero)
                .AsNoTracking()
                .ToListAsync();

            if (!sensores.Any()) return NotFound();
            return Ok(sensores);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Criar sensor")]
        [SwaggerResponse(201, "Sensor criado com sucesso", typeof(Sensor))]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<ActionResult<Sensor>> CriarSensor([FromBody] Sensor sensor)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _contexto.Sensor.Add(sensor);
            await _contexto.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSensorPorId), new { id = sensor.Id }, sensor);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar sensor")]
        [SwaggerResponse(204, "Sensor atualizado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos ou ID inconsistente")]
        [SwaggerResponse(404, "Sensor não encontrado")]
        public async Task<IActionResult> AtualizarSensor(int id, [FromBody] Sensor sensor)
        {
            if (id != sensor.Id) return BadRequest("ID da URL diferente do objeto enviado.");
            _contexto.Entry(sensor).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_contexto.Sensor.Any(e => e.Id == id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Excluir sensor")]
        [SwaggerResponse(204, "Sensor excluído com sucesso")]
        [SwaggerResponse(404, "Sensor não encontrado")]
        public async Task<IActionResult> ExcluirSensor(int id)
        {
            var sensor = await _contexto.Sensor.FindAsync(id);
            if (sensor == null) return NotFound();

            _contexto.Sensor.Remove(sensor);
            await _contexto.SaveChangesAsync();
            return NoContent();
        }
    }
}



