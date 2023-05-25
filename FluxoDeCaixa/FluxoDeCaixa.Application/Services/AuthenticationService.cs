﻿using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluxoDeCaixa.Application.Interfaces;
using FluxoDeCaixa.Application.Requests.AuthenticationRequests;
using FluxoDeCaixa.Application.Responses;
using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Domain.Repositories;
using FluxoDeCaixa.Shared.Abstractions;
using FluxoDeCaixa.Shared.AppSettings;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FluxoDeCaixa.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        #region Constructor

        public AuthenticationService
        (
            IOptions<AuthOptions> authOptions,
            IDateTimeService dateTimeService,
            IHashService hashService,
            ITokenClaimsService tokenClaimsService,
            IUsuarioRepository repository,
            IUnitOfWork uow
        )
        {
            _authOptions = authOptions.Value;
            _dateTimeService = dateTimeService;
            _hashService = hashService;
            _tokenClaimsService = tokenClaimsService;
            _repository = repository;
            _uow = uow;
        }

        #endregion

        #region Fields

        private readonly AuthOptions _authOptions;
        private readonly IDateTimeService _dateTimeService;
        private readonly IHashService _hashService;
        private readonly ITokenClaimsService _tokenClaimsService;
        private readonly IUsuarioRepository _repository;
        private readonly IUnitOfWork _uow;

        #endregion

        #region Methods

        public async Task<Result<TokenResponse>> AuthenticateAsync(LogInRequest request)
        {
            await request.ValidateAsync();
            if (!request.IsValid)
                return Result.Invalid(request.ValidationResult.AsErrors());

            var usuario = await _repository.ObterPorEmailAsync(request.Email);
            if (usuario == null)
                return Result.NotFound("A conta informada não existe.");

            // Verificando se a conta está bloqueada.
            if (usuario.EstaBloqueado(_dateTimeService))
                return Result.Error("A sua conta está bloqueada, entre em contato com o nosso suporte.");

            // Verificando se a senha corresponde a senha criptografada gravada na base de dados.
            if (_hashService.Compare(request.Password, usuario.HashSenha))
            {
                // Gerando as regras (roles).
                var claims = GenerateClaims(usuario);

                // Gerando o token de acesso.
                var (accessToken, createdAt, expiresAt) = _tokenClaimsService.GenerateAccessToken(claims);

                // Gerando o token de atualização.
                var refreshToken = _tokenClaimsService.GenerateRefreshToken();

                // Vinculando o token atualização ao usuário.
                usuario.AdicionarToken(new Token(accessToken, refreshToken, createdAt, expiresAt));

                _repository.Update(usuario);
                await _uow.CommitAsync();

                return Result.Success(new TokenResponse(accessToken, createdAt, expiresAt, refreshToken));
            }

            // Se o login for inválido, será incrementado o número de falhas,
            // se atingido o limite de tentativas de acesso a conta será bloqueada por um determinado tempo.
            var lockedTimeSpan = TimeSpan.FromSeconds(_authOptions.SecondsBlocked);
            usuario.IncrementarFalhas(_dateTimeService, _authOptions.MaximumAttempts, lockedTimeSpan);

            _repository.Update(usuario);
            await _uow.CommitAsync();

            return Result.Error("O e-mail ou senha está incorreta.");
        }

        public async Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            await request.ValidateAsync();
            if (!request.IsValid)
                return Result.Invalid(request.ValidationResult.AsErrors());

            var usuario = await _repository.ObterPorTokenAtualizacaoAsync(request.Token);
            if (usuario == null)
                return Result.NotFound("Nenhum token encontrado.");

            // Verificando se o token de atualização está expirado.
            var token = usuario.Tokens.FirstOrDefault(t => t.Atualizacao == request.Token);
            if (token == null || token.EstaValido(_dateTimeService))
                return Result.Error("O token inválido ou expirado.");

            // Gerando as regras (roles).
            var claims = GenerateClaims(usuario);

            // Gerando um novo token de acesso.
            var (accessToken, createdAt, expiresAt) = _tokenClaimsService.GenerateAccessToken(claims);

            // Revogando (cancelando) o token de atualização atual.
            token.Revogar(createdAt);

            // Gerando um novo token de atualização.
            var newRefreshToken = _tokenClaimsService.GenerateRefreshToken();

            // Vinculando o novo token atualização ao usuário.
            usuario.AdicionarToken(new Token(accessToken, newRefreshToken, createdAt, expiresAt));

            _repository.Update(usuario);
            await _uow.CommitAsync();

            return Result.Success(new TokenResponse(accessToken, createdAt, expiresAt, newRefreshToken));
        }

        private static Claim[] GenerateClaims(Usuario usuario) => new[]
        {
        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Sub, usuario.Nome, ClaimValueTypes.String),
        new Claim(JwtRegisteredClaimNames.Email, usuario.Email.ToString(), ClaimValueTypes.Email)
    };

        #endregion
    }
}
