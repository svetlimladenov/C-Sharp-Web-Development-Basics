using Microsoft.EntityFrameworkCore;
using MishMashWebApp.Models;

namespace MishMashWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Channel> Channels { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<UserInChannel> UserInChannel { get; set; }

        public ApplicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=MishMash;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChannelTag>()
                .HasKey(x => new {x.ChannelId, x.TagId});

            modelBuilder.Entity<Channel>()
                .HasMany(x => x.ChannelTags)
                .WithOne(x => x.Channel);

            modelBuilder.Entity<Tag>()
                .HasMany(x => x.TagChannels)
                .WithOne(x => x.Tag);
        }
    }
}
