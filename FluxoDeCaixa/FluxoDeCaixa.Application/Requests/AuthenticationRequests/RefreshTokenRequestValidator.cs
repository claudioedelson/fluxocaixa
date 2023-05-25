using FluentValidation;

namespace FluxoDeCaixa.Application.Requests.AuthenticationRequests
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
            => RuleFor(req => req.Token).NotEmpty();
    }
}
