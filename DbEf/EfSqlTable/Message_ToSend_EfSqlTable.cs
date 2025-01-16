using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;

namespace DbEf.EfSqlTable
{
    public class Message_ToSend_EfSqlTable : IEntityTypeConfiguration<Message_ToSend>
    {
        public void Configure(EntityTypeBuilder<Message_ToSend> builder)
        {
            builder.ToTable("Message_ToSend");

            builder.HasKey(x => new { x.UserId, x.Date, x.Time});
            builder.Property(x => x.Date).HasColumnType("date").IsRequired();
            builder.Property(x => x.Time).HasColumnType("time").IsRequired();
            builder.Property(x => x.UserId).HasColumnType("bigint").IsRequired();
            builder.Property(x => x.Message).IsRequired().HasMaxLength(500).IsUnicode();
            

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
