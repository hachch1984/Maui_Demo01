using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace DbEf.EfSqlTable
{
    public class UserDocumentType_EfSqlTable : IEntityTypeConfiguration<UserDocumentType>
    {
        public void Configure(EntityTypeBuilder<UserDocumentType> builder)
        {
            builder.ToTable("UserDocumentType");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100).IsUnicode();
            builder.Property(x => x.Active).IsRequired();


            var categories = new List<UserDocumentType>
        {
            new () { Id = 1, Name = "Dni", Active = true },
            new () { Id = 2, Name = "Pasaporte", Active = true },
            new () { Id = 3, Name = "Libreta Militar", Active = true },
        };

            builder.HasData(categories);

        }
    }
}
