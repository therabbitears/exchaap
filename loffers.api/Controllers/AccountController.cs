using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using loffers.api.Models;
using loffers.api.Providers;
using loffers.api.Results;
using Loffers.Server.Controllers;
using Loffers.Server.Data.Authentication;
using loffers.api.Services;
using System.IO;
using System.Linq;

namespace loffers.api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ParentController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private UsersService service;

        public AccountController()
        {
            service = new UsersService();
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserManager.PasswordValidator = new CustomPasswordValidator();
            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }


        //[HttpPost]
        //[Route("login")]
        //public async Task<IHttpActionResult> Login([FromBody] LoginModel model)
        //{
        //    var user = await UserManager.FindByNameAsync(model.Username);
        //    if (user != null && await UserManager.CheckPasswordAsync(user, model.Password))
        //    {

        //        var authClaims = new[]
        //        {
        //            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
        //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //        };

        //        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Khh#$aP$-Z@^aHatke_eKK_!#K(humma_^O_BaaNNAT@@_HAI"));

        //        var token = new JwtSecurityToken(
        //            issuer: "http://dotnetdetail.net",
        //            audience: "http://dotnetdetail.net",
        //            expires: DateTime.Now.AddDays(50),
        //            claims: authClaims,
        //            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        //            );

        //        var result = new
        //        {
        //            token = new JwtSecurityTokenHandler().WriteToken(token),
        //            expiration = token.ValidTo,
        //            vendor = user.Id,
        //            published = await publisherService.IsUserPublisher(model.Username),
        //            name = user.Name
        //        };

        //        return Ok(new HttpResult(result));
        //    }

        //    return Unauthorized(new HttpResult(null, true, new List<Error>() { new Error { Code = HttpResult.ErrorCodes.INVALIDCREDENTIALS, Description = "User name or password is incorrect." } }));
        //}

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserManager.PasswordValidator = new CustomPasswordValidator();
            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new HttpResult(new { Token = string.Empty }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, ModelState.FirstOrDefault().Value.Errors.FirstOrDefault().ErrorMessage)));
            }

            ApplicationUser user = null;
            try
            {
                UserManager.PasswordValidator = new CustomPasswordValidator();
                user = new ApplicationUser() { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber, Name = model.Name };

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    return Ok(new HttpResult(new { Token = string.Empty }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, result.Errors.FirstOrDefault())));

                #region Tech debt-- move it somewhere else

                await service.UpdateSnapshot(new Data.SnapshotModel() { Name = model.Name, Email = model.Email, Phone = model.PhoneNumber }, user.Id);

                #endregion

                return Ok(new HttpResult(new { Token = user.Id }, false));
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(user?.Id))
                    await UserManager.DeleteAsync(user);

                return Ok(new HttpResult(new { Token = string.Empty }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while creating account.")));
            }
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IHttpActionResult> Update(UpdateUserModel model)
        {
            var token = User.Identity.Name;
            var user = await UserManager.FindByNameAsync(token);
            if (user != null)
            {
                user.Email = model.Email;
                user.PhoneNumber = model.Phone;
                user.Name = model.Name;

                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var successResult = new
                    {
                        userName = model.Name
                    };

                    return Ok(new HttpResult(successResult));
                }

                return Ok(new HttpResult(new { Token = string.Empty }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, result.Errors.FirstOrDefault())));
            }

            var notFoundResult = new
            {
                status = 404,
                userName = string.Empty
            };

            return Ok(new HttpResult(notFoundResult, true, HttpResult.SingleError(HttpResult.ErrorCodes.USERDOESNTEXIST, "User doesn't exist.")));
        }

        [Route("detail")]
        [HttpGet]
        public async Task<IHttpActionResult> Detail()
        {
            var token = User.Identity.Name;
            var user = await UserManager.FindByNameAsync(token);
            if (user != null)
            {
                return Ok(new HttpResult(new { user.Name, user.Email, user.PhoneNumber }));
            }

            var notFoundResult = new
            {
                status = 404,
                userName = string.Empty
            };

            return Ok(new HttpResult(notFoundResult, true, HttpResult.SingleError(HttpResult.ErrorCodes.USERDOESNTEXIST, "User doesn't exist.")));

        }

        [AllowAnonymous]
        [Route("verifytoken")]
        [HttpGet]
        public async Task<IHttpActionResult> Valid()
        {
            var token = User.Identity.Name;
            if (!string.IsNullOrEmpty(token))
                return Ok(new HttpResult(true, false));

            return Ok(new HttpResult(false, true, HttpResult.SingleError(HttpResult.ErrorCodes.USERDOESNTEXIST, "Invalid token.")));
        }

        [AllowAnonymous]
        [Route("forgot")]
        [HttpPost]
        public async Task<IHttpActionResult> Forgot(ResetPasswordModel email)
        {
            try
            {
                var emailContent = File.ReadAllText(HttpContext.Current.Server.MapPath("~/Email/Forgot.html"));
                var result = await service.ForgottenPasswordEmail(email.EmailAddress, emailContent);
                return Ok(new HttpResult(result, false, token: email.EmailAddress));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(false, true, HttpResult.SingleError(HttpResult.ErrorCodes.USERDOESNTEXIST, "An error occurred while sending forgot password request.")));
            }
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("resetpassword")]
        public async Task<IHttpActionResult> ChangePassword([FromBody] ResetPasswordModel model)
        {
            var existingUser = await service.GetSnapshotForUser(model.EmailAddress);
            if (existingUser != null && existingUser.SecurityCode == model.ResetCode && existingUser.SecurityCodeValidatity > DateTime.UtcNow)
            {
                if (model.ConfirmPassword == model.NewPassword)
                {
                    var user = await UserManager.FindByNameAsync(model.EmailAddress);
                    if (user != null)
                    {
                        UserManager.PasswordValidator = new CustomPasswordValidator();
                        var result = await UserManager.RemovePasswordAsync(user.Id);
                        if (result.Succeeded)
                        {
                            result = await UserManager.AddPasswordAsync(user.Id, model.NewPassword);
                            if (result.Succeeded)
                            {
                                await service.InvalidateSecurityCode(existingUser);
                                return Ok(new HttpResult(new { Token = user.Id }));
                            }
                        }

                        var errorResult = new
                        {
                            status = 403,
                            userName = model.EmailAddress,
                        };

                        return Ok(new HttpResult(errorResult, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while updating the user password."), user.Id));
                    }
                }
            }

            var notFoundResult = new
            {
                status = 404,
                userName = string.Empty
            };

            return Ok(new HttpResult(notFoundResult, true, HttpResult.SingleError(HttpResult.ErrorCodes.USERDOESNTEXIST, "User doesn't exist.")));
        }

        [HttpPut]
        [Route("updatepassword")]
        public async Task<IHttpActionResult> ChangePassword([FromBody] UpdatePasswordModel model)
        {
            var token = User.Identity.Name;
            var user = await UserManager.FindByNameAsync(token);
            if (user != null)
            {
                UserManager.PasswordValidator = new CustomPasswordValidator();
                var result = await UserManager.ChangePasswordAsync(user.Id, model.CurrentPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    return Ok(new HttpResult(new { Token = user.Id }));
                }

                var errorResult = new
                {
                    status = 403,
                    userName = token,
                };

                return Ok(new HttpResult(errorResult, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, result.Errors.FirstOrDefault()), user.Id));
            }

            var notFoundResult = new
            {
                status = 404,
                userName = string.Empty
            };

            return Ok(new HttpResult(notFoundResult, true, HttpResult.SingleError(HttpResult.ErrorCodes.USERDOESNTEXIST, "User doesn't exist.")));
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
