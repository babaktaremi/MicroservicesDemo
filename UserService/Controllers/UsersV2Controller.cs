using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/Users")]
    [ApiController]
    public class UsersV2Controller : UsersController
    {
        public override IActionResult GetUserRegistrationStatus()
        {
            var result = NewUserRegistration.DummyUsers;

            return Ok(result);
        }
    }
}
