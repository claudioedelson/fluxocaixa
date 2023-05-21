using FluxoDeCaixa.Shared;
using FluxoDeCaixa.Shared.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
