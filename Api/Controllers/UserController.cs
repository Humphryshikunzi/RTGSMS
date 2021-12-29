using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rtgsms.Models;
using RtgsmsApi.Constants;
using RtgsmsApi.Data;
using RtgsmsApi.Models.Dtos;
using RtgsmsApi.Models.UserEntity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks; 

namespace Rtgsms.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {

        #region fields
        private SignInManager<AppUser> _signInManager;
        private  RtgsmsDbContext  _rtgsmsDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppSettings _appSettings;
        #endregion

        public UserController(SignInManager<AppUser> signInManager, RtgsmsDbContext rtgsmsDbContext, IOptions<AppSettings> appSettings, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _rtgsmsDbContext = rtgsmsDbContext;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]AppUser userEntity)
        {
            userEntity.TwoFactorEnabled = false;
            var result = await _signInManager.UserManager.CreateAsync(userEntity, userEntity.PasswordHash);
            if (result.Succeeded)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(userEntity.Email);
                await _signInManager.UserManager.AddToRoleAsync(user, user.Role);
                userEntity.AuthKey = GenerateJwtToken(user); 
                return Ok(userEntity);
            }
            return BadRequest();
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = Roles.Admin)]
        [HttpGet("getall")]
        public IActionResult GetUsers()
        {
            return Ok(_rtgsmsDbContext.Users);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetUser(string id)
        {
            var user = _rtgsmsDbContext.Users.FirstOrDefault(user => user.Id == id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(AppUser appUser)
        {
            _rtgsmsDbContext.Entry(appUser).State = EntityState.Modified;

            try
            {
                await _rtgsmsDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_rtgsmsDbContext.Users.Any(u => u.Id == appUser.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(appUser);
        }

        [HttpDelete("delete")]
        public IActionResult Delete(string id)
        {
            var user = _rtgsmsDbContext.Users.FirstOrDefault(user => user.Id == id);
            if (user != null)
            {
                _rtgsmsDbContext.Remove(user);
                _rtgsmsDbContext.SaveChanges();
                return Ok(user);
            }
            return BadRequest();
        }

        [HttpPost("authenticateUser")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]AuthDto authDto)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(authDto.Email);
            var result = await _signInManager.PasswordSignInAsync(user, authDto.Password, false, false);
            if (result.Succeeded)
            {
                var userData = _rtgsmsDbContext.Users.FirstOrDefault(user => user.Email == authDto.Email);
                userData.AuthKey = GenerateJwtToken(user);
                return Ok(userData);
            }
            return BadRequest();
        }

        [HttpPost("recoverPassword")]
        [AllowAnonymous]
        public IActionResult SendPasswordRecoveryMail(AuthDto  user)
        {
           //var code = EmailServices.PasswordRecoveryMail(user.Email);
            return Ok();
        }

        private string GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(30);
            var token = new JwtSecurityToken("Rtgsms.com", claims: claims, expires: expires, signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
