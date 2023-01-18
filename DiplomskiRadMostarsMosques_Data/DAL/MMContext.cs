using DiplomskiRadMostarsMosques_Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Data.DAL
{
    public class MMContext : DbContext
    {
        public MMContext()
            : base("Name=MMConnectionString")
        {
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //one-to-(zero or one)

            modelBuilder
                .Entity<Administrator>()
                .Property(t => t.KorisnikId)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new[]
                        {
                            new IndexAttribute("Index1"),
                            new IndexAttribute("Index2") { IsUnique = true }
                        }));

            modelBuilder.Entity<Administrator>().HasRequired(x => x.Korisnik).WithMany().HasForeignKey(x => x.KorisnikId);
            //modelBuilder.Entity<RegistrovaniKorisnik>().HasRequired(x => x.Korisnik).WithMany().HasForeignKey(x => x.KorisnikID);
            modelBuilder.Entity<Galerija>().HasMany(e => e.SlikaInfos).WithRequired(e => e.Galerija).WillCascadeOnDelete(true);
            
            //many-to-one
        }

        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Korisnik> Korisniks { get; set; }
        public DbSet<Aktivnost> Aktivnosts { get; set; }
        public DbSet<Dogadjaj> Dogadjajs { get; set; }
        public DbSet<Dzemat> Dzemats { get; set; }
        public DbSet<Kanton> Kantons { get; set; }
        public DbSet<KatgorijaVijest> KategorijaVijests { get; set; }
        public DbSet<Lokacija> Lokacijas { get; set; }
        public DbSet<Medzlis> Medzlis { get; set; }
        public DbSet<Objekat> Objekats { get; set; }
        public DbSet<Opstina> Opstinas { get; set; }
        public DbSet<TipObjekta> TipObjektas { get; set; }
        public DbSet<Vijest> Vijests { get; set; }
        public DbSet<Galerija> Galerijas { get; set; }
        public DbSet<SlikaInfo> SlikaInfos { get; set; }
        public DbSet<Jezik> Jeziks { get; set; }
        public DbSet<ObjekatTranslation> ObjekatTranslations { get; set; }
        public DbSet<MedzlisTranslation> MedzlisTranslations { get; set; }
        public DbSet<VijestTranslation> VijestTranslations { get; set; }
        public DbSet<DogadjajiTranslation> DogadjajiTranslations { get; set; }
    }
}