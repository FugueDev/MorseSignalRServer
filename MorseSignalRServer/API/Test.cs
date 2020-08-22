using Microsoft.AspNetCore.Mvc;

namespace MorseSignalRServer.API
{
    public class Test : Controller
    {
        // GET
        public IActionResult Index()
        {
            return Ok(); 
        }
    }
}