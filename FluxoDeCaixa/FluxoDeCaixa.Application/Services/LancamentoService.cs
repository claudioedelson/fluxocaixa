using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using AutoMapper;
using FluxoDeCaixa.Application.Interfaces;
using FluxoDeCaixa.Application.Requests.Lancamento;
using FluxoDeCaixa.Application.Responses;
using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Domain.Enums;
using FluxoDeCaixa.Domain.Repositories;
using FluxoDeCaixa.Shared.Abstractions;

namespace FluxoDeCaixa.Application.Services
{
    public class LancamentoService : ILancamentoService
    {
        private readonly ILancamentoRepository _repository;
        private readonly ILivroCaixaRepository _livroCaixaRepository;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public LancamentoService(ILancamentoRepository repository, ILivroCaixaRepository livroCaixaRepository, IUnitOfWork uow, IMapper mapper)
        {
            _repository = repository;
            _livroCaixaRepository = livroCaixaRepository;
            _uow = uow;
            _mapper = mapper;
        }

        
        public async Task<Result<LancamentoResponse>> LancarAsync(LancamentoRequest request, TipoLancamento tipoLancmento)
        {
            await request.ValidateAsync();

            if (!request.IsValid)
                return Result.Invalid(request.ValidationResult.AsErrors());

            var livro = await _livroCaixaRepository.ObterCriar(request.DataLancamento);
            var lancamento = new Lancamento(request.Descricao,request.DataLancamento,request.ValorLancamento,livro.Id, tipoLancmento);
            livro.AdicionarLancamento(lancamento);

            await _uow.CommitAsync();
            
            return Result.Success(_mapper.Map<LancamentoResponse>(lancamento));
        }
    }
}
