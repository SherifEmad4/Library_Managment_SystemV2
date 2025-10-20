namespace Library_Managment.Application.DTOs
{
    public class UpdateBookDto { 
        public string Title { get; set; } = null!; 
        public string Author { get; set; } = null!; 
        public int PublishedYear { get; set; } 
        public bool IsAvailable { get; set; } 
    }
}
