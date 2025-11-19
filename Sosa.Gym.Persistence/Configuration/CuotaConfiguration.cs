using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sosa.Gym.Domain.Entidades.Cliente;
using Sosa.Gym.Domain.Entidades.Cuota;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Persistence.Configuration
{
    public class CuotaConfiguration
    {
        public CuotaConfiguration(EntityTypeBuilder<CuotaEntity> entityBuilder)
        {
            entityBuilder.ToTable("Cuotas");
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Monto).IsRequired();
            entityBuilder.Property(x => x.Anio).IsRequired();
            entityBuilder.Property(x => x.Mes).IsRequired();
            entityBuilder.Property(x => x.Estado).IsRequired();

            entityBuilder.HasOne(x => x.Cliente)
                .WithMany(x => x.Cuotas)
                .HasForeignKey(x => x.ClienteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
