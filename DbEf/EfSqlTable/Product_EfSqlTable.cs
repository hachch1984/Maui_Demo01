using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace DbEf.EfSqlTable
{
    public class Product_EfSqlTable : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100).IsUnicode();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500).IsUnicode();
            builder.Property(x => x.Price).IsRequired().HasColumnType("decimal (10,2)");
            builder.Property(x => x.Active).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();


            builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);


            var products = new List<Product>
        {
            new () { Id = 1, Name = "Laptop", Description = "High-performance laptop", Price = 999.99m, Active = true, CategoryId = 1 },
            new () { Id = 2, Name = "Smartphone", Description = "Latest model smartphone", Price = 699.99m, Active = true, CategoryId = 1 },
            new () { Id = 3, Name = "Headphones", Description = "Noise cancelling headphones", Price = 199.99m, Active = true, CategoryId = 1 },
            new () { Id = 4, Name = "Mouse", Description = "Wireless mouse", Price = 29.99m, Active = true, CategoryId = 1 },
            new () { Id = 5, Name = "Keyboard", Description = "Mechanical keyboard", Price = 89.99m, Active = true, CategoryId = 1 },

            new () { Id = 6, Name = "Novel", Description = "Award-winning fiction novel", Price = 15.99m, Active = true, CategoryId = 2 },
            new () { Id = 7, Name = "Biography", Description = "Biography of a famous scientist", Price = 19.99m, Active = true, CategoryId = 2 },
            new () { Id = 8, Name = "Cookbook", Description = "A collection of gourmet recipes", Price = 35.99m, Active = true, CategoryId = 2 },
            new () { Id = 9, Name = "History", Description = "Detailed history of the 20th century", Price = 40.99m, Active = true, CategoryId = 2 },
            new () { Id = 10, Name = "Science Fiction", Description = "Sci-fi thriller set in space", Price = 14.99m, Active = true, CategoryId = 2 },

            new () { Id = 11, Name = "T-shirt", Description = "Cotton t-shirt", Price = 20.99m, Active = true, CategoryId = 3 },
            new () { Id = 12, Name = "Jeans", Description = "Comfortable blue jeans", Price = 45.99m, Active = true, CategoryId = 3 },
            new () { Id = 13, Name = "Jacket", Description = "Leather jacket", Price = 89.99m, Active = true, CategoryId = 3 },
            new () { Id = 14, Name = "Sneakers", Description = "Running sneakers", Price = 50.99m, Active = true, CategoryId = 3 },
            new () { Id = 15, Name = "Scarf", Description = "Woolen scarf", Price = 19.99m, Active = true, CategoryId = 3 },

            new () { Id = 16, Name = "Blender", Description = "Multi-function blender", Price = 59.99m, Active = true, CategoryId = 4 },
            new () { Id = 17, Name = "Microwave", Description = "Compact microwave", Price = 99.99m, Active = true, CategoryId = 4 },
            new () { Id = 18, Name = "Toaster", Description = "Stainless steel toaster", Price = 29.99m, Active = true, CategoryId = 4 },
            new () { Id = 19, Name = "Coffee Maker", Description = "Espresso coffee maker", Price = 120.99m, Active = true, CategoryId = 4 },
            new () { Id = 20, Name = "Dishwasher", Description = "Energy efficient dishwasher", Price = 399.99m, Active = true, CategoryId = 4 },

            new () { Id = 21, Name = "Football", Description = "Professional football", Price = 25.99m, Active = true, CategoryId = 5 },
            new () { Id = 22, Name = "Tennis Racket", Description = "Carbon fiber tennis racket", Price = 115.99m, Active = true, CategoryId = 5 },
            new () { Id = 23, Name = "Basketball", Description = "Official size basketball", Price = 29.99m, Active = true, CategoryId = 5 },
            new () { Id = 24, Name = "Golf Clubs", Description = "Set of golf clubs", Price = 460.99m, Active = true, CategoryId = 5 },
            new () { Id = 25, Name = "Baseball Bat", Description = "Aluminum baseball bat", Price = 89.99m, Active = true, CategoryId = 5 },
        };

            builder.HasData(products);

        }
    }
}
