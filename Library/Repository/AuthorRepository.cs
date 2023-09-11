using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
namespace Library.Repository {
    public class AuthorRepository : IAuthorRepository{
        private readonly ApplicationDbContext _context;
        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Author>> GetAuthors()
        {
            return await _context.Authors.ToListAsync();
        }
    }
}
