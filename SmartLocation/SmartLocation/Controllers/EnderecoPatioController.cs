using Asp.Versioning;
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartLocation.Models;
using SmartLocation.Api.Infrastructure;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartLocation.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
public class EnderecoPatiosController : ControllerBase
    {
        private readonly Contexto _context;

        public EnderecoPatiosController(Contexto context)
        {
            _context = context;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Listar endereços de pátio")]
        [SwaggerResponse(200, "Lista de endereços retornada com sucesso", typeof(PagedResult<EnderecoPatio>))]
        public async Task<ActionResult<PagedResult<EnderecoPatio>>> GetEnderecosPatio([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            var result = await _context.EnderecosPatio
                .AsNoTracking()
                .OrderBy(e => e.Id)
                .ToPagedResultAsync(page, pageSize, baseUrl);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Buscar endereço de pátio por ID")]
        [SwaggerResponse(200, "Endereço encontrado", typeof(EnderecoPatio))]
        [SwaggerResponse(404, "Endereço não encontrado")]
        public async Task<ActionResult<EnderecoPatio>> GetEnderecoPatio(int id)
        {
            var enderecoPatio = await _context.EnderecosPatio.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
            if (enderecoPatio == null) return NotFound();
            return Ok(enderecoPatio);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Criar endereço de pátio")]
        [SwaggerResponse(201, "Endereço criado com sucesso", typeof(EnderecoPatio))]
        [SwaggerResponse(400, "Dados inválidos")]
        public async Task<ActionResult<EnderecoPatio>> PostEnderecoPatio([FromBody] EnderecoPatio enderecoPatio)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _context.EnderecosPatio.Add(enderecoPatio);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEnderecoPatio), new { id = enderecoPatio.Id }, enderecoPatio);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualizar endereço de pátio")]
        [SwaggerResponse(204, "Endereço atualizado com sucesso")]
        [SwaggerResponse(400, "Dados inválidos ou ID inconsistente")]
        [SwaggerResponse(404, "Endereço não encontrado")]
        public async Task<IActionResult> PutEnderecoPatio(int id, [FromBody] EnderecoPatio enderecoPatio)
        {
            if (id != enderecoPatio.Id) return BadRequest("ID da URL diferente do objeto enviado.");
            _context.Entry(enderecoPatio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.EnderecosPatio.Any(e => e.Id == id)) return NotFound();
                throw;
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Excluir endereço de pátio")]
        [SwaggerResponse(204, "Endereço excluído com sucesso")]
        [SwaggerResponse(404, "Endereço não encontrado")]
        public async Task<IActionResult> DeleteEnderecoPatio(int id)
        {
            var enderecoPatio = await _context.EnderecosPatio.FindAsync(id);
            if (enderecoPatio == null) return NotFound();

            _context.EnderecosPatio.Remove(enderecoPatio);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}



