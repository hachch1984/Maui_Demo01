using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace DbEf.EfSqlTable
{
    public class Message_Sended_EfSqlTable : IEntityTypeConfiguration<Message_Sended>
    {
        public void Configure(EntityTypeBuilder<Message_Sended> builder)
        {
            builder.ToTable("Message_Sended");

            builder.HasKey(x => new { x.UserId, x.Date, x.Time });

            builder.Property(x => x.Date).HasColumnType("date").IsRequired();
            builder.Property(x => x.Time).HasColumnType("time").IsRequired();
            builder.Property(x => x.UserId).HasColumnType("bigint").IsRequired();
            builder.Property(x => x.Message).IsRequired().HasMaxLength(500).IsUnicode();

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
