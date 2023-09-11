using Library.Interfaces;
using Library.Models;
using Library.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace Library.Controllers {
    public class BookController : Controller {
        private readonly IBookRepository _bookRepository;
        private readonly UserManager<AppUser> _userManager;
        public BookController(IBookRepository bookRepository,UserManager<AppUser> userManager)
        {
            _bookRepository = bookRepository;
            _userManager = userManager;

        }
        // GET
        public async Task<IActionResult> Index()
        {
            var book = await _bookRepository.GetAllBooksWithAuthors();
            return View(book);
        }

        public async Task<IActionResult> Details(int id)
        {
            var books = await _bookRepository.GetBookByID(id,includeAuthor:true);
            return View(books);
        }
        public async Task<IActionResult> Rent(int id)
        {
            //Get Current User ID
            var currentUser = await _userManager.GetUserAsync(User);
            int bookID = id;
            //Get Book
            var bookToRent = await _bookRepository.GetBookByID(id,includeAuthor:true);
            //Check If book is rented
            var existingRental =  _bookRepository.GetUserBookRental(currentUser.Id, bookID);
            if (existingRental == true){
                TempData["ErrorMessage"] = "You've already rented this book.";
                return View("Details", bookToRent);
            }

            //Using the Id will associate the book to the user on another table and update this one
            if (bookToRent != null){
                // Check if the book is available to rent (Amount > Rented)
                if (bookToRent.Rented < bookToRent.Amount){
                    var rentedBook = new UserBooks()
                    {
                        
                        FkBooks = bookID,
                        FkUser = currentUser.Id,
                    };
                    _bookRepository.Rent(rentedBook);
                    bookToRent.Rented++;
                    _bookRepository.Update(bookToRent);
                    return RedirectToAction("Index");
                } else
                {
                    // The book is not available to rent
                    ModelState.AddModelError("", "Sorry, this book is not available for rent.");
                }
            }
            
            
            return RedirectToAction("Details", bookToRent);
        }
        public async Task<IActionResult> Update(int id)
        {
            var books = await _bookRepository.GetBookByID(id,includeAuthor:true);
            var authors = await _bookRepository.GetAuthors();
            if (books == null)
                return View("Error");
            var bookVM = new EditBookViewModel()
            {
                id = books.Id,
                Amount = books.Amount,
                AuthorId = books.AuthorId,
                // Authors = authors,
                Description = books.Description, 
                BookName = books.BookName,
                Comment = books.Comment,
            };
            ViewBag.Authors = authors;
            return View(bookVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(EditBookViewModel bookVM)
        {
            if (!ModelState.IsValid){
                ModelState.AddModelError("", "Failed to edit book");
                return View("Update", bookVM);
            }
            try
            {
                var book = await _bookRepository.GetBookByID(bookVM.id, includeAuthor: true);
                if (book != null)
                {
                    // Update the book properties based on the ViewModel
                    book.Amount = bookVM.Amount;
                    book.AuthorId = bookVM.AuthorId;
                    book.Description = bookVM.Description;
                    book.BookName = bookVM.BookName;
                    book.Comment = bookVM.Comment;

                    // Save the changes to the database
                    _bookRepository.Update(book);

                    // Provide a success message
                    TempData["SuccessMessage"] = "Book updated successfully";

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., database update failure)
                ModelState.AddModelError("", "An error occurred while updating the book. Please try again.");
                // Log the exception for debugging (consider using a logging framework)
            }


            return View(bookVM);
        }
        [HttpPost]
        
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var bookVM = new AddBookViewModel();
            var authors = await _bookRepository.GetAuthors();
            ViewBag.Authors = authors;
            return View(bookVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(AddBookViewModel bookVM)
        {
            if (!ModelState.IsValid){
                ModelState.AddModelError("", "Failed to edit book");
                return View("Add", bookVM);
            }
            var book = new Books
            {
                Amount = bookVM.Amount,
                AuthorId = bookVM.AuthorId,
                Description = bookVM.Description,
                BookName = bookVM.BookName,
                Comment = bookVM.Comment,
            };
            _bookRepository.Add(book);
            return await Task.FromResult<IActionResult>(RedirectToAction("Index"));

        }

        public async Task<IActionResult> Delete(int id)
        {
          var bookdetail=  await _bookRepository.GetBookByID(id,includeAuthor:true);
          return View(bookdetail);

        }

        [HttpPost] [ActionName("Delete")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var bookdetails=  await _bookRepository.GetBookByID(id,includeAuthor:true);
            if (bookdetails is null) return View("Index");
            _bookRepository.Delete(bookdetails);
            return RedirectToAction("Index");
        }

    }
}
