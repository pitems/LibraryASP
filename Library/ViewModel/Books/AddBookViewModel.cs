using Library.Models;
namespace Library.ViewModel {
    public class AddBookViewModel {
        public int Amount { get; set; }
        public string BookName { get; set; }
        public int AuthorId { get; set; } // Property to store the selected author's ID

        public string? Comment { get; set; }
        public string? Description { get; set; }
    }
}
