using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // <--- Agregar esto
using Microsoft.AspNetCore.Identity; // <--- Agregar esto
using Periodico.Models;
using System;

namespace Periodico.Data
{
    public class PeriodicoContext : IdentityDbContext //DbContext 
    {
        public PeriodicoContext(DbContextOptions<PeriodicoContext> options)
        : base(options)
        {
        }
        public DbSet<Noticia> Noticias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Noticia>(entity =>
            {
                entity.ToTable("noticia");
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Id)
                      .ValueGeneratedOnAdd();
                entity.Property(n => n.Titulo)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(n => n.Tipo)
                      .HasColumnType("smallint");
            });
   
        }
    }
}
