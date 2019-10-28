using Microsoft.EntityFrameworkCore;
using DotMoney.EFExtensions;
using System.IO;
using Xunit;

namespace DotMoney.Tests {

    public class Context : DbContext {
        public DbSet<Product> Products { get; set; }

        public Context() {
            Database.EnsureCreated();
        }

        public Context(DbContextOptions<Context> opts) : base(opts) {
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
        public virtual Money Price { get; set; }
    }

    public class EfTests {

        public EfTests() {
            if (File.Exists("test.db"))
                File.Delete("test.db");
        }

        [Fact]
        public void Test1() {
            using (var context = new Context()) {
                var product = new Product {
                    Name = "Product 1",
                    Price = new Money(12.99m, "USD")
                };

                context.Update(product);
                context.SaveChanges();
            }

            using (var context = new Context()) {
                var products = context.Products.ToListAsync().Result;

                Assert.Equal("Product 1", products[0].Name);
                Assert.Equal(new Money(12.99m, "USD"), products[0].Price);
            }
        }

        [Fact]
        public void LazyLoadingTest() {
            var opts = new DbContextOptionsBuilder<Context>()
                .UseLazyLoadingProxies()
                .Options;

            using (var context = new Context(opts)) {
                var product = new Product {
                    Name = "Product 1",
                    Price = new Money(12.99m, "USD")
                };

                context.Update(product);
                context.SaveChanges();
            }

            using (var context = new Context(opts)) {
                var products = context.Products.ToListAsync().Result;

                Assert.Equal("Product 1", products[0].Name);
                Assert.Equal(new Money(12.99m, "USD"), products[0].Price);
            }
        }
    }
}
