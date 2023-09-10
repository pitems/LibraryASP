using Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace Library.Controllers {
    public class BookController : Controller {
        private readonly IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;

        }
        // GET
        public async Task<IActionResult> Index()
        {
            var book = await _bookRepository.GetAll();
            return View(book);
        }

        public async Task<IActionResult> Details(int id)
        {
            var books = await _bookRepository.GetBookByID(id);
            return View(books);
        }

    }
}
