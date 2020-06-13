using System;
using FoodTruck.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodTruck.Infrastructure.Data
{
    public class FoodTruckContext : DbContext
    {
        public FoodTruckContext(DbContextOptions<FoodTruckContext> options) : base(options) { }

        public DbSet<Core.Models.FoodTruck> FoodTrucks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Core.Models.FoodTruck>()
                .HasKey(c => c.LocationId);
        }
    }
}
