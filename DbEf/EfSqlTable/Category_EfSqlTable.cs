using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace DbEf.EfSqlTable
{
    public class Category_EfSqlTable : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100).IsUnicode();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500).IsUnicode();
            builder.Property(x => x.Active).IsRequired();


            var categories = new List<Category>
        {
            new () { Id = 1, Name = "Electronics", Description = "Devices and gadgets", Active = true },
            new () { Id = 2, Name = "Books", Description = "Fiction and non-fiction books", Active = true },
            new () { Id = 3, Name = "Clothing", Description = "Men's and women's clothing", Active = true },
            new () { Id = 4, Name = "Home & Kitchen", Description = "Home appliances and kitchen gadgets", Active = true },
            new () { Id = 5, Name = "Sports", Description = "Sports equipment and accessories", Active = true } 
        };

            builder.HasData(categories);

        }
    }
}
