using Loffers.Server.Data.Authentication;
using Loffers.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Loffers.Server.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AuthenticationController : ParentController
    {
        private UserManager<ApplicationUser> userManager;
        private readonly PublisherService publisherService;
        public AuthenticationController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            publisherService = new PublisherService();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {

                var authClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Khh#$aP$-Z@^aHatke_eKK_!#K(humma_^O_BaaNNAT@@_HAI"));

                var token = new JwtSecurityToken(
                    issuer: "http://dotnetdetail.net",
                    audience: "http://dotnetdetail.net",
                    expires: DateTime.Now.AddDays(50),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                var result = new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    vendor = user.Id,
                    published = await publisherService.IsUserPublisher(model.Username),
                    name = user.Name
                };

                return Ok(new HttpResult(result));
            }

            return Unauthorized(new HttpResult(null, true, new List<Error>() { new Error { Code = HttpResult.ErrorCodes.INVALIDCREDENTIALS, Description = "User name or password is incorrect." } }));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Resgister([FromBody] SignupViewModel model)
        {
            var user = await userManager.CreateAsync(new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email,
                Name = model.Name
            }, model.Password);

            if (user.Succeeded)
            {
                var result = new
                {
                    userName = model.Name
                };

                return Ok(new HttpResult(result));
            }

            var errorResult = new
            {
                status = 403,
                userName = string.Empty,
            };

            return Ok(new HttpResult(errorResult, true, user.Errors.Select(c => new Error() { Code = c.Code, Description = c.Description }).ToList(), null));
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserModel model)
        {
            var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var user = await userManager.FindByNameAsync(token);
            if (user != null)
            {
                user.Email = model.Email;
                user.PhoneNumber = model.Phone;
                user.Name = model.Name;

                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var successResult = new
                    {
                        userName = model.Name
                    };

                    return Ok(new HttpResult(successResult));
                }

                var errorResult = new
                {
                    status = 403,
                    userName = token,
                };

                return Unauthorized(new HttpResult(errorResult, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while updating the user information."), user.Id));
            }

            var notFoundResult = new
            {
                status = 404,
                userName = string.Empty
            };

            return Unauthorized(new HttpResult(notFoundResult, true, HttpResult.SingleError(HttpResult.ErrorCodes.USERDOESNTEXIST, "User doesn't exist.")));
        }

        [Route("detail")]
        public async Task<IActionResult> Detail()
        {
            var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var user = await userManager.FindByNameAsync(token);
            if (user != null)
            {
                return Ok(new HttpResult(new { user.Name, user.Email, user.PhoneNumber }));
            }

            var notFoundResult = new
            {
                status = 404,
                userName = string.Empty
            };

            return Unauthorized(new HttpResult(notFoundResult, true, HttpResult.SingleError(HttpResult.ErrorCodes.USERDOESNTEXIST, "User doesn't exist.")));

        }

        [HttpPut]
        [Route("updatepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UpdatePasswordModel model)
        {
            var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var user = await userManager.FindByNameAsync(token);
            if (user != null)
            {
                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok(new HttpResult(new { Token = user.Id }));
                }

                var errorResult = new
                {
                    status = 403,
                    userName = token,
                };

                return Unauthorized(new HttpResult(errorResult, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while updating the user password."), user.Id));
            }

            var notFoundResult = new
            {
                status = 404,
                userName = string.Empty
            };

            return Unauthorized(new HttpResult(notFoundResult, true, HttpResult.SingleError(HttpResult.ErrorCodes.USERDOESNTEXIST, "User doesn't exist.")));
        }
    }
}
