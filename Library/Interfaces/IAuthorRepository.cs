using Library.Models;
namespace Library.Interfaces {
    public interface IAuthorRepository {
        Task<IEnumerable<Author>> GetAuthors();
    }
}
