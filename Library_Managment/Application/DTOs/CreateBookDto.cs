namespace Library_Managment.Application.DTOs
{
    public class CreateBookDto 
    { 
        public string Title { get; set; } = null!; 
        public string Author { get; set; } = null!; 
        public int PublishedYear { get; set; } 
    }
}
