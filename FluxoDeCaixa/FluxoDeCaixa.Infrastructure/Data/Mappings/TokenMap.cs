﻿using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FluxoDeCaixa.Infrastructure.Data.Mappings
{
    public class TokenMap : IEntityTypeConfiguration<Token>
    {
        public void Configure(EntityTypeBuilder<Token> builder)
        {
            builder.ConfigureBaseEntity();

            builder.ToTable("token");

            builder.Property(token => token.UsuarioId)
                .IsRequired();

            builder.Property(token => token.Acesso)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(2048)
                .HasComment("AcessToken");

            builder.Property(token => token.Atualizacao)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(2048)
                .HasComment("RefreshToken");

            builder.Property(token => token.CriadoEm)
                .IsRequired();

            builder.Property(token => token.ExpiraEm)
                .IsRequired();

            builder.Property(token => token.RevogadoEm)
                .IsRequired(false);

            builder.HasOne(token => token.Usuario)
                .WithMany(usuario => usuario.Tokens)
                .HasForeignKey(token => token.UsuarioId);
        }
    }
}
