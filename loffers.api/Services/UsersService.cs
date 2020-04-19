using loffers.api.Data;
using loffers.api.Models.Generator;
using Loffers.Server.Services;
using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace loffers.api.Services
{
    public class UsersService : BaseService
    {
        public async Task<int> Update(ConfugurationModel model, string token)
        {
            var userConfiguration = await context.UseConfigurations.FirstOrDefaultAsync(c => c.UserId == token);
            if (userConfiguration != null)
            {
                var existingCOnfiguration = JsonConvert.DeserializeObject<ConfugurationModel>(userConfiguration.Configuration);
                existingCOnfiguration.IsPublisher = model.IsPublisher ?? existingCOnfiguration.IsPublisher;
                existingCOnfiguration.MaxRange = model.MaxRange ?? existingCOnfiguration.MaxRange;
                existingCOnfiguration.UnitOfMeasurement = model.UnitOfMeasurement ?? existingCOnfiguration.UnitOfMeasurement;
                userConfiguration.LastEditedOn = DateTime.Now;
                userConfiguration.Configuration = JsonConvert.SerializeObject(existingCOnfiguration);
            }
            else
            {
                var configuration = new UseConfigurations()
                {
                    UserId = token,
                    LastEditedOn = DateTime.Now,
                    Configuration = JsonConvert.SerializeObject(model)
                };

                context.UseConfigurations.Add(configuration);
            };

            return await context.SaveChangesAsync();
        }

        public async Task<T> Get<T>(string value)
        {
            var userConfiguration = await context.UseConfigurations.FirstOrDefaultAsync(c => c.UserId == value);
            if (userConfiguration != null)
            {
                return JsonConvert.DeserializeObject<T>(userConfiguration.Configuration);
            }

            return Activator.CreateInstance<T>();
        }

        public async Task<object> UpdateSnapshot(SnapshotModel model, string userId)
        {
            var userConfiguration = await context.UserProfileSnapshots.FirstOrDefaultAsync(c => c.UserId == userId);
            if (userConfiguration != null)
            {
                userConfiguration.FullName = model.Name ?? userConfiguration.FullName;
                userConfiguration.DateOfBirth = model.DateOfBirth ?? userConfiguration.DateOfBirth;
                userConfiguration.LastLat = model.LastLat ?? userConfiguration.LastLat;
                userConfiguration.LastLong = model.LastLong ?? userConfiguration.LastLong;
                userConfiguration.PrimaryEmail = model.Email ?? userConfiguration.PrimaryEmail;
                userConfiguration.PrimaryPhone = model.Phone ?? userConfiguration.PrimaryPhone;
            }
            else
            {
                var snapshot = new UserProfileSnapshots()
                {
                    UserId = userId,
                    FullName = model.Name,
                    PrimaryEmail = model.Email,
                    PrimaryPhone = model.Phone,
                    DateOfBirth = model.DateOfBirth,
                    LastLat = model.LastLat,
                    LastLong = model.LastLong,
                    SecurityCodeValidatity = DateTime.Now.AddDays(-10)
                };

                context.UserProfileSnapshots.Add(snapshot);
            };

            return await context.SaveChangesAsync();
        }

        public async Task<UserProfileSnapshots> GetSnapshotForUser(string emailAddress)
        {
            return await context.UserProfileSnapshots.FirstOrDefaultAsync(c => c.PrimaryEmail == emailAddress);
        }

        public async Task<object> ForgottenPasswordEmail(string emailAddress, string email)
        {
            var user = await context.UserProfileSnapshots.FirstOrDefaultAsync(c => c.PrimaryEmail == emailAddress);
            if (user != null)
            {
                var code = RandonService.Instance.GenerateRandomNumber(111111, 999999);
                user.SecurityCode = code.ToString();
                user.SecurityCodeValidatity = DateTime.UtcNow.AddHours(24);
                email = email.Replace("**UserName**", user.FullName);
                email = email.Replace("**Code**", code.ToString());
                await SendEmail(emailAddress, email, "Forgotten password request loffers", "LoFfers accounts", "loffers@sklative.com");
                await context.SaveChangesAsync();
            }

            return 1;
        }

        static async Task SendEmail(string emailAddress, string emailStructure, string subject, string senderName, string senderEmailAddress)
        {
            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress(senderEmailAddress, senderName);
                mail.To.Add(new MailAddress(emailAddress));
                mail.Subject = subject;
                mail.Body = emailStructure;
                mail.IsBodyHtml = true;
                // Can set to false, if you are sending pure text.

                using (SmtpClient smtp = new SmtpClient("smtp.sendgrid.net", 25))
                {
                    smtp.Credentials = new NetworkCredential("banshi003", "Admin@123");
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
        }

        public async Task InvalidateSecurityCode(UserProfileSnapshots existingUser)
        {
            var user = await context.UserProfileSnapshots.FirstOrDefaultAsync(c => c.UserId == existingUser.UserId);
            if (user != null)
            {
                user.SecurityCode = null;
                user.SecurityCodeValidatity = DateTime.UtcNow.AddDays(-1);
                await context.SaveChangesAsync();
            }
        }
    }
}