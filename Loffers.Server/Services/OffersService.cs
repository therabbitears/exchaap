using Loffers.Server.Data;
using Loffers.Server.Errors;
using Loffers.Server.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Loffers.Server.Services
{
    public class OffersService
    {
        private LoffersContext context;
        CategoriesService categoryService;
        PublisherService publisherService;

        public OffersService()
        {
            context = new LoffersContext();
            categoryService = new CategoriesService();
            publisherService = new PublisherService();
        }

        public async Task<object> Create(Offers offer)
        {
            var offerresult = await context.Offers.AddAsync(offer);
            context.Entry(offer.Publisher).State = EntityState.Unchanged;

            if (offer.OfferCategories != null)
            {
                foreach (var item in offer.OfferCategories)
                {
                    item.CreatedBy = offer.CreatedBy;
                    item.CreatedOn = offer.CreatedOn;
                    item.LastEditedBy = offer.LastEditedBy;
                    item.LastEditedOn = offer.LastEditedOn;

                    item.Offer = offer;
                    item.Category = await categoryService.GetOne(item.Id);
                    context.Entry(item.Category).State = EntityState.Unchanged;
                    item.Active = true;
                }
            }

            if (offer.OfferLocations != null)
            {
                foreach (var item in offer.OfferLocations)
                {
                    item.CreatedBy = offer.CreatedBy;
                    item.CreatedOn = offer.CreatedOn;
                    item.LastEditedBy = offer.LastEditedBy;
                    item.LastEditedOn = offer.LastEditedOn;
                    item.Offer = offer;
                    context.Entry(item.PublisherLocation).State = EntityState.Unchanged;
                    context.Entry(item.PublisherLocation.Location).State = EntityState.Unchanged;
                    item.Active = true;
                }
            }

            await context.SaveChangesAsync();
            return new { offer.Id };
        }


        public async Task<object> FindOffersAround(double currentLat, double currentLong, int maximumDistanceInMeters, string[] categories, string unit, string token)
        {
            int showDistanceIn = (int)ShowDistanceIn.Kilometers;
            if (unit != null)
            {
                if (unit.ToLower() == "mi")
                    showDistanceIn = (int)ShowDistanceIn.Miles;

                if (unit.ToLower() == "m")
                    showDistanceIn = (int)ShowDistanceIn.Meters;
            }

            var resultSet = context.Offers
                                        .Include(c => c.OfferLocations).ThenInclude(e => e.PublisherLocation).ThenInclude(f => f.Location)
                                        .Include(c => c.Publisher)
                                        .Include(c => c.OfferCategories).ThenInclude(g => g.Category)
                                        .Join(context.OfferLocations,
                                            offer => offer.OfferId,
                                            location => location.OfferId,
                                            (offer, location) => new
                                            {
                                                location.OfferLocationId,
                                                offer.OfferHeadline,
                                                offer.OfferDescription,
                                                offer.TermsAndConditions,
                                                offer.Image,
                                                offer.Id,
                                                location.PublisherLocation.Location.Lat,
                                                location.PublisherLocation.Location.Long,
                                                PublisherName = location.PublisherLocation.Publisher.Name,
                                                PublisherLogo = location.PublisherLocation.Publisher.Image,
                                                PublisherId = location.PublisherLocation.Publisher.Id,
                                                LocationId = location.PublisherLocation.Id,
                                                LocationName = location.PublisherLocation.Location.Name,
                                                location.PublisherLocation.Location.DisplayAddress,
                                                SubPublisherLogo = location.PublisherLocation.Image,
                                                offer.ValidFrom,
                                                offer.ValidTill,
                                                offer.OfferCategories,
                                                offer.OfferId,
                                                Starred = string.IsNullOrEmpty(token) ? false : context.UserStarredOffers.Any(c => c.OfferId == offer.OfferId && c.OfferLocationId == location.OfferLocationId && c.UserId == token),
                                                Distance = new Coordinates(currentLat, currentLong).DistanceTo(new Coordinates((double)location.PublisherLocation.Location.Lat, (double)location.PublisherLocation.Location.Long), showDistanceIn)
                                            })
                                        .Select(d => new
                                        {
                                            Name = d.OfferHeadline,
                                            Detail = d.OfferDescription,
                                            Terms = d.TermsAndConditions,
                                            d.Image,
                                            OfferToken = d.Id,
                                            d.PublisherName,
                                            d.PublisherLogo,
                                            PublisherToken = d.PublisherId,
                                            LocationToken = d.LocationId,
                                            d.LocationName,
                                            LocationAddress = d.DisplayAddress,
                                            d.SubPublisherLogo,
                                            d.ValidFrom,
                                            d.ValidTill,
                                            Categories = d.OfferCategories.Select(c => new { c.Category.Name, c.Category.Id }),
                                            Coordinates = new { d.Lat, d.Long },
                                            d.Distance,
                                            d.Id,
                                            d.Starred
                                        })
                                        .AsEnumerable()
                                        .Where(c => ((maximumDistanceInMeters == 0) || c.Distance <= maximumDistanceInMeters) && (((categories == null || categories.Length == 0) || (categories.Length > 0 && c.Categories.Any(d => categories.Contains(d.Id))))))
                                        //.Where(c => c.OfferHeadline == "Republic day offer")
                                        .ToList();



            return resultSet;
        }

        internal async Task<object> Report(OfferReportModel model, string userName)
        {
            if (model != null && !string.IsNullOrEmpty(model.Id))
            {
                var offer = await context.Offers.FirstOrDefaultAsync(c => c.Id == model.Id);
                if (offer != null)
                {
                    var complaint = new OfferComplaints()
                    {
                        CreatedOn = DateTime.Now,
                        CreatedBy = userName,
                        Offer = offer,
                        ReportContent = JsonConvert.SerializeObject(model)
                    };

                    context.OfferComplaints.Add(complaint);
                    await context.SaveChangesAsync();
                    return new { offer.Id, offer.OfferHeadline };
                }
            }

            throw new OfferNotFoundException();
        }

        public async Task<object> Star(string token, string locationToken, string user)
        {
            var offer = await context.Offers.FirstOrDefaultAsync(c => c.Id == token);
            var publisherLocation = await context.PublisherLocations.FirstOrDefaultAsync(c => c.Id == locationToken);
            if (offer != null && publisherLocation != null)
            {
                var offerLocation = await context.OfferLocations.FirstOrDefaultAsync(c => c.OfferId == offer.OfferId && c.PublisherLocationId == publisherLocation.PublisherLocationId && c.Active);
                var starred = await context.UserStarredOffers.FirstOrDefaultAsync(c => c.UserId == user && c.OfferId == offer.OfferId && c.OfferLocationId == offerLocation.OfferLocationId);
                if (starred != null)
                {
                    starred.LastEditedOn = DateTime.Now;
                    starred.Active = !starred.Active;
                }
                else
                {
                    starred = new UserStarredOffers()
                    {
                        Active = true,
                        CreatedOn = DateTime.Now,
                        LastEditedOn = DateTime.Now,
                        Offer = offer,
                        OfferLocation = offerLocation,
                        UserId = user
                    };

                    context.UserStarredOffers.Add(starred);
                }

                await context.SaveChangesAsync();
                return new { offer.Id, offer.OfferHeadline, Starred = true };
            }

            throw new OfferNotFoundException();
        }

        public async Task<object> Starred(double currentLat, double currentLong, string token, string unit)
        {
            int showDistanceIn = (int)ShowDistanceIn.Kilometers;
            if (unit != null)
            {
                if (unit.ToLower() == "mi")
                    showDistanceIn = (int)ShowDistanceIn.Miles;

                if (unit.ToLower() == "m")
                    showDistanceIn = (int)ShowDistanceIn.Meters;
            }

            var resultSet = await context.UserStarredOffers
                                            .Include(c => c.OfferLocation).ThenInclude(e => e.PublisherLocation).ThenInclude(f => f.Location)
                                            .Include(c => c.Offer).ThenInclude(c => c.OfferCategories).ThenInclude(c => c.Category)
                                            .Where(c => c.UserId == token).Select(d => new
                                            {
                                                Name = d.Offer.OfferHeadline,
                                                Detail = d.Offer.OfferDescription,
                                                Terms = d.Offer.TermsAndConditions,
                                                d.Offer.Image,
                                                OfferToken = d.Offer.Id,
                                                PublisherName = d.Offer.Publisher.Name,
                                                PublisherLogo = d.Offer.Publisher.Image,
                                                PublisherToken = d.Offer.Publisher.PublisherId,
                                                LocationName = d.OfferLocation.PublisherLocation.Location.Name,
                                                LocationAddress = d.OfferLocation.PublisherLocation.Location.DisplayAddress,
                                                SubPublisherLogo = d.OfferLocation.PublisherLocation.Image,
                                                d.Offer.ValidFrom,
                                                d.Offer.ValidTill,
                                                Categories = d.Offer.OfferCategories.Select(c => new { c.Category.Name, c.Category.Id }),
                                                Coordinates = new { d.OfferLocation.PublisherLocation.Location.Lat, d.OfferLocation.PublisherLocation.Location.Long },
                                                Distance = new Coordinates(currentLat, currentLong).DistanceTo(new Coordinates((double)d.OfferLocation.PublisherLocation.Location.Lat, (double)d.OfferLocation.PublisherLocation.Location.Long), showDistanceIn),
                                                d.Offer.Id
                                            }).ToListAsync();

            return resultSet;
        }

        public async Task<object> Details(string id, double currentLat, double currentLong, string unit)
        {
            int showDistanceIn = (int)ShowDistanceIn.Kilometers;
            if (unit != null)
            {
                if (unit.ToLower() == "mi")
                    showDistanceIn = (int)ShowDistanceIn.Miles;

                if (unit.ToLower() == "m")
                    showDistanceIn = (int)ShowDistanceIn.Meters;
            }

            var resultSet = await context.Offers
                                        .Include(c => c.OfferLocations).ThenInclude(e => e.PublisherLocation).ThenInclude(f => f.Location)
                                        .Include(c => c.Publisher)
                                        .Include(c => c.OfferCategories).ThenInclude(g => g.Category)
                                        .Join(context.OfferLocations,
                                            offer => offer.OfferId,
                                            location => location.OfferId,
                                            (offer, location) => new
                                            {
                                                location.OfferLocationId,
                                                offer.OfferHeadline,
                                                offer.OfferDescription,
                                                offer.TermsAndConditions,
                                                offer.Image,
                                                offer.Id,
                                                location.PublisherLocation.Location.Lat,
                                                location.PublisherLocation.Location.Long,
                                                PublisherName = location.PublisherLocation.Publisher.Name,
                                                PublisherLogo = location.PublisherLocation.Publisher.Image,
                                                PublisherId = location.PublisherLocation.Publisher.Id,
                                                LocationName = location.PublisherLocation.Location.Name,
                                                location.PublisherLocation.Location.DisplayAddress,
                                                SubPublisherLogo = location.PublisherLocation.Image,
                                                offer.ValidFrom,
                                                offer.ValidTill,
                                                offer.OfferCategories,
                                                Distance = new Coordinates(currentLat, currentLong).DistanceTo(new Coordinates((double)location.PublisherLocation.Location.Lat, (double)location.PublisherLocation.Location.Long), showDistanceIn)
                                            })
                                        .Select(d => new
                                        {
                                            d.Id,
                                            Name = d.OfferHeadline,
                                            Detail = d.OfferDescription,
                                            Terms = d.TermsAndConditions,
                                            d.Image,
                                            OfferToken = d.Id,
                                            d.PublisherName,
                                            d.PublisherLogo,
                                            PublisherToken = d.PublisherId,
                                            d.LocationName,
                                            LocationAddress = d.DisplayAddress,
                                            d.SubPublisherLogo,
                                            d.ValidFrom,
                                            d.ValidTill,
                                            Categories = d.OfferCategories.Select(c => new { c.Category.Name, c.Category.Id }),
                                            Coordinates = new { d.Lat, d.Long },
                                            d.Distance
                                        })
                                        .FirstOrDefaultAsync(c => c.Id == id);

            return resultSet;
        }

        private bool isLocationClosedBy(OfferLocations location, double currentLat, double currentLong, int maximumDistanceInMeters)
        {
            return new Coordinates(currentLat, currentLong).DistanceTo(new Coordinates((double)location.PublisherLocation.Location.Lat, (double)location.PublisherLocation.Location.Long)) < maximumDistanceInMeters;
        }

        public async Task<object> Update(OfferModel offer, string token)
        {
            var offerresult = await context.Offers.Include(c => c.OfferCategories).Include(c => c.OfferLocations).FirstOrDefaultAsync(c => c.Id == offer.Id);
            offerresult.Active = true;
            offerresult.LastEditedBy = token;
            offerresult.LastEditedOn = DateTime.Now;
            offerresult.OfferDescription = offer.Detail;
            offerresult.OfferHeadline = offer.Heading;
            offerresult.TermsAndConditions = offer.Terms;
            offerresult.ValidFrom = offer.ValidFrom.Value;
            offerresult.ValidTill = offer.ValidTo.Value;
            offerresult.Image = offer.Image;

            if (offerresult.OfferCategories.Any())
            {
                foreach (var item in offerresult.OfferCategories)
                {
                    item.Active = false;
                }
            }

            if (offer.Categories != null && offer.Categories.Any())
            {
                foreach (var item in offer.Categories)
                {
                    if (offerresult.OfferCategories.Any(c => c.Id == item.Id))
                    {
                        offerresult.OfferCategories.FirstOrDefault(c => c.Id == item.Id).Active = true;
                    }
                    else
                    {
                        var newlyAdded = new Models.OfferCategories() { Id = item.Id };
                        newlyAdded.CreatedBy = token;
                        newlyAdded.CreatedOn = DateTime.Now;
                        newlyAdded.LastEditedBy = token;
                        newlyAdded.LastEditedOn = DateTime.Now;
                        newlyAdded.Offer = offerresult;
                        newlyAdded.Category = await categoryService.GetOne(item.Id);
                    }
                }
            }

            if (offerresult.OfferLocations.Any())
            {
                foreach (var item in offerresult.OfferLocations)
                {
                    item.Active = false;
                }
            }

            if (offer.OfferLocations != null && offer.OfferLocations.Any())
            {
                foreach (var item in offer.OfferLocations)
                {
                    var newlyAdded = new Models.OfferLocations();
                    newlyAdded.CreatedBy = token;
                    newlyAdded.CreatedOn = DateTime.Now;
                    newlyAdded.LastEditedBy = token;
                    newlyAdded.LastEditedOn = DateTime.Now;
                    newlyAdded.Offer = offerresult;
                    newlyAdded.PublisherLocation = await context.PublisherLocations.Include(c => c.Location).FirstOrDefaultAsync(c => c.Id == item.Id);
                    context.Entry(newlyAdded.PublisherLocation).State = EntityState.Unchanged;
                    context.Entry(newlyAdded.PublisherLocation.Location).State = EntityState.Unchanged;
                    newlyAdded.Active = true;
                }
            }

            await context.SaveChangesAsync();
            return new { offer.Id, offer.Heading };
        }

        public async Task<object> GetOne(string id, string token, bool v)
        {
            try
            {
                if (context.Offers.Any(c => c.CreatedBy == token))
                {
                    var offer = await context.Offers.FirstOrDefaultAsync(c => c.Id == id);
                    var offerObject = new
                    {
                        offer.OfferHeadline,
                        offer.OfferDescription,
                        offer.TermsAndConditions,
                        offer.Image,
                        offer.CreatedBy,
                        offer.CreatedOn,
                        offer.ValidFrom,
                        offer.ValidTill,
                        offer.Active,
                        offer.Id,
                        Locations = await context.PublisherLocations.Include(c => c.Location).Where(c => c.PublisherId == offer.PublisherId).Select(c => new OfferLocationModel { Id = c.Id, Name = c.Location.Name, Selected = false }).ToListAsync(),
                        Categories = await context.Categories.Select(c => new OfferCategoryModel { Id = c.Id, Name = c.Name, Selected = false }).ToListAsync()
                    };

                    var offerCategories = await context.OfferCategories.Include(c => c.Category).Where(c => c.OfferId == offer.OfferId).ToListAsync();
                    var offerLocations = await context.OfferLocations.Include(c => c.PublisherLocation).Include(c => c.PublisherLocation.Location).Where(c => c.OfferId == offer.OfferId).Select(c => new { c.PublisherLocation.Id, c.PublisherLocation.Location.Name, Selected = true }).ToListAsync();

                    foreach (var item in offerCategories)
                    {
                        var categoryExist = offerObject.Categories.FirstOrDefault(c => c.Id == item.Category.Id);
                        if (categoryExist != null)
                        {
                            categoryExist.Selected = true;
                        }
                    }

                    foreach (var item in offerLocations)
                    {
                        var categoryExist = offerObject.Locations.FirstOrDefault(c => c.Id == item.Id);
                        if (categoryExist != null)
                        {
                            categoryExist.Selected = true;
                        }
                    }

                    return offerObject;
                }
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public async Task<object> ForUser(string token)
        {
            return await context.Offers.Select(c => new
            {
                c.OfferHeadline,
                c.OfferDescription,
                c.TermsAndConditions,
                c.Image,
                c.CreatedBy,
                c.CreatedOn,
                c.Id
            }).Where(c => c.CreatedBy == token).ToListAsync();
        }
    }
}
