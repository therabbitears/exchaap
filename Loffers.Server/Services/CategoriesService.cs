using Loffers.Server.Data;
using Loffers.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loffers.Server.Services
{
    public class CategoriesService
    {
        private LoffersContext context;

        public CategoriesService()
        {
            context = new LoffersContext();
        }

        public async Task<object> GetAll()
        {
            return await context.Categories.Where(c => c.Active).Select(c => new { c.Id, c.Name, c.Image }).ToListAsync();
        }

        public async Task<Categories> GetOne(string id)
        {
            return await context.Categories.Where(c => c.Id == id).SingleOrDefaultAsync();
        }

        public async Task<object> UserCategories(string token)
        {
            var categories = await context.Categories.Where(c => c.Active).ToListAsync();
            var userCategories = await context.UserCategories.Where(c => c.UserId == token && c.Active).ToListAsync();
            return categories.Select(c => new { c.Id, c.Name, Selected = userCategories.Any(c => c.CategoryId == c.CategoryId && c.Active) }).ToList();
        }

        public async Task<object> SaveUserCategories(List<UserCategoryModel> categories, string userId)
        {
            var userCategories = await context.UserCategories.Include(c => c.Category).Where(c => c.UserId == userId).ToListAsync();
            if (userCategories.Any())
            {
                foreach (var item in userCategories)
                {
                    if (!categories.Any(c => c.Id == item.Category.Id))
                        item.Active = false;
                    else
                        item.Active = true;
                }
            }

            if (categories != null && categories.Any())
            {
                foreach (var item in categories)
                {
                    if (!userCategories.Any(c => c.Category.Id == item.Id))
                    {
                        var newlyAdded = new UserCategories();
                        newlyAdded.CreatedOn = DateTime.Now;
                        newlyAdded.UserId = userId;
                        newlyAdded.Active = true;
                        newlyAdded.LastEditedOn = DateTime.Now;
                        newlyAdded.Category = await GetOne(item.Id);
                        context.Entry(newlyAdded.Category).State = EntityState.Unchanged;
                        context.UserCategories.Add(newlyAdded);
                    }
                }
            }

            await context.SaveChangesAsync();
            return categories;
        }
    }
}