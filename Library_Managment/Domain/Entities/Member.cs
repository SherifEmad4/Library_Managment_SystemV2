using Library_Managment.Domain.Entities;

namespace Library_Managment.Domain.Entities
{
    public class Member
    {
        public int Id { get; set; }               // Primary Key
        public string Name { get; set; } = null!; // Member's full name
        public string Email { get; set; } = null!; // Unique email for login
        public string PasswordHash { get; set; } = null!; // Hashed password
        public DateTime JoinDate { get; set; }

        // Navigation property (related borrow records)
        public ICollection<BorrowRecord>? BorrowRecords { get; set; }
    }
}
