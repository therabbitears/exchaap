using loffers.api.Models.Generator;
using Loffers.Server.Errors;
using Loffers.Server.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Loffers.Server.Services
{
    /// <summary>
    /// PublisherService
    /// </summary>
    public class PublisherService : BaseService
    {

        public PublisherService(LoffersContext context = null) : base(context)
        {
        }

        public async Task<object> Save(Publishers publishe)
        {
            context.Publishers.Add(publishe);
            await context.SaveChangesAsync();
            return new { publishe.Id, publishe.Name };
        }

        public async Task<Publishers> GetPublisherForUser(string token)
        {
            return await context.Publishers.FirstOrDefaultAsync(c => c.CreatedBy == token);
        }

        public async Task<object> GetPublishersForUser(string token)
        {
            return await context.Publishers.Include(c => c.PublisherLocations).Include("PublisherLocations.Locations").Where(c => c.CreatedBy == token).Select(c => new
            {
                c.Id,
                c.Name,
                c.Image,
                c.Description,
                Locations = c.PublisherLocations.Select(d => new { d.Id, d.Image, d.Locations.Name, LocationAddress = d.Locations.DisplayAddress, d.Locations.Lat, d.Locations.Long }),

            }).ToListAsync();
        }

        public async Task<PublisherLocationViewModel> SaveLocation<T>(PublisherLocationViewModel publisher, string token)
        {
            var publisherForUser = await GetPublisherForUser(token);
            if (publisherForUser != null)
            {
                var publisherLocations = new PublisherLocations()
                {
                    Active = true,
                    CreatedBy = token,
                    CreatedOn = DateTime.Now,
                    LastEditedBy = token,
                    LastEditedOn = DateTime.Now,
                    Id = Guid.NewGuid().ToString(),
                    Publishers = publisherForUser,
                    Locations = new Locations()
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

                context.Entry(publisherLocations.Publishers).State = EntityState.Unchanged;
                context.PublisherLocations.Add(publisherLocations);
                await context.SaveChangesAsync();
                publisher.Id = publisherLocations.Id;
                return publisher;
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
            return await context.Publishers.Include(c => c.PublisherLocations).Include("PublisherLocations.Locations").Where(c => c.Id == id).Select(c => new
            {
                c.Id,
                c.Name,
                c.Image,
                c.Description,
                Locations = c.PublisherLocations.Select(d => new { d.Id, d.Image, d.Locations.Name, LocationAddress = d.Locations.DisplayAddress, d.Locations.Lat, d.Locations.Long }),

            }).FirstOrDefaultAsync();
        }

        public async Task<object> GetSingleLocationForPublisher(int publisherID, string id)
        {
            return await context.PublisherLocations.Where(c => c.PublisherID == publisherID && c.Id == id).Select(c => new
            {
                c.Id,
                c.Locations.Name,
                c.Locations.DisplayAddress,
                c.Locations.Lat,
                c.Locations.Long
            }).SingleOrDefaultAsync();
        }

        public async Task<object> GetAllLocationsForPublisher(int publisherId)
        {
            return await context.PublisherLocations.Where(c => c.PublisherID == publisherId).Select(c => new
            {
                c.Id,
                c.Locations.Name,
                c.Locations.DisplayAddress,
                c.Locations.Lat,
                c.Locations.Long
            }).ToListAsync();
        }

        public async Task<PublisherLocations> GetOneLocation(string id)
        {
            return await context.PublisherLocations.Include(c => c.Locations).Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserPublisher(string username)
        {
            return await context.Publishers.AnyAsync(c => c.CreatedBy == username);
        }
    }
}
