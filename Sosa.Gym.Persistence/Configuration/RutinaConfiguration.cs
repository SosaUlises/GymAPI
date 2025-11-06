using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sosa.Gym.Domain.Entidades.Rutina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Persistence.Configuration
{
    public class RutinaConfiguration
    {
        public RutinaConfiguration(EntityTypeBuilder<RutinaEntity> entityBuilder)
        {
            entityBuilder.ToTable("Rutinas");
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Nombre).IsRequired();
            entityBuilder.Property(x => x.Descripcion).IsRequired();
            entityBuilder.Property(x => x.FechaCreacion).IsRequired();

            entityBuilder.HasOne(x => x.Usuario).WithMany(x=>x.Rutinas).HasForeignKey(x=>x.UsuarioId);

        }
    }
}
