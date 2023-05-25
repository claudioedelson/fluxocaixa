using FluxoDeCaixa.Shared;
using FluxoDeCaixa.Shared.Messages;
using System.ComponentModel.DataAnnotations;

namespace FluxoDeCaixa.Application.Requests.AuthenticationRequests
{
    public class LogInRequest : BaseRequestWithValidation
    {
        public LogInRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; }

        [Required]
        [MinLength(4)]
        [DataType(DataType.Password)]
        public string Password { get; }

        public override async Task ValidateAsync()
            => ValidationResult = await LazyValidator.ValidateAsync<LogInRequestValidator>(this);
    }
}
