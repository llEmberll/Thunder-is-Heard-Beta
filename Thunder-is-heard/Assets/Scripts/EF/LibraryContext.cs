using Microsoft.EntityFrameworkCore;


    public class LibraryContext : DbContext
    {
        public DbSet<Book> Book { get; set; }

        public DbSet<Publisher> Publisher { get; set; }

        public DbSet<Base> Base { get; set; }

        public DbSet<Build> Build { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        optionsBuilder.UseMySQL(
            $"server={Config.dataBase["host"]};" +
            $"port={Config.dataBase["port"]};" +
            $"database={Config.dataBase["database"]};" +
            $"username={Config.dataBase["username"]};" +
            $"password={Config.dataBase["password"]}"
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Title).IsRequired();
                entity.HasOne(d => d.Publisher)
                  .WithMany(p => p.Books);
            });

            modelBuilder.Entity<Base>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.Name).IsRequired();
                entity.HasMany(d => d.Builds);
            });

        modelBuilder.Entity<Build>()
            .HasMany(e => e.Bases);
    }

    }



