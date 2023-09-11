using Library.Interfaces;
using Library.Models;
using Library.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace Library.Controllers {
    public class MyBooksController : Controller {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserBookRepository _userBookRepository;
        public MyBooksController(UserManager<AppUser> userManager,IUserBookRepository userBookRepository)
        {
            _userManager = userManager;
            _userBookRepository = userBookRepository;


        }
        // GET
        public async Task<IActionResult> Index()
        {
            
            var currentUser = await _userManager.GetUserAsync(User);
          var userBooks =await  _userBookRepository.GetBooksUser(currentUser.Id);
            return View(userBooks);
        }

        public async Task<IActionResult> RemoveBook(int bookID)
        {
            // Get User ID
            var currentUser = await _userManager.GetUserAsync(User);
            var selectedBook = await _userBookRepository.GetUserById(bookID);
            //delete element from UserBooks
            var deleted= _userBookRepository.Delete(selectedBook);
            var userBooks =await  _userBookRepository.GetBooksUser(currentUser.Id);
            if (deleted == true){
                TempData["SuccessMessage"] = "Book removed successfully";
            }
            else{
                TempData["ErrorMessage"] = "Couldn't remove book";
               
                
            }
            return View("Index", userBooks);
        }
    }
}
