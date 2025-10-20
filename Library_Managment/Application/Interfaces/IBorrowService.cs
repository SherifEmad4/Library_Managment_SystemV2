using Library_Managment.Application.Common;

namespace Library_Managment.Application.Interfaces
{
    public interface IBorrowService
    {
        Task<Result> BorrowAsync(int memberId, int bookId);
        Task<Result> ReturnAsync(int memberId, int bookId);
    }

}
