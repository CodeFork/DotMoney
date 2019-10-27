using Microsoft.EntityFrameworkCore;
using NetMoney;
using NetMoneyEF;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NetMoneyTests {

    public class Context : DbContext {
        public DbSet<Product> Products { get; set; }

        public Context() {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Filename=test.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Product>()
                .OwnsOneMoney(p => p.Price);
        }
    }

    public class Product {
        public int Id { get; set; }
        public string Name { get; set; }
        public Money Price { get; set; }
    }

    public class EfTests {
        [Fact]
        public void Test1() {
            using var context = new Context();

            var product = new Product {
                Name = "Product 1",
                Price = new Money(12.99m, "USD")
            };

            context.Update(product);
            context.SaveChanges();

            var products = context.Products.ToListAsync().Result;

            Assert.Equal(product.Name, products[0].Name);
            Assert.Equal(product.Price, products[0].Price);
        }
    }
}
