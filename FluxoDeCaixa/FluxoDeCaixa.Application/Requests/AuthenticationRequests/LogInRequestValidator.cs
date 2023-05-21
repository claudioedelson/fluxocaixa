using FluentValidation;
using FluxoDeCaixa.Shared.Extensions;

namespace FluxoDeCaixa.Application.Requests.AuthenticationRequests
{
    public class LogInRequestValidator : AbstractValidator<LogInRequest>
    {
        public LogInRequestValidator()
        {
            RuleFor(req => req.Email)
                .NotEmpty()
                .IsValidEmailAddress()
                .MaximumLength(100);

            RuleFor(req => req.Password)
                .NotEmpty()
                .MinimumLength(4);
        }
    }
}
