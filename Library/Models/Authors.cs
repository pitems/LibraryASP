namespace Library.Models {
    public class Author
    {
        public int ID { get; set; } // Primary key
        public string AuthorName { get; set; }
        public int BooksWritten { get; set; }
        public int Popularity { get; set; }

        // Navigation property to represent the books written by this author
        public ICollection<Books> Books { get; set; } = new List<Books>();
    }
}
