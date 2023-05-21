using FluxoDeCaixa.API.Extensions;
using FluxoDeCaixa.API.Models;
using FluxoDeCaixa.Application.Interfaces;
using FluxoDeCaixa.Application.Requests.Lancamento;
using FluxoDeCaixa.Application.Responses;
using FluxoDeCaixa.Application.Services;
using FluxoDeCaixa.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace FluxoDeCaixa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class LivroCaixaController : ControllerBase
    {
        private readonly ILivroCaixaService _service;
        public LivroCaixaController(ILivroCaixaService service) => _service = service;

        /// <summary>
        /// Obter Livro Caixa por data 
        /// </summary>
        /// <param name="request">Data Livro, formato: yyyy-MM-dd </param>
        /// <response code="200">Retorna Livro Caixa.</response>
        /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
        /// <response code="404">Quando nenhuma conta é encontrado pelo e-mail e senha fornecido.</response>
        [HttpGet()]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<LivroCaixaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] DateTime request)
        => (await _service.ObterAsync(request)).ToActionResult();

    }
}
