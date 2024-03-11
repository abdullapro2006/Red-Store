using Microsoft.AspNetCore.Mvc;
using RedStore.Database;

namespace RedStore.Contollers.Admin
{
    [Route("admin/sizes")]
    public class SizeController : Controller
    {
        private readonly RedStoreDbContext _redStoreDbContext;

        public SizeController(RedStoreDbContext redStoreDbContext)
        {
            _redStoreDbContext = redStoreDbContext;
        }

        [HttpGet]
        public IActionResult Sizes()
        {
            var colors = _redStoreDbContext.Sizes.ToList();

            return View("Views/Admin/Size/Sizes.cshtml", colors);
        }
    }
}
