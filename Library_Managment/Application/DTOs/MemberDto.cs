namespace Library_Managment.Application.DTOs
{
    public class MemberDto { 
        public int Id { get; set; } 
        public string Name { get; set; } = null!; 
        public string Email { get; set; } = null!; 
        public DateTime JoinDate { get; set; 
        } 
    }
}
