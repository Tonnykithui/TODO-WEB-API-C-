using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebTODO.ILoginRegisterServices;
using WebTODO.Models;

namespace WebTODO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginRegister _loginRegister;

        public AuthController(ILoginRegister loginRegister)
        {
            _loginRegister = loginRegister;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] Register register)
        {
            if (register == null)
                return BadRequest("Provide registrations details");

            if (ModelState.IsValid)
            {
                var result = await _loginRegister.RegisterAsync(register);
                if (!result.IsError)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Problem with user Registration,Please try again");
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginUserAsync(Login login)
        {
            if (ModelState.IsValid)
            {
                var result = await _loginRegister.LoginAsync(login);

                if (!result.IsError)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest("Unable to login user, please try again");
        }
    }
}