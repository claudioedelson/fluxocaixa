using FluxoDeCaixa.Shared;
using FluxoDeCaixa.Shared.Messages;
using System.ComponentModel.DataAnnotations;

namespace FluxoDeCaixa.Application.Requests.AuthenticationRequests
{
    public class RefreshTokenRequest : BaseRequestWithValidation
    {
        public RefreshTokenRequest(string token) => Token = token;

        /// <summary>
        /// Token de atualização (RefreshToken)
        /// </summary>
        [Required]
        public string Token { get; }

        public override async Task ValidateAsync()
            => ValidationResult = await LazyValidator.ValidateAsync<RefreshTokenRequestValidator>(this);
    }
}
