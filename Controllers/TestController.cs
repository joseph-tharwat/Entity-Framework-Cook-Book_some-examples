using EntityFrameworkeCookBook.DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkeCookBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public TestController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [HttpGet("/")]
        public IActionResult testDbContext()
        {
            appDbContext.Users.Add(new User {name="u1" });
            appDbContext.SaveChanges();
            return Ok("done");
            
        }
    }
}
