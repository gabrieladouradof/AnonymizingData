using Microsoft.AspNetCore.Mvc;

namespace ProtectedDba.Controllers {

    public class HomeController : ControllerBase 
    {
        [Anonymize]
    }

}