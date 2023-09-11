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
        public async Task<IEnumerable<Books>> GetAllBooksWithAuthors()
        {
            return await _context.Books.Include(b=> b.Author).ToListAsync();
        }
        //List of Authors move later to another repository
        public async Task<IEnumerable<Author>> GetAuthors()
        {
            return await _context.Authors.ToListAsync();
        }
        public async Task<Books> GetBookByID(int id, bool includeAuthor = false)
        {
            var query = _context.Books.AsQueryable();

            if (includeAuthor)
            {
                query = query.Include(b => b.Author);
            }

            return await query.FirstOrDefaultAsync(books => books.Id == id);
        }

        public Boolean Update(Books book)
        {
            try
            {
                _context.Entry(book).State = EntityState.Modified;
                return Save(); // Call the Save method to save changes
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

        public Boolean Add(Books books)
        {
            _context.Add(books);
            return Save();
        }
        public Boolean Delete(Books books)
        {
            _context.Remove(books);
            return Save();
        }
        public Boolean Rent(UserBooks rentedBook)
        {
            _context.UserBooks.Add(rentedBook);
            return Save();
        }
        public Boolean GetUserBookRental(String userID, Int32 bookID)
        {
            return _context.UserBooks.Any(ub => ub.FkUser == userID && ub.FkBooks == bookID);
        }
    }
}
