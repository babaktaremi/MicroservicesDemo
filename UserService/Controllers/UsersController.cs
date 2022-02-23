using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("RegistrationStatus")]
        public virtual IActionResult GetUserRegistrationStatus()
        {
            var result = UserRegistration.DummyUsers;

            return Ok(result);
        }
    }
}
