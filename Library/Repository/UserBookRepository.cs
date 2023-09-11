using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
namespace Library.Repository {
    public class UserBookRepository :IUserBookRepository {
        private readonly ApplicationDbContext _context;
        public UserBookRepository(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<UserBooks>> GetBooksUser(String userID)
        {
            //Rememeber that the virtual elements may be called but need to be included in order to work
          return await  _context.UserBooks.Where(ub => ub.FkUser == userID).Include(ub=> ub.FkBooksNavigation.Author).ToListAsync();
        }
        public async Task<UserBooks> GetUserBookByIdAndUser(String userID, Int32 bookID)
        {
            return await _context.UserBooks.Where(ub => ub.FkUser == userID && ub.FkBooks == bookID).FirstOrDefaultAsync();
        }
        public async Task<UserBooks> GetUserById(Int32 UserBookId)
        {
            return await _context.UserBooks.FirstOrDefaultAsync(books => books.Id == UserBookId);
        }
        public Boolean Delete(UserBooks book)
        {
            _context.UserBooks.Remove(book);
            return Save();
        }
        
        public bool Save()
        {
            try
            {
                return _context.SaveChanges() > 0; // Return true if one or more entities were saved
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency conflicts if necessary
                // For example, you may choose to reload the entity and retry the update
                return false;
            }
            catch (DbUpdateException)
            {
                // Handle other database update errors if necessary
                return false;
            }
        }
    }
}
