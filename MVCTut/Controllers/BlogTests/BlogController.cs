using Data.Repository;
using MVCTut.ViewModels;
using System.Web;
using System.Web.Mvc;

namespace MVCTut.Controllers.BlogTests
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        //public BlogController()
        //{

        //}
        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        [HttpGet]
        public ViewResult Posts()
        {
            var viewModel = new ListViewModel();
            return View(viewModel);
        }
        [HttpPost]
        public ViewResult Posts(int p = 1)
        {
            var viewModel = new ListViewModel(_blogRepository, p);

            ViewBag.Title = "Latest Posts";
            return View("List", viewModel);
        }
        [HttpPost]
        public ViewResult Category(string category, int p = 1)
        {

            var viewModel = new ListViewModel(_blogRepository, category, "Category", p);

            if (viewModel.Category == null)
                throw new HttpException(404, "Category not found");

            ViewBag.Title = string.Format(@"Latest posts on category ""{0}""",
                                viewModel.Category.Name);
            return View("List", viewModel);
        }
        [HttpPost]
        public ViewResult Tag(string tag, int p = 1)
        {
            var viewModel = new ListViewModel(_blogRepository, tag, "Tag", p);

            if (viewModel.Tag == null)
                throw new HttpException(404, "Tag not found");

            ViewBag.Title = string.Format(@"Latest posts tagged on ""{0}""",
                viewModel.Tag.Name);
            return View("List", viewModel);
        }

        public ViewResult Search(string s, int p = 1)
        {
            ViewBag.Title = string.Format(@"Lists of posts found
                        for search text ""{0}""", s);

            var viewModel = new ListViewModel(_blogRepository, s, "Search", p);
            return View("List", viewModel);
        }
        public ViewResult Post(int year, int month, string title)
        {
            var post = _blogRepository.Post(year, month, title);

            if (post == null)
                throw new HttpException(404, "Post not found");

            if (post.Published == false && User.Identity.IsAuthenticated == false)
                throw new HttpException(401, "The post is not published");

            return View(post);
        }

        [ChildActionOnly]
        public PartialViewResult Sidebars()
        {
            var widgetViewModel = new WidgetViewModel(_blogRepository);
            return PartialView("_Sidebars", widgetViewModel);
        }

    }
}