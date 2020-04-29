using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace loffers.api.Providers
{
    public class CustomPasswordValidator : IIdentityValidator<string>
    {
        public CustomPasswordValidator()
        {
        }

        public Task<IdentityResult> ValidateAsync(string item)
        {
            return Task.FromResult(IdentityResult.Success);
        }
    }
}