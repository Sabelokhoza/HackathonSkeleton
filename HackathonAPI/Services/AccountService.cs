using AutoMapper;
using HackathonAPI.Contracts;
using HackathonAPI.Data;
using HackathonAPI.Models;
using HackathonAPI.Models.AccountModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HackathonAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AccountService(AppDbContext appDbContext, UserManager<User> userManager, IMapper mapper, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<SignInResultDTO> SignInAsync(SignInDTO signInDTO)
        {
            var user = await _userManager.FindByEmailAsync(signInDTO.Email);
            if (user is null)
            {
                return  null;
            }

            var isValidPassword = await _userManager.CheckPasswordAsync(user, signInDTO.Password);
            if (!isValidPassword)
            {
                return null;
            }

            var token = await GenerateToken(user);

            return new SignInResultDTO
            {
                userId = user.Id,
                Name = user.FirstName + " " + user.LastName,
                Token = token
            };
        }



        public async Task<IEnumerable<IdentityError>> SignUpAsync(SignUpDTO signUpDTO)
        {
            var user = _mapper.Map<User>(signUpDTO);
            user.UserName = signUpDTO.Email;
            var result = await _userManager.CreateAsync(user, signUpDTO.Password);

            if (result.Succeeded)
            {
                _ = await _userManager.AddToRoleAsync(user, "Client");
            }

            return result.Errors;
        }


        #region HelperMethods
        private async Task<string> GenerateToken(User user)
        {
            var tokenKey = _configuration.GetValue<string>("TokenKey") ?? throw new Exception("cannot access token key");
            if (tokenKey.Length < 64)
            {
                throw new Exception("Your token key needs to be longer ");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var claims = new List<Claim>()
            {
                new (ClaimTypes.NameIdentifier , user.UserName)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        #endregion

    }
}
