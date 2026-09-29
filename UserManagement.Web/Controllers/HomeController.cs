namespace UserManagement.WebMS.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public ViewResult Index() => View();
}
