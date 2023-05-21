﻿using FluxoDeCaixa.Domain.ValuesObjects;
using FluxoDeCaixa.Shared.Abstractions;
using System;
using System.Collections.Generic;

namespace FluxoDeCaixa.Domain.Entities
{
    public class Usuario : BaseEntity, IAggregateRoot
    {
        private readonly List<Token> _tokens = new();

        public Usuario(string nome, string email, string hashSenha)
        {
            Nome = nome;
            Email = email;
            HashSenha = hashSenha;
        }

        private Usuario() // ORM
        {
        }

        public string Nome { get; private init; }
        public string Email { get; private init; }
        public string HashSenha { get; private init; }
        public DateTime? UltimoAcessoEm { get; private set; }
        public DateTime? BloqueioExpiraEm { get; private set; }
        public int NumeroFalhasAoAcessar { get; private set; }

        public IReadOnlyList<Token> Tokens => _tokens.AsReadOnly();

        public void AdicionarToken(Token token) => _tokens.Add(token);

        public void DefinirUltimoAcesso(DateTime dataUltimoAcesso) => UltimoAcessoEm = dataUltimoAcesso;

        /// <summary>
        /// Indica se a conta do usuário está bloqueada.
        /// </summary>
        /// <param name="dateTimeService"></param>
        /// <returns>Verdadeiro se a conta estiver bloqueada; caso contrário, falso.</returns>
        public bool EstaBloqueado(IDateTimeService dateTimeService) => BloqueioExpiraEm > dateTimeService.Now;

        /// <summary>
        /// Incremenenta o número de acessos que falharam.
        /// Quando é atingido o limite de acessos a conta será bloqueada por um tempo.
        /// </summary>
        /// <param name="dateTimeService"></param>
        /// <param name="numeroTentativas">Número máximo de tentativas até a conta ser bloqueada.</param>
        /// <param name="lockedTimeSpan">Determinado tempo em que a conta ficará bloqueada.</param>
        public void IncrementarFalhas(IDateTimeService dateTimeService, int numeroTentativas, TimeSpan lockedTimeSpan)
        {
            if (EstaBloqueado(dateTimeService))
                return;

            NumeroFalhasAoAcessar++;

            if (NumeroFalhasAoAcessar == numeroTentativas)
            {
                NumeroFalhasAoAcessar = 0;
                BloqueioExpiraEm = dateTimeService.Now.Add(lockedTimeSpan);
            }
        }
    }
}
