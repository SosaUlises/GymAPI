using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sosa.Gym.Domain.Entidades.Cliente;
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

            entityBuilder.Property(x => x.Nombre).IsRequired();
            entityBuilder.Property(x => x.Apellido).IsRequired();
            entityBuilder.Property(x => x.Dni).IsRequired();

            entityBuilder.HasIndex(x => x.Dni).IsUnique();

            entityBuilder.HasOne(x => x.Cliente)
                .WithOne(x => x.Usuario)
                .HasForeignKey<ClienteEntity>(x => x.UsuarioId);
        }
    }
}
