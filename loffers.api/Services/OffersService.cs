using loffers.api.Models.Generator;
using Loffers.Server.Data;
using Loffers.Server.Errors;
using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Loffers.Server.Services
{
    public class OffersService : BaseService
    {
        CategoriesService categoryService;
        PublisherService publisherService;

        public OffersService(LoffersContext context = null) : base(context)
        {
            categoryService = new CategoriesService(this.context);
            publisherService = new PublisherService(this.context);
        }

        public async Task<object> Create(Offers offer)
        {
            var offerresult = context.Offers.Add(offer);
            context.Entry(offer.Publishers).State = EntityState.Unchanged;

            if (offer.OfferCategories != null)
            {
                foreach (var item in offer.OfferCategories)
                {
                    item.CreatedBy = offer.CreatedBy;
                    item.CreatedOn = offer.CreatedOn;
                    item.LastEditedBy = offer.LastEditedBy;
                    item.LastEditedOn = offer.LastEditedOn;

                    item.Offers = offer;
                    item.Categories = await categoryService.GetOne(item.Id);
                    context.Entry(item.Categories).State = EntityState.Unchanged;
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
                    item.Offers = offer;
                    context.Entry(item.PublisherLocations).State = EntityState.Unchanged;
                    context.Entry(item.PublisherLocations.Locations).State = EntityState.Unchanged;
                    item.Active = true;
                }
            }

            await context.SaveChangesAsync();
            return new { offer.Id };
        }

        public async Task<object> Create(OfferModel model, string token)
        {
            var publisherForUser = await publisherService.GetPublisherForUser(token);
            var locations = model.OfferLocations.Select(c => new OfferLocations() { }).ToList();
            int index = 0;
            foreach (var location in model.OfferLocations)
            {
                locations[index].PublisherLocations = await publisherService.GetOneLocation(location.Id);
                index++;
            }

            var offer = new Offers()
            {
                Active = true,
                CreatedBy = token,
                CreatedOn = DateTime.Now,
                LastEditedBy = token,
                LastEditedOn = DateTime.Now,
                OfferDescription = model.Detail,
                OfferHeadline = model.Heading,
                TermsAndConditions = model.Terms,
                ValidFrom = model.ValidFrom.Value,
                ValidTill = model.ValidTo.Value,
                Image = model.Image,
                Publishers = publisherForUser,
                Id = Guid.NewGuid().ToString(),
                OfferCategories = model.Categories.Select(c => new OfferCategories() { Id = c.Id }).ToList(),
                OfferLocations = locations,
            };

            return await Create(offer);
        }

        public async Task<object> FindOffersAround(double currentLat, double currentLong, int maximumDistanceInMeters, string[] categories, string unit, string token, int pageNumber)
        {
            var take = 10;
            var skip = pageNumber * 10;
            var resultSet = context.Offers
                                        .Include(c => c.OfferLocations)
                                        .Include("OfferLocations.PublisherLocation")
                                        .Include("OfferLocations.PublisherLocations.Locations")
                                        //ThenInclude(e => e.OfferLocations.PublisherLocation).ThenInclude(f => f.Location)
                                        .Include(c => c.Publishers)
                                        .Include(c => c.OfferCategories).Include("OfferCategories.Categories")
                                        .Join(context.OfferLocations,
                                            offer => offer.OfferID,
                                            location => location.OfferID,
                                            (offer, location) => new
                                            {
                                                location.OfferLocationID,
                                                offer.OfferHeadline,
                                                offer.OfferDescription,
                                                offer.TermsAndConditions,
                                                offer.Image,
                                                offer.Id,
                                                location.PublisherLocations.Locations.Lat,
                                                location.PublisherLocations.Locations.Long,
                                                PublisherName = location.PublisherLocations.Publishers.Name,
                                                PublisherLogo = location.PublisherLocations.Publishers.Image,
                                                PublisherId = location.PublisherLocations.Publishers.Id,
                                                LocationId = location.PublisherLocations.Id,
                                                LocationName = location.PublisherLocations.Locations.Name,
                                                location.PublisherLocations.Locations.DisplayAddress,
                                                SubPublisherLogo = location.PublisherLocations.Image,
                                                offer.ValidFrom,
                                                offer.ValidTill,
                                                offer.OfferCategories,
                                                offer.OfferID,
                                                Starred = string.IsNullOrEmpty(token) ? false : context.UserStarredOffers.Any(c => c.OfferId == offer.OfferID && c.OfferLocationID == location.OfferLocationID && c.UserId == token && c.Active)
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
                                            Categories = d.OfferCategories.Select(c => new { c.Categories.Name, c.Categories.Id }),
                                            Coordinates = new { d.Lat, d.Long },
                                            Distance = new CoordinatesDistance()
                                            {
                                                DistanceIn = unit,
                                                Distance = (double)LoffersContext.DistanceCalculate((decimal)currentLat, d.Lat, (decimal)currentLong, d.Long),
                                            },
                                            d.Id,
                                            d.Starred
                                        });

            if (categories != null && categories.Any())
                resultSet = resultSet.Where(c => c.Categories.Any(d => categories.Contains(d.Id)));

            if (maximumDistanceInMeters > 0)
                resultSet = resultSet.Where(c => c.Distance.Distance <= maximumDistanceInMeters);

            return await resultSet.OrderBy(c => c.Distance.Distance).Skip(skip).Take(take).ToListAsync();
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
                        Offers = offer,
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
                var offerLocation = await context.OfferLocations.FirstOrDefaultAsync(c => c.OfferID == offer.OfferID && c.PublisherLocationID == publisherLocation.PublisherLocationID && c.Active);
                var starred = await context.UserStarredOffers.FirstOrDefaultAsync(c => c.UserId == user && c.OfferId == offer.OfferID && c.OfferLocationID == offerLocation.OfferLocationID);
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
                        Offers = offer,
                        OfferLocations = offerLocation,
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
                                            .Include(c => c.OfferLocations).Include("OfferLocations.PublisherLocations").Include("OfferLocations.PublisherLocations.Locations")
                                            .Include(c => c.Offers).Include("Offers.OfferCategories").Include("Offers.OfferCategories.Categories")
                                            .Where(c => c.UserId == token && c.Active).Select(d => new
                                            {
                                                Name = d.Offers.OfferHeadline,
                                                Detail = d.Offers.OfferDescription,
                                                Terms = d.Offers.TermsAndConditions,
                                                d.Offers.Image,
                                                OfferToken = d.Offers.Id,
                                                PublisherName = d.Offers.Publishers.Name,
                                                PublisherLogo = d.Offers.Publishers.Image,
                                                PublisherToken = d.Offers.Publishers.PublisherID,
                                                LocationName = d.OfferLocations.PublisherLocations.Locations.Name,
                                                LocationAddress = d.OfferLocations.PublisherLocations.Locations.DisplayAddress,
                                                SubPublisherLogo = d.OfferLocations.PublisherLocations.Image,
                                                d.Offers.ValidFrom,
                                                Starred = true,
                                                LocationToken = d.OfferLocations.PublisherLocations.Id,
                                                d.Offers.ValidTill,
                                                Categories = d.Offers.OfferCategories.Select(c => new { c.Categories.Name, c.Categories.Id }),
                                                Coordinates = new { d.OfferLocations.PublisherLocations.Locations.Lat, d.OfferLocations.PublisherLocations.Locations.Long },
                                                Distance = new CoordinatesDistance() { DistanceIn = unit },
                                                d.Offers.Id
                                            }).ToListAsync();


            resultSet.ForEach(c => c.Distance.Distance = CalculateCoordicates(c.Coordinates.Lat, c.Coordinates.Long, currentLat, currentLong, showDistanceIn));
            return resultSet;
        }

        private double CalculateCoordicates(decimal lat, decimal @long, double currentLat, double currentLong, int showDistanceIn)
        {
            return new Coordinates(currentLat, currentLong).DistanceTo(new Coordinates((double)lat, (double)@long), showDistanceIn);
        }

        public async Task<object> Details(string id, string locationId, double currentLat, double currentLong, string unit)
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
                                        .Include(c => c.OfferLocations).Include("OfferLocations.PublisherLocations").Include("OfferLocations.PublisherLocations.Locations")
                                        .Include(c => c.Publishers)
                                        .Include(c => c.OfferCategories).Include("OfferCategories.Categories")
                                        .Join(context.OfferLocations,
                                            offer => offer.OfferID,
                                            location => location.OfferID,
                                            (offer, location) => new
                                            {
                                                location.OfferLocationID,
                                                offer.OfferHeadline,
                                                offer.OfferDescription,
                                                offer.TermsAndConditions,
                                                offer.Image,
                                                offer.Id,
                                                LocationId = location.PublisherLocations.Id,
                                                location.PublisherLocations.Locations.Lat,
                                                location.PublisherLocations.Locations.Long,
                                                PublisherName = location.PublisherLocations.Publishers.Name,
                                                PublisherLogo = location.PublisherLocations.Publishers.Image,
                                                PublisherId = location.PublisherLocations.Publishers.Id,
                                                LocationName = location.PublisherLocations.Locations.Name,
                                                location.PublisherLocations.Locations.DisplayAddress,
                                                SubPublisherLogo = location.PublisherLocations.Image,
                                                offer.ValidFrom,
                                                offer.ValidTill,
                                                offer.OfferCategories
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
                                            LocationToken = d.LocationId,
                                            Categories = d.OfferCategories.Select(c => new { c.Categories.Name, c.Categories.Id }),
                                            Coordinates = new { d.Lat, d.Long },
                                            Distance = new CoordinatesDistance() { DistanceIn = unit },
                                        })
                                        .FirstOrDefaultAsync(c => c.Id == id && c.LocationToken == locationId);

            resultSet.Distance.Distance = CalculateCoordicates(resultSet.Coordinates.Lat, resultSet.Coordinates.Long, currentLat, currentLong, showDistanceIn);
            return resultSet;
        }

        private bool isLocationClosedBy(OfferLocations location, double currentLat, double currentLong, int maximumDistanceInMeters)
        {
            return new Coordinates(currentLat, currentLong).DistanceTo(new Coordinates((double)location.PublisherLocations.Locations.Lat, (double)location.PublisherLocations.Locations.Long)) < maximumDistanceInMeters;
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
                        var newlyAdded = new OfferCategories() { Id = item.Id };
                        newlyAdded.CreatedBy = token;
                        newlyAdded.CreatedOn = DateTime.Now;
                        newlyAdded.LastEditedBy = token;
                        newlyAdded.LastEditedOn = DateTime.Now;
                        newlyAdded.Offers = offerresult;
                        newlyAdded.Categories = await categoryService.GetOne(item.Id);
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
                    var newlyAdded = new OfferLocations();
                    newlyAdded.CreatedBy = token;
                    newlyAdded.CreatedOn = DateTime.Now;
                    newlyAdded.LastEditedBy = token;
                    newlyAdded.LastEditedOn = DateTime.Now;
                    newlyAdded.Offers = offerresult;
                    newlyAdded.PublisherLocations = await context.PublisherLocations.Include(c => c.Locations).FirstOrDefaultAsync(c => c.Id == item.Id);
                    context.Entry(newlyAdded.PublisherLocations).State = EntityState.Unchanged;
                    context.Entry(newlyAdded.PublisherLocations.Locations).State = EntityState.Unchanged;
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
                        Locations = await context.PublisherLocations.Include(c => c.Locations).Where(c => c.PublisherID == offer.PublisherID).Select(c => new OfferLocationModel { Id = c.Id, Name = c.Locations.Name, Selected = false }).ToListAsync(),
                        Categories = await context.Categories.Select(c => new OfferCategoryModel { Id = c.Id, Name = c.Name, Selected = false }).ToListAsync()
                    };

                    var offerCategories = await context.OfferCategories.Include(c => c.Categories).Where(c => c.OfferID == offer.OfferID).ToListAsync();
                    var offerLocations = await context.OfferLocations.Include(c => c.PublisherLocations).Include(c => c.PublisherLocations.Locations).Where(c => c.OfferID == offer.OfferID).Select(c => new { c.PublisherLocations.Id, c.PublisherLocations.Locations.Name, Selected = true }).ToListAsync();

                    foreach (var item in offerCategories)
                    {
                        var categoryExist = offerObject.Categories.FirstOrDefault(c => c.Id == item.Categories.Id);
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
            return await context.Offers.Include(c => c.OfferCategories).Include("OfferCategories.Categories")
                .Include(c => c.OfferLocations).Include("OfferLocations.PublisherLocations").Include("OfferLocations.PublisherLocations.Locations").Select(c => new
                {
                    c.OfferHeadline,
                    c.OfferDescription,
                    c.TermsAndConditions,
                    c.ValidFrom,
                    c.ValidTill,
                    c.Image,
                    c.CreatedBy,
                    c.CreatedOn,
                    c.Id,
                    Categories = c.OfferCategories.Select(d => new { d.Categories.Id, d.Categories.Name }),
                    Locations = c.OfferLocations.Select(d => new { d.PublisherLocations.Id, d.PublisherLocations.Locations.Name })
                }).Where(c => c.CreatedBy == token).ToListAsync();
        }
    }
}
