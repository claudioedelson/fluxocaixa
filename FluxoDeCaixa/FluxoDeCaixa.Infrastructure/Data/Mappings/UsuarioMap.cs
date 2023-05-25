using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FluxoDeCaixa.Infrastructure.Data.Mappings
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ConfigureBaseEntity();

            builder.ToTable("usuario");

            builder.Property(usuario => usuario.Nome)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(30);

           
            builder.Property(usuario => usuario.Email)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasMaxLength(100);
            
            builder.Property(usuario => usuario.HashSenha)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(60);

            builder.Property(usuario => usuario.UltimoAcessoEm)
                .IsRequired(false);

            builder.Property(usuario => usuario.BloqueioExpiraEm)
                .IsRequired(false);

            builder.Property(usuario => usuario.NumeroFalhasAoAcessar)
                .IsRequired();

            builder.Navigation(usuario => usuario.Tokens)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
