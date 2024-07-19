using Domain;
using Domain.src;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<Korisnik, AppRole, Guid, 
                                IdentityUserClaim<Guid>, KorisnikRole, IdentityUserLogin<Guid>, 
                                IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>               //da bismo obezbedili da se svuda koristi Guid u klasama iz Identity package umesto string-a
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Biljka> Biljke { get; set; }
        // Korisnici i Vodici???
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<Vodic> Vodici { get; set; }
        //public DbSet<Admin> Admini { get; set; }
        public DbSet<Obilazak> Obilasci { get; set; }
        public DbSet<PrijavljeniObilazak> PrijavljeniObilasci { get; set; }
        public DbSet<Ruta> Rute { get; set; }
        public DbSet<KoordinateRute> KoordinateRute { get; set; }
        public DbSet<Podrucje> Podrucja { get; set; }
        public DbSet<KoordinatePodrucja> KoordinatePodrucja { get; set; }
        public DbSet<Podrucja_Biljke> Podrucja_Biljke { get; set; }
        public DbSet<Ocena> Ocene { get; set; }
        public DbSet<Komentar> Komentari { get; set; }
        public DbSet<VodicZahtev> VodicZahtevi { get; set; }
        public DbSet<Photo> Slike { get; set; }
        public DbSet<UserFollowing> UserFollowings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Korisnik>()
            .HasMany(korisnik => korisnik.KorisnikRoles)
            .WithOne(korisnikRole => korisnikRole.Korisnik)
            .HasForeignKey(korisnikRole => korisnikRole.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

            builder.Entity<AppRole>()
            .HasMany(role => role.KorisnikRoles)
            .WithOne(korisnikRole => korisnikRole.Role)
            .HasForeignKey(korisnikRole => korisnikRole.RoleId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();


            builder.Entity<Podrucja_Biljke>()
                .HasKey(pb => new { pb.BiljkaID, pb.PodrucjeID });

            builder.Entity<Podrucja_Biljke>()
                .HasOne(pb => pb.Biljka)
                .WithMany(b => b.Podrucja)
                .HasForeignKey(pb => pb.BiljkaID);

            builder.Entity<Podrucja_Biljke>()
                .HasOne(pb => pb.Podrucje)
                .WithMany(p => p.Biljke)
                .HasForeignKey(pb => pb.PodrucjeID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PrijavljeniObilazak>()
                .HasKey(po => new { po.ObilazakID, po.KorisnikID });

            builder.Entity<PrijavljeniObilazak>()
                .HasOne(po => po.Obilazak)
                .WithMany(o => o.Ucesnici)
                .HasForeignKey(po => po.ObilazakID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PrijavljeniObilazak>()
                .HasOne(po => po.Korisnik)
                .WithMany(k => k.PrijavljeniObilasci)
                .HasForeignKey(po => po.KorisnikID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Komentar>()
                .HasOne(o => o.Obilazak)
                .WithMany(c => c.Komentari)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Obilazak>()
                .HasOne(o => o.Podrucje)
                .WithMany(c => c.Obilasci)
                .OnDelete(DeleteBehavior.Cascade);

            // builder.Entity<Obilazak>()
            //     .HasOne(o => o.Ruta)
            //     .WithMany(r => r.Obilasci)
            //     .OnDelete(DeleteBehavior.Cascade);//verovatno ce mora ovo da bi se kaskadno brisao obilazak kad se obrise ruta

            builder.Entity<KoordinatePodrucja>()
                .HasOne(o => o.Podrucje)
                .WithMany(c => c.Koordinate)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Ruta>()
                .HasOne(r => r.Podrucje)
                .WithMany(p => p.Rute)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<KoordinateRute>()
                .HasOne(kr => kr.Ruta)
                .WithMany(r => r.Koordinate)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserFollowing>()
                .HasKey(k => new { k.ObserverId, k.TargetId });

            builder.Entity<UserFollowing>()
                .HasOne(o => o.Observer)
                .WithMany(f => f.Followings)
                .HasForeignKey(o => (Guid)o.ObserverId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserFollowing>()
                .HasOne(t => t.Target)
                .WithMany(f => f.Followers)
                .HasForeignKey(t => (Guid)t.TargetId)
                .OnDelete(DeleteBehavior.Cascade);

            // OBAVEZNO PROVERI DA LI TREBA DA OVDE RESIS BRISANJE POMOCU CASCADE A NE U DELETE KLASU U FOLDER PODRUCJA!!!

            // builder.Entity<UserFollowing>(b =>
            // {
            //     b.HasKey(k => new { k.ObserverId, k.TargetId });

            //     b.HasOne(o => o.Observer)
            //         .WithMany(f => f.Followings)
            //         .HasForeignKey(o => o.ObserverId)
            //         .OnDelete(DeleteBehavior.Cascade);
            //     b.HasOne(t => t.Target)
            //         .WithMany(f => f.Followers)
            //         .HasForeignKey(t => t.TargetId)
            //         .OnDelete(DeleteBehavior.Cascade);
            // });
            
            
        }
    }

   
}