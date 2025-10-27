using Library_Managment.Domain.Entities;
using Library_Managment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library_Managment.Infrastructure.Repositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(LibraryDbContext context) : base(context) { }

        public async Task<List<Book>> GetBooksAsync(string? title, string? author, int page, int pageSize)
        {
            var query = _dbSet.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                query = query.Where(b => b.Title.Contains(title));
            if (!string.IsNullOrEmpty(author))
                query = query.Where(b => b.Author.Contains(author));

            return await query
                .OrderBy(b => b.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
