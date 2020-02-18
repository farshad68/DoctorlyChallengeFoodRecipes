using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webservices.Data;
using Webservices.Models;

namespace Webservices
{
    public class RepositoryContext : IdentityDbContext<ApplicationUser>
    {

        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {

        }



        public DbSet<Category> Category { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredient { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<RecipeTokenLookUP> RecipeTokenLookUP { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RecipeIngredient>()
                .HasKey(t => new { t.IngredientID, t.RecipeID });
            builder.Entity<RecipeIngredient>()
                .HasOne(T => T.Ingredient)
                .WithMany(X => X.Recipes)
                .HasForeignKey(Y => Y.IngredientID);
            builder.Entity<RecipeIngredient>()
                .HasOne(T => T.Recipe)
                .WithMany(X => X.Ingredients)
                .HasForeignKey(Y => Y.RecipeID);

            builder.Entity<Unit>()
            .HasIndex(u => u.Name)
            .IsUnique();

            builder.Entity<Ingredient>()
            .HasIndex(i => i.Name)
            .IsUnique();

            builder.Entity<Country>()
            .HasIndex(c => c.Name)
            .IsUnique();

            builder.Entity<Category>()
            .HasIndex(ca => ca.Name)
            .IsUnique();
        }
    }
}
