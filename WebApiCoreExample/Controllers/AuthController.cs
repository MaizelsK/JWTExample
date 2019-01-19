using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace WebApiCoreExample.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly SportContext _context;

        public AuthController(UserService userService, SportContext context)
        {
            _userService = userService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody]User user)
        {
            var token = await _userService.Authenticate(user.Login, user.Password);

            if (string.IsNullOrEmpty(token))
                return BadRequest();

            return Ok(new { token });
        }
    }
}