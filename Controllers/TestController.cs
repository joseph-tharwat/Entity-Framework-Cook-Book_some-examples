using EntityFrameworkeCookBook.DataAccessLayer;
using EntityFrameworkeCookBook.DataAccessLayer.Payment;
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
            appDbContext.Users.Where(u => u.id == 1).First().name="u2";
            //appDbContext.addresses.Add(new Address { BlockNum = 1,stName="st", userId = 1 });
            appDbContext.SaveChanges();
            return Ok("done");  
            
        }

        [HttpPost]
        public IActionResult MappingHierachyPost()
        {
            appDbContext.instapayPayments.Add(new InstapayPayment { Name = "inst", Email = "jo@inst.com" });
            appDbContext.creditCardPayments.Add(new CreditCardPayment { Name="cred", CardNumber="123"});
            appDbContext.SaveChanges();
            return Ok("Added");
        }

        [HttpGet]
        public IActionResult MappingHierachyGet()
        {
            var result = appDbContext.paymentMethods.OfType<InstapayPayment>().ToList();
            return Ok(result);
        }

        [HttpGet("all")]
        public IActionResult MappingHierachyGetAll()
        {
            var result = appDbContext.paymentMethods.ToList();
            return Ok(result);

        }
    }
}
