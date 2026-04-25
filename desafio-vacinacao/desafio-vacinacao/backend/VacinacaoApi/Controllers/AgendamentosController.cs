using Microsoft.AspNetCore.Mvc;
using VacinacaoApi.DTOs;
using VacinacaoApi.Services;

namespace VacinacaoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentosController : ControllerBase
    {
        private readonly IAgendamentoService _service;

        public AgendamentosController(IAgendamentoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.ListarTodos();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CriarAgendamentoDto dto)
        {
            var erro = await _service.Criar(dto);
            if (erro != null)
                return BadRequest(new { message = erro });

            return Ok(new { message = "Agendamento realizado com sucesso!" });
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> PutStatus(int id, [FromBody] AtualizarStatusDto dto)
        {
            var sucesso = await _service.AtualizarStatus(id, dto);
            if (!sucesso)
                return NotFound();

            return NoContent();
        }
    }
}
