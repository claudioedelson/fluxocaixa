using FluxoDeCaixa.API.Extensions;
using FluxoDeCaixa.API.Models;
using FluxoDeCaixa.Application.Interfaces;
using FluxoDeCaixa.Application.Requests.AuthenticationRequests;
using FluxoDeCaixa.Application.Responses;
using FluxoDeCaixa.Shared.Messages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace FluxoDeCaixa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _service;
        public AuthController(IAuthenticationService service) => _service = service;

        /// <summary>
        /// Efetua a autenticação.
        /// </summary>
        /// <param name="request">Endereço de e-mail e senha.</param>
        /// <response code="200">Retorna o token de acesso.</response>
        /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
        /// <response code="404">Quando nenhuma conta é encontrado pelo e-mail e senha fornecido.</response>
        [HttpPost("authenticate")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Authenticate([FromBody] LogInRequest request)
            => (await _service.AuthenticateAsync(request)).ToActionResult();

        /// <summary>
        /// Atualiza um token de acesso.
        /// </summary>
        /// <param name="request">O Token de atualização (RefreshToken).</param>
        /// <response code="200">Retorna um novo token de acesso.</response>
        /// <response code="400">Retorna lista de erros, se a requisição for inválida.</response>
        /// <response code="401">Sem autorização.</response>
        /// <response code="404">Quando nenhum token de acesso é encontrado.</response>
        [HttpPost("refresh-token")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ApiResponse<TokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
            => (await _service.RefreshTokenAsync(request)).ToActionResult();
    }
}
