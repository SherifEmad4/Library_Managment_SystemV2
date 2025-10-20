using Library_Managment.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library_Managment.Infrastructure.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<BorrowRecord> BorrowRecords { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Member>()
                .HasIndex(m => m.Email)
                .IsUnique();

            builder.Entity<BorrowRecord>()
                .HasOne(b => b.Book)
                .WithMany(bk => bk.BorrowRecords)
                .HasForeignKey(b => b.BookId);

            builder.Entity<BorrowRecord>()
                .HasOne(b => b.Member)
                .WithMany(m => m.BorrowRecords)
                .HasForeignKey(b => b.MemberId);
        }
    }
}
