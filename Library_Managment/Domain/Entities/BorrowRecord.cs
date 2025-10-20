using Library_Managment.Domain.Entities;

namespace Library_Managment.Domain.Entities
{
    public class BorrowRecord
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // Navigation properties
        public Book Book { get; set; } = null!;
        public Member Member { get; set; } = null!;  
    }
}
