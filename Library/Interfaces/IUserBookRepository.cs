using Library.Models;
namespace Library.Interfaces {
    public interface IUserBookRepository {
        Task<IEnumerable<UserBooks>> GetBooksUser(string userID);
        Task<UserBooks> GetUserBookByIdAndUser(string userID, int bookID);
        Task<UserBooks> GetUserById(int UserBookId);
        bool Save();
        bool Delete(UserBooks book);
    }
}
