using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sosa.Gym.Domain.Entidades.Progreso;
using Sosa.Gym.Domain.Entidades.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Persistence.Configuration
{
    public class UsuarioConfiguration
    {
        public UsuarioConfiguration(EntityTypeBuilder<UsuarioEntity> entityBuilder)
        {

            entityBuilder.ToTable("Usuarios");
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Nombre).IsRequired();
            entityBuilder.Property(x => x.Apellido).IsRequired();
            entityBuilder.Property(x => x.Email).IsRequired();
            entityBuilder.Property(x => x.Peso).IsRequired();
            entityBuilder.Property(x => x.Objetivo).IsRequired();
            entityBuilder.Property(x => x.Altura).IsRequired();
            entityBuilder.Property(x => x.Edad).IsRequired();
            entityBuilder.Property(x => x.FechaRegistro).IsRequired();

            entityBuilder.HasMany(x => x.Progresos)
                .WithOne(x => x.Usuario).HasForeignKey(x => x.UsuarioId);

            entityBuilder.HasMany(x => x.Rutinas)
            .WithOne(x => x.Usuario).HasForeignKey(x => x.UsuarioId);
        }
    }
}
