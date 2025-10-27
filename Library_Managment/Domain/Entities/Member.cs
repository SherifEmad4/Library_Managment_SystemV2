using Library_Managment.Domain.Entities;

namespace Library_Managment.Domain.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime JoinDate { get; set; }

        //public string Role { get; set; } = "Member"; // Default role is "Member"

        public ICollection<BorrowRecord>? BorrowRecords { get; set; }
    }

}
