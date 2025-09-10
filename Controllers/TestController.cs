using EntityFrameworkeCookBook.DataAccessLayer;
using EntityFrameworkeCookBook.DataAccessLayer.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using EntityFrameworkeCookBook.DataAccessLayer.Query;

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


        [HttpGet("transaction")]
        public async Task<IActionResult> testTransaction()
        {
            await using var transaction = await appDbContext.Database.BeginTransactionAsync();
            var u = appDbContext.Users.Where(u => u.id == 1).First();
            u.name = "new u5";
            await appDbContext.SaveChangesAsync();
            await Task.Delay(7000); //any request need to change this row must wait untill commit
            await transaction.CommitAsync();
            
            return Ok("updated Done");
        }

        [HttpGet("TimeStamp")]
        public async Task<IActionResult> testTimeStamp()
        {
            var u = appDbContext.Users.Where(u => u.id == 1).First();
            u.name = "new u5";
            await Task.Delay(7000); //-> make a change in database
            await appDbContext.SaveChangesAsync();//here will throw exception

            return Ok("updated Done");
        }


        [HttpGet("TimeStampNeglectChanges")]
        public async Task<IActionResult> testTimeStampNeglectChanges()
        {
            var u = appDbContext.Users.Where(u => u.id == 1).First();
            u.name = "new u5";
            await Task.Delay(7000); //-> make a change in database
            try
            {
                await appDbContext.SaveChangesAsync();//here will throw exception 
            }
            catch (Exception ex)
            {
                //neglect the changes
                appDbContext.Entry(u).State = EntityState.Deleted;
                u = appDbContext.Users.Where(u => u.id == 1).First();//get the new version. If there are any incoming logic depends on this entity, they will use the new version
            }

            return Ok("updated Done");
        }

        [HttpGet("NeglectTimeStamp")]
        public async Task<IActionResult> testNeglectTimeStamp()
        {
            var u = appDbContext.Users.Where(u => u.id == 1).First();
            u.name = "new u5";

            await Task.Delay(7000); //-> make a change in database 
            try
            {
                await appDbContext.SaveChangesAsync();
            }   
            catch(Exception e) // concurancy exception
            {
                var newRowVersion = appDbContext.Users.Where(u => u.id == 1).Select(u => u.rowVersion).First(); // get the new value 
                appDbContext.Users.Entry(u).Property(u => u.rowVersion).OriginalValue = newRowVersion;
                u.rowVersion = newRowVersion;
                var result = await appDbContext.SaveChangesAsync(); // insert the new value
            }
            
            return Ok("updated Done");
        }


        [HttpGet("checkQueryWithShadowProperty")]
        public async Task<IActionResult> checkQueryWithShadowProperty()
        {
            var u = await appDbContext.Users.Where(u => EF.Property<string>(u, "CreatedBy") == "Joseph").ToListAsync();
            return Ok(u);
        }



        [HttpGet("clientSideBuildInFunctionWithLINQ")]
        public async Task<IActionResult> clientSideBuildInFunctionWithLINQ()
        {
            var u = await appDbContext.Users.Where(u => u.name.Contains("u1")).ToListAsync();
            return Ok(u);
        }
        private bool MyContains(string name, string match)
        {
            return name.Contains(match);
        }

        [HttpGet("clientSideUserFunctionWithLINQ_error")]
        public async Task<IActionResult> clientSideUserFunctionWithLINQ_error()
        {
            var u = await appDbContext.Users.
                Where(u => MyContains(u.name, "dUser1")).ToListAsync(); // will throw exception
            return Ok(u);
        }

        [HttpGet("clientSideUserFunctionWithLINQ")]
        public IActionResult clientSideUserFunctionWithLINQ()
        {
            var u = appDbContext.Users
                .AsEnumerable()
                .Where(u => MyContains(u.name, "dUser1"))
                .ToList(); //will not get exception

            return Ok(u);
        }



        [HttpGet("FilterQueryExtension")]
        public IActionResult FilterQueryExtension()
        {
            var u = appDbContext.Users.FilterByName("duser1").ToList();
            return Ok(u);
        }


    }
}
