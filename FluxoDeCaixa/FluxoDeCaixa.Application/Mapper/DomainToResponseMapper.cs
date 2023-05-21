using AutoMapper;
using FluxoDeCaixa.Application.Responses;
using FluxoDeCaixa.Domain.Entities;

namespace FluxoDeCaixa.Application.Mapper
{
    public class DomainToResponseMapper : Profile
    {
        public DomainToResponseMapper()
        {
            CreateMap<Lancamento, LancamentoResponse>();
            CreateMap<LivroCaixa, LivroCaixaResponse>(MemberList.Destination)
                .ForMember(dest=> dest.DataMovimento, opt=> opt.MapFrom(src=> src.DataMovimento))
                .ForMember(dest=> dest.SaldoAnterior, opt=> opt.MapFrom(src=> src.SaldoAnterior))
                .ForMember(dest=> dest.ValorTotal, opt=> opt.MapFrom(src=> src.Lancamentos.Sum(s=> s.ValorLancamento) + src.SaldoAnterior))
                ;
        }
    }
}
