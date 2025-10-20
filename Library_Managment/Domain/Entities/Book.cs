namespace Library_Managment.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int PublishedYear { get; set; }
        public bool IsAvailable { get; set; } = true;

        // Navigation
        public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
    }
}
