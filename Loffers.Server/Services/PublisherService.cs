using Loffers.Server.Errors;
using Loffers.Server.Models;
using Loffers.Server.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Loffers.Server.Services
{
    /// <summary>
    /// PublisherService
    /// </summary>
    public class PublisherService : BaseService
    {
        public async Task<object> Save(Publishers publishe)
        {
            await context.Publishers.AddAsync(publishe);
            await context.SaveChangesAsync();
            return new { publishe.Id, publishe.Name };
        }

        public async Task<Publishers> GetPublisherForUser(string token)
        {
            return await context.Publishers.FirstOrDefaultAsync(c => c.CreatedBy == token);
        }

        public async Task<object> GetPublishersForUser(string token)
        {
            return await context.Publishers.Include(c => c.PublisherLocations).ThenInclude(d => d.Location).Where(c => c.CreatedBy == token).Select(c => new
            {
                c.Id,
                c.Name,
                c.Image,
                c.Description,
                Locations = c.PublisherLocations.Select(d => new { d.Id, d.Image, d.Location.Name, LocationAddress = d.Location.DisplayAddress, d.Location.Lat, d.Location.Long }),

            }).ToListAsync();
        }

        public async Task<object> SaveLocation(PublisherLocationViewModel publisher, string token)
        {
            var publisherForUser = await GetPublisherForUser(token);
            if (publisherForUser != null)
            {
                var publisherLocations = new Models.PublisherLocations()
                {
                    Active = true,
                    CreatedBy = token,
                    CreatedOn = DateTime.Now,
                    LastEditedBy = token,
                    LastEditedOn = DateTime.Now,
                    Id = Guid.NewGuid().ToString(),
                    Publisher = publisherForUser,
                    Location = new Models.Locations()
                    {
                        Active = true,
                        Lat = publisher.Lat,
                        Long = publisher.Long,
                        Name = publisher.Name,
                        DisplayAddress = publisher.DisplayAddress,
                        CreatedBy = token,
                        CreatedOn = DateTime.Now,
                        LastEditedBy = token,
                        LastEditedOn = DateTime.Now,
                    }
                };

                context.Entry(publisherLocations.Publisher).State = EntityState.Unchanged;
                await context.PublisherLocations.AddAsync(publisherLocations);
                await context.SaveChangesAsync();
                return new { publisher = publisherForUser.Id, location = publisherLocations.Id };
            }

            throw new PublisherNotFoundException();
        }

        internal async Task<object> Save(PublisherViewModel publisher, string token)
        {
            var pub = await context.Publishers.FirstOrDefaultAsync(c => c.Id == publisher.Id && c.CreatedBy == token);
            if (pub != null)
            {
                pub.Image = publisher.Image;
                pub.Name = publisher.Name;
                pub.Description = publisher.Description;
                await context.SaveChangesAsync();
                return new { pub.Id, pub.Name };
            }

            throw new PublisherNotFoundException();
        }

        public async Task<object> GetOnePublisher(string id)
        {
            return await context.Publishers.Include(c => c.PublisherLocations).ThenInclude(d => d.Location).Where(c => c.Id == id).Select(c => new
            {
                c.Id,
                c.Name,
                c.Image,
                c.Description,
                Locations = c.PublisherLocations.Select(d => new { d.Id, d.Image, d.Location.Name, LocationAddress = d.Location.DisplayAddress, d.Location.Lat, d.Location.Long }),

            }).FirstOrDefaultAsync();
        }

        public async Task<object> GetAllLocationsForPublisher(int publisherId)
        {
            return await context.PublisherLocations.Where(c => c.PublisherId == publisherId).Select(c => new
            {
                c.Id,
                c.Location.Name,
                c.Location.DisplayAddress,
                c.Location.Lat,
                c.Location.Long
            }).ToListAsync();
        }

        public async Task<PublisherLocations> GetOneLocation(string id)
        {
            return await context.PublisherLocations.Include(c => c.Location).Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserPublisher(string username)
        {
            return await context.Publishers.AnyAsync(c => c.CreatedBy == username);
        }
    }
}
