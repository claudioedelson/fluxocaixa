using Ardalis.Result;
using AutoMapper;
using FluxoDeCaixa.Application.Interfaces;
using FluxoDeCaixa.Application.Responses;
using FluxoDeCaixa.Domain.Repositories;

namespace FluxoDeCaixa.Application.Services
{
    public class LivroCaixaService : ILivroCaixaService
    {
        private readonly ILivroCaixaRepository _repository;
        private readonly IMapper _mapper;

        public LivroCaixaService(ILivroCaixaRepository repository, IMapper mapper) 
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<LivroCaixaResponse>> ObterAsync(DateTime data)
        {
            var livro = await _repository.Obter(data);

            return (_mapper.Map<LivroCaixaResponse>(livro));
        }
    }
}
