using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Library_Managment.Infrastructure.Data
{
    public class LibraryDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
    {
        public LibraryDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();

            optionsBuilder.UseSqlServer("Data Source=DESKTOP-JHGAGJ5;Initial Catalog=Library_Managment;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;");

            return new LibraryDbContext(optionsBuilder.Options);
        }
    }
}
