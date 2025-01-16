using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace DbEf.EfSqlTable
{
    public class User_EfSqlTable : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.ToTable("User");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100).IsUnicode();
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100).IsUnicode();
            builder.Property(x => x.Password).IsRequired().HasMaxLength(100).IsUnicode();
            builder.Property(x => x.UserDocumentTypeId).IsRequired();
            builder.Property(x => x.UserDocumentValue).IsRequired().HasMaxLength(20).IsUnicode();

            builder.HasOne(x => x.UserDocumentType).WithMany().HasForeignKey(x => x.UserDocumentTypeId);

            var users = new List<User>
        {
            new() { Id = 1, Name = "John Doe", Email = "john.doe@email.com", Password = "123" ,UserDocumentTypeId=1,UserDocumentValue="01123456"},
            new () { Id = 2, Name = "Jane Smith", Email = "jane.smith@email.com", Password = "123" ,UserDocumentTypeId=1,UserDocumentValue = "02123456" },
            new () { Id = 3, Name = "Alice Johnson", Email = "alice.johnson@email.com", Password = "123"  ,UserDocumentTypeId=1, UserDocumentValue = "03123456"},
            new () { Id = 4, Name = "Bob Brown", Email = "bob.brown@email.com", Password = "123"  ,UserDocumentTypeId=1, UserDocumentValue = "04123456"},
            new () { Id = 5, Name = "Carol White", Email = "carol.white@email.com", Password = "123"  ,UserDocumentTypeId=1, UserDocumentValue = "05123456"},
            new () { Id = 6, Name = "Dave Black", Email = "dave.black@email.com", Password = "123"  ,UserDocumentTypeId=1, UserDocumentValue = "06123456"},
            new () { Id = 7, Name = "Eve Green", Email = "eve.green@email.com", Password = "123"  ,UserDocumentTypeId=1, UserDocumentValue = "07123456"},
            new () { Id = 8, Name = "Frank Gray", Email = "frank.gray@email.com", Password = "123"  ,UserDocumentTypeId=1, UserDocumentValue = "08123456"},
            new () { Id = 9, Name = "Grace Blue", Email = "grace.blue@email.com", Password = "123"  ,UserDocumentTypeId=1, UserDocumentValue = "09123456"},
            new () { Id = 10, Name = "Henry", Email = "henry@email.com", Password = "123"  ,UserDocumentTypeId=1, UserDocumentValue = "10123456"}
        };

            builder.HasData(users);
        }
    }
}
