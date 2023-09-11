using Library.Models;
namespace Library.ViewModel {
    public class EditBookViewModel {
        public int id { get; set; }
        public int Amount { get; set; }
        public string BookName { get; set; }
        public int AuthorId { get; set; } // Property to store the selected author's ID

        public string? Comment { get; set; }
        public string? Description { get; set; }
        
        // public IEnumerable<Author> Authors { get; set; } // Property to hold the list of authors
    }
}
