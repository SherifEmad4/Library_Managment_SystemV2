namespace Library_Managment.Application.DTOs
{
    public class CreateMemberDto { 
        public string Name { get; set; } = null!; 
        public string Email { get; set; } = null!; 
        public string Password { get; set; } = null!; 
    }
}
