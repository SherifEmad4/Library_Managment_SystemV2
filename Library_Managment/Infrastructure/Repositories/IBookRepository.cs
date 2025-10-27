using Library_Managment.Domain.Entities;

namespace Library_Managment.Infrastructure.Repositories
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Task<List<Book>> GetBooksAsync(string? title, string? author, int page, int pageSize);
    }
}
