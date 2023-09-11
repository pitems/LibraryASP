using Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace Library.Controllers {
    public class AuthorController : Controller {
        private readonly IAuthorRepository _authorRepository;
        public AuthorController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;

        }
        // GET
        public async Task<IActionResult> Index()
        {
            var authors =await  _authorRepository.GetAuthors();
            return View(authors);
        }
        public async Task<IActionResult> Add()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            return View();
        }
    }
}
