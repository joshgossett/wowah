using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AHWeb.Db
{
    class AuctionsDB : DbContext
    {
        public DbSet<AuctionSnap> AuctionSnaps { get; set; }
        public DbSet<BonusList> BonusLists { get; set; }
        public DbSet<Modifier> Modifiers { get; set; }
        public DbSet<RealmData> RealmDatas { get; set; }
        public DbSet<Auction> Auctions { get; set; }

        public DbSet<AuctionBonusList> AuctionBonusLists { get; set; }
        public DbSet<AuctionModifier> AuctionModifiers { get; set; }
        public DbSet<AuctionSnapRealmData> AuctionSnapRealmDatas { get; set; }


        public AuctionsDB()
        {
            //this.Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.UseMySql("Server=localhost;Database=AuctionsDB;Uid=root;Pwd=jmgjmg;");
#else
            optionsBuilder.UseMySql("Server=wowahdb;Database=AuctionsDB;Uid=root;Pwd=jmgjmg;");
#endif
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Modifier>().HasAlternateKey(x => x.Type);
            modelBuilder.Entity<BonusList>().HasAlternateKey(x => x.BonusListId);
            modelBuilder.Entity<AuctionBonusList>().HasKey(x => new { x.AuctionID, x.BonusListID });
            modelBuilder.Entity<AuctionModifier>().HasKey(x => new { x.AuctionID, x.ModifierID });
            modelBuilder.Entity<AuctionSnapRealmData>().HasKey(x => new { x.AuctionSnapID, x.RealmDataID });
            modelBuilder.Entity<RealmData>().HasAlternateKey(x => x.Realm);
            modelBuilder.Entity<Modifier>().HasAlternateKey(x => new { x.Type, x.Value });

            modelBuilder.Entity<AuctionSnap>()
                .HasIndex(x => x.TimeStamp)
                .IsUnique();
        }
    }
}
