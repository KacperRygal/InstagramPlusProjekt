using Microsoft.EntityFrameworkCore;

namespace InstPlusEntityFr
{
    public class DbInstagramPlus : DbContext
    {
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<Post> Posty { get; set; }
        public DbSet<Komentarz> Komentarze { get; set; }
        public DbSet<PolubieniePosta> PolubieniaPostow { get; set; }
        public DbSet<PolubienieKomentarza> PolubieniaKomentarzy { get; set; }
        public DbSet<Obserwowany> Obserwowani { get; set; }
        public DbSet<Obserwujacy> Obserwujacy { get; set; }

        //many-many
        public DbSet<TagPostu> TagiPostow { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PolubieniePosta>().HasKey(e => new { e.PostId, e.UzytkownikId }); //czy to jest ok? nie wiem - śmieć daje taki błąd jak dam 2 razy [Key] albo nie dam wcale >:( : The entity type 'PolubienieKomentarza' requires a primary key to be defined. If you intended to use a keyless entity type, call 'HasNoKey' in 'OnModelCreating'. For more information on keyless entity types, see https://go.microsoft.com/fwlink/?linkid=2141943.
            modelBuilder.Entity<PolubienieKomentarza>().HasKey(e => new { e.KomentarzId, e.UzytkownikId });
            modelBuilder.Entity<Obserwujacy>().HasKey(e => new { e.ObserwatorId, e.ObserwowanyId }); //kurwaaaaaaaaa znowu błąd wtf: A key cannot be configured on 'Obserwujacy' because it is a derived type. The key must be configured on the root type 'Obserwowany'. If you did not intend for 'Obserwowany' to be included in the model, ensure that it is not referenced by a DbSet property on your context, referenced in a configuration call to ModelBuilder, or referenced from a navigation on a type that is included in the model.
            modelBuilder.Entity<Obserwowany>().HasKey(e => new { e.ObserwatorId, e.ObserwowanyId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //warto wkleić swojego ConnStr ;)))
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=\"New Database\";Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        }
    }
}
