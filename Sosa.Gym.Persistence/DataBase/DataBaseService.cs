using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sosa.Gym.Application.DataBase;
using Sosa.Gym.Domain.Entidades.Cliente;
using Sosa.Gym.Domain.Entidades.Ejercicio;
using Sosa.Gym.Domain.Entidades.Progreso;
using Sosa.Gym.Domain.Entidades.Rutina;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.Persistence.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosa.Gym.Persistence.DataBase
{
    public class DataBaseService : IdentityDbContext<UsuarioEntity, IdentityRole<int>, int>, IDataBaseService
    {
        public DataBaseService(DbContextOptions options) : base(options)
        { 

        }

        public DbSet<EjercicioEntity> Ejercicios { get; set; }
        public DbSet<ClienteEntity> Clientes { get; set; }
        public DbSet<ProgresoEntity> Progresos { get; set; }
        public DbSet<DiasRutinaEntity> DiasRutinas { get; set; }
        public DbSet<RutinaEntity> Rutinas { get; set; }
        public DbSet<UsuarioEntity> Usuarios { get; set; }

        public async Task<bool> SaveAsync()
        {
            return await SaveChangesAsync() > 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            EntityConfiguration(modelBuilder);
        }

        private void EntityConfiguration(ModelBuilder modelBuilder) 
        {
            new UsuarioConfiguration(modelBuilder.Entity<UsuarioEntity>());
            new ClienteConfiguration(modelBuilder.Entity<ClienteEntity>());
            new RutinaConfiguration(modelBuilder.Entity<RutinaEntity>());
            new ProgresoConfiguration(modelBuilder.Entity<ProgresoEntity>());
            new EjercicioConfiguration(modelBuilder.Entity<EjercicioEntity>());
            new DiasRutinaConfiguration(modelBuilder.Entity<DiasRutinaEntity>());
        }
    }
}
