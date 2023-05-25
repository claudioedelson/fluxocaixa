using FluxoDeCaixa.Shared.Abstractions;

namespace FluxoDeCaixa.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
