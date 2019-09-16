using Data.Repository;
using System.Web.Mvc;

namespace MVCTut.Controllers
{
    public class HomeController : Controller
    {
        private BranchRepository _branchRepository;
        private TellerPostingRepository _tellerPostingRepo;
        private TellerManagementRepository _tellerRepo;

        public HomeController()
        {
            _branchRepository = new BranchRepository();
            _tellerPostingRepo = new TellerPostingRepository();
            _tellerRepo = new TellerManagementRepository();
        }
        public ActionResult Index()
        {
            Session["BranchCount"] = _branchRepository.GetAll().Count;
            Session["TPostCount"] = _tellerPostingRepo.GetAll().Count;
            Session["TCount"] = _tellerRepo.GetAll().Count;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}