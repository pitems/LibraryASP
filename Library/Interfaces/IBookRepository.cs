using Library.Models;
namespace Library.Interfaces {
    public interface IBookRepository {
        Task<IEnumerable<Books>> GetAll();
        Task<Books> GetBookByID(int id);
    }
    
}
