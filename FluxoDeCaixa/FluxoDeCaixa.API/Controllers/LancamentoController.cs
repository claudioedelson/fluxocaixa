using FluxoDeCaixa.API.Extensions;
using FluxoDeCaixa.API.Models;
using FluxoDeCaixa.Application.Interfaces;
using FluxoDeCaixa.Application.Requests.Lancamento;
using FluxoDeCaixa.Application.Responses;
using FluxoDeCaixa.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace FluxoDeCaixa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class LancamentoController : ControllerBase
    {
        private readonly ILancamentoService _lancamentoService;
        public LancamentoController(ILancamentoService LancamentoService) 
        {
            _lancamentoService = LancamentoService;
        }


        /// <summary>
        /// Realizar Lançamento de Entrada
        /// </summary>
        /// <param name="request">Parametros de Lançamento.</param>
        /// <response code="200">Retorna Lançamento de Entrada.</response>
        /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
        [HttpPost("receber")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<LancamentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Receber([FromBody] LancamentoRequest request)
        => (await _lancamentoService.LancarAsync(request, TipoLancamento.Entrada)).ToActionResult();


        /// <summary>
        /// Realizar Lançamento de Saída
        /// </summary>
        /// <param name="request">Parametros de Lançamento.</param>
        /// <response code="200">Retorna Lançamento de Saída.</response>
        /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
        [HttpPost("pagar")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<LancamentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Pagar([FromBody] LancamentoRequest request)
        =>  (await _lancamentoService.LancarAsync(request, TipoLancamento.Saida)).ToActionResult();
       
    }
}
