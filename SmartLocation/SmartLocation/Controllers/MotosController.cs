using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLocation.Models;
using SmartLocation.Api.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartLocation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotosController : ControllerBase
    {
        private readonly Contexto _contexto;

        public MotosController(Contexto contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar motos", Description = "Retorna lista de motos com paginação e HATEOAS.")]
        [SwaggerResponse(200, "Lista de motos retornada com sucesso", typeof(PagedResult<Moto>))]
        [SwaggerResponse(400, "Parâmetros inválidos")]
        public async Task<ActionResult<PagedResult<Moto>>> GetMotos([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            var result = await _contexto.Moto
                .AsNoTracking()
                .OrderBy(m => m.Id)
                .ToPagedResultAsync(page, pageSize, baseUrl);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Buscar moto por ID")]
        [SwaggerResponse(200, "Moto encontrada", typeof(Moto))]
        [SwaggerResponse(404, "Moto não encontrada")]
        public async Task<ActionResult<Moto>> GetMotoPorId(int id)
        {
            var moto = await _contexto.Moto.FindAsync(id);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        [HttpGet("search")]
        [SwaggerOperation(Summary = "Buscar motos por modelo")]
        [SwaggerResponse(200, "Motos encontradas", typeof(IEnumerable<Moto>))]
        [SwaggerResponse(404, "Nenhuma moto encontrada")]
        public async Task<ActionResult<IEnumerable<Moto>>> BuscarPorModelo([FromQuery] string modelo)
        {
            var motos = await _contexto.Moto
                .Where(m => m.Modelo.Contains(modelo))
                .AsNoTracking()
                .ToListAsync();

            if (!motos.Any()) return NotFound();
            return Ok(motos);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Criar moto")]
        [SwaggerResponse(201, "Moto criada com sucesso", typeof(Moto))]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<ActionResult<Moto>> CriarMoto([FromBody] Moto moto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _contexto.Moto.Add(moto);
            await _contexto.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMotoPorId), new { id = moto.Id }, moto);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar moto")]
        [SwaggerResponse(204, "Moto atualizada com sucesso")]
        [SwaggerResponse(400, "Dados inválidos ou ID inconsistente")]
        [SwaggerResponse(404, "Moto não encontrada")]
        public async Task<IActionResult> AtualizarMoto(int id, [FromBody] Moto moto)
        {
            if (id != moto.Id) return BadRequest("ID da URL diferente do objeto enviado.");

            _contexto.Entry(moto).State = EntityState.Modified;
            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_contexto.Moto.Any(e => e.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Excluir moto")]
        [SwaggerResponse(204, "Moto excluída com sucesso")]
        [SwaggerResponse(404, "Moto não encontrada")]
        public async Task<IActionResult> ExcluirMoto(int id)
        {
            var moto = await _contexto.Moto.FindAsync(id);
            if (moto == null) return NotFound();

            _contexto.Moto.Remove(moto);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }
    }
}



