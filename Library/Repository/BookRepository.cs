using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
namespace Library.Repository {
    public class BookRepository : IBookRepository{
        private readonly ApplicationDbContext _context;
        public BookRepository(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<Books>> GetAll()
        {
            return await _context.Books.ToListAsync();
        }
        public async Task<Books> GetBookByID(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(books => books.Id == id);
        }
    }
}
