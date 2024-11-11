
using HackathonAPI.Contracts;
using HackathonAPI.Models;
using HackathonAPI.Models.AccountModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Security.Cryptography;
using System.Text;

namespace HackathonAPI.Controllers
{
   
    public class AccountController: BaseApiController 
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<IEnumerable<IdentityError>>> SignUpAsync(SignUpDTO signUpDTO)
        {
            var result = await _accountService.SignUpAsync(signUpDTO);
            return Ok(result);
        }
        [HttpPost("signin")]
        public async Task<ActionResult<SignInResultDTO>> SignInAsync(SignInDTO signInDTO)
        {
            var result = await _accountService.SignInAsync(signInDTO);
            return Ok(result);
        }
    }
}
