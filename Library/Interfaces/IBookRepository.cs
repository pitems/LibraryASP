using Library.Models;
namespace Library.Interfaces {
    public interface IBookRepository {
        Task<IEnumerable<Books>> GetAllBooksWithAuthors();
        Task<IEnumerable<Author>> GetAuthors();
        Task<Books> GetBookByID(int id,bool includeAuthor);
        bool Update(Books book);
        bool Save();
        bool Add(Books books);
        bool Delete(Books books);
        bool Rent(UserBooks rentedBook);
        bool GetUserBookRental(string userID, int bookID);
    }   
    
}
