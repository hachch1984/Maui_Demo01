using Microsoft.EntityFrameworkCore;
using Model;

namespace DbEf
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        }


        public DbSet<User> User { get; set; }
        public DbSet<UserDocumentType> UserDocumentType { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }

        public DbSet<Message_ToSend> Message_ToSend { get; set; }
        public DbSet<Message_Sended> Message_Sended { get; set; }
    }
}
