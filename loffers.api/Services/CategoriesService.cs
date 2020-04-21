using loffers.api.Models.Generator;
using Loffers.Server.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Loffers.Server.Services
{
    public class CategoriesService : BaseService
    {
        public CategoriesService(LoffersContext context = null) : base(context)
        {
        }

        public async Task<object> GetAll()
        {
            return await context.Categories.Include(c => c.ParentCategories).Where(c => c.Active).Select(c => new { c.Id, c.Name, c.Image, ParentId = c.ParentCategories != null ? c.ParentCategories.Id : null }).ToListAsync();
        }

        public async Task<Categories> GetOne(string id)
        {
            return await context.Categories.Where(c => c.Id == id).SingleOrDefaultAsync();
        }

        public async Task<object> UserCategories(string token)
        {
            var categories = await context.Categories.Where(c => c.Active).ToListAsync();
            var userCategories = await context.UserCategories.Where(c => c.UserId == token && c.Active).ToListAsync();
            return categories.Select(c => new { c.Id, c.Name, Selected = userCategories.Any(d => d.CategoryID == c.CategoryID && c.Active) }).ToList();
        }

        public async Task<object> SaveUserCategories(List<UserCategoryModel> categories, string userId)
        {
            var userCategories = await context.UserCategories.Include(c => c.Categories).Where(c => c.UserId == userId).ToListAsync();
            if (userCategories.Any())
            {
                foreach (var item in userCategories)
                {
                    if (!categories.Any(c => c.Id == item.Categories.Id))
                        item.Active = false;
                    else
                        item.Active = true;
                }
            }

            if (categories != null && categories.Any())
            {
                foreach (var item in categories)
                {
                    if (!userCategories.Any(c => c.Categories.Id == item.Id))
                    {
                        var newlyAdded = new UserCategories();
                        newlyAdded.CreatedOn = DateTime.Now;
                        newlyAdded.UserId = userId;
                        newlyAdded.Active = true;
                        newlyAdded.LastEditedOn = DateTime.Now;
                        newlyAdded.Categories = await GetOne(item.Id);
                        context.Entry(newlyAdded.Categories).State = EntityState.Unchanged;
                        context.UserCategories.Add(newlyAdded);
                    }
                }
            }

            await context.SaveChangesAsync();
            return categories;
        }
    }
}