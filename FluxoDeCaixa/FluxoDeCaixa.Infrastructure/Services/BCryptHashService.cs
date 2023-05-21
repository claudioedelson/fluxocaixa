﻿using Ardalis.GuardClauses;
using FluxoDeCaixa.Shared.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Infrastructure.Services
{
    public class BCryptHashService : IHashService
    {
        private readonly ILogger<BCryptHashService> _logger;

        public BCryptHashService(ILogger<BCryptHashService> logger) => _logger = logger;

        public bool Compare(string text, string hash)
        {
            Guard.Against.NullOrWhiteSpace(text, nameof(text));
            Guard.Against.NullOrWhiteSpace(hash, nameof(hash));

            try
            {
                return BCrypt.Net.BCrypt.EnhancedVerify(text, hash);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao verificar o HASH com BCrypt");
                throw;
            }
        }

        public string Hash(string text)
        {
            Guard.Against.NullOrWhiteSpace(text, nameof(text));

            try
            {
                return BCrypt.Net.BCrypt.EnhancedHashPassword(text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao gerar o HASH com BCrypt");
                throw;
            }
        }
    }
}