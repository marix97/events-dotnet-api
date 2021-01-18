using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventAPI.Entities.DatabaseContext
{
    public class EventsAPIContext : DbContext
    {
        public EventsAPIContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<EventGuest> EventGuest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventGuest>()
                .HasKey(x => new { x.EventId, x.GuestId });
            modelBuilder.Entity<EventGuest>()
                .HasOne(e => e.Event)
                .WithMany(e => e.Guests)
                .HasForeignKey(e => e.EventId);
            modelBuilder.Entity<EventGuest>()
                .HasOne(g => g.Guest)
                .WithMany(g => g.Events)
                .HasForeignKey(g => g.GuestId);

            modelBuilder.Entity<Guest>()
                .HasIndex(g => g.Email)
                .IsUnique();
        }
    }
}
