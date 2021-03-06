using loffers.api.Models.Generator;
using loffers.api.ViewModels;
using Loffers.Server.Data;
using Loffers.Server.Errors;
using Loffers.Server.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            context.Entry(offer.Categories).State = EntityState.Unchanged;

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
            List<OfferLocations> locations = new List<OfferLocations>();
            if (string.IsNullOrEmpty(model.OfferLocation.Id))
            {
                var publisherLocation = await new PublisherService(context).SaveLocation<PublisherLocationViewModel>(new ViewModels.PublisherLocationViewModel()
                {
                    DisplayAddress = model.OfferLocation.DisplayAddress,
                    Lat = model.OfferLocation.Lat,
                    Long = model.OfferLocation.Long,
                    Name = model.OfferLocation.Name
                }, token);



                var offerLocations = new OfferLocations()
                {
                    Active = true,
                    CreatedBy = token,
                    CreatedOn = DateTime.UtcNow,
                    LastEditedOn = DateTime.UtcNow,
                    LastEditedBy = token,
                    PublisherLocations = await context.PublisherLocations.FirstOrDefaultAsync(c => c.Id == publisherLocation.Id)
                };

                context.Entry(offerLocations.PublisherLocations).State = EntityState.Unchanged;
                locations.Add(offerLocations);
            }

            var offer = new Offers()
            {
                Categories = await context.Categories.FirstOrDefaultAsync(c => c.Id == model.Category.Id),
                Active = true,
                CreatedBy = token,
                CreatedOn = DateTime.Now,
                LastEditedBy = token,
                LastEditedOn = DateTime.Now,
                OfferDescription = model.Detail,
                OfferHeadline = model.Heading,
                TermsAndConditions = model.Terms,
                ValidFrom = model.ValidFrom != null ? model.ValidFrom.Value : DateTime.UtcNow,
                Image = model.Image,
                OriginalImage = model.OriginalImage,
                Publishers = publisherForUser,
                Id = Guid.NewGuid().ToString(),
                OfferCategories = model.Categories.Select(c => new OfferCategories() { Id = c.Id }).ToList(),
                OfferLocations = locations,
            };

            return await Create(offer);
        }

        public async Task Activate(OfferModel model, string userId)
        {
            var offer = await context.Offers.FirstOrDefaultAsync(c => c.Id == model.Id && c.CreatedBy == userId);
            if (offer != null)
            {
                offer.Active = !offer.Active;
                await context.SaveChangesAsync();
                return;
            }

            throw new Exception("This ad doesn't belong to you.");
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
                                                offer.OriginalImage,
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
                                                offer.Categories,
                                                offer.OfferID,
                                                offer.Active,
                                                Starred = string.IsNullOrEmpty(token) ? false : context.UserStarredOffers.Any(c => c.OfferId == offer.OfferID && c.OfferLocationID == location.OfferLocationID && c.UserId == token && c.Active)
                                            })
                                        .Select(d => new
                                        {
                                            Name = d.OfferHeadline,
                                            Detail = d.OfferDescription,
                                            Terms = d.TermsAndConditions,
                                            d.Image,
                                            d.OriginalImage,
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
                                            Categories = d.OfferCategories.Where(c => c.Active).Select(c => new { c.Categories.Name, c.Categories.Id, c.Categories.Image }),
                                            Category = new { d.Categories.Id, d.Categories.Image, d.Categories.Name },
                                            Coordinates = new { d.Lat, d.Long },
                                            Distance = new CoordinatesDistance()
                                            {
                                                DistanceIn = unit,
                                                Distance = (double)LoffersContext.DistanceCalculate((decimal)currentLat, d.Lat, (decimal)currentLong, d.Long),
                                            },
                                            d.Id,
                                            d.Starred,
                                            d.Active,
                                            Url = BaseUrlToSaveImages + "home/offerdetails?id=" + d.Id + "&unit=" + unit
                                        }).Where(c => c.Active);

            if (categories != null && categories.Any())
                resultSet = resultSet.Where(c => categories.Contains(c.Category.Id));

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

        public async Task<object> Star(string token, string user)
        {
            var offer = await context.Offers.Include(c => c.OfferLocations).FirstOrDefaultAsync(c => c.Id == token);
            var offerLocation = offer?.OfferLocations?.FirstOrDefault();
            if (offer != null && offerLocation != null)
            {
                var starred = await context.UserStarredOffers.FirstOrDefaultAsync(c => c.UserId == user && c.OfferId == offer.OfferID);
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
                                                d.Offers.OriginalImage,
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
                                                Category = new { d.Offers.Categories.Id, d.Offers.Categories.Image, d.Offers.Categories.Name },
                                                Categories = d.Offers.OfferCategories.Where(c => c.Active).Select(c => new { c.Categories.Name, c.Categories.Id, c.Categories.Image }),
                                                Coordinates = new { d.OfferLocations.PublisherLocations.Locations.Lat, d.OfferLocations.PublisherLocations.Locations.Long },
                                                Distance = new CoordinatesDistance() { DistanceIn = unit },
                                                d.Offers.Id,
                                                Url = BaseUrlToSaveImages + "home/offerdetails?id=" + d.Offers.Id + "&unit=" + unit
                                            }).ToListAsync();


            resultSet.ForEach(c => c.Distance.Distance = CalculateCoordicates(c.Coordinates.Lat, c.Coordinates.Long, currentLat, currentLong, showDistanceIn));
            return resultSet;
        }

        private double CalculateCoordicates(decimal lat, decimal @long, double currentLat, double currentLong, int showDistanceIn)
        {
            return new Coordinates(currentLat, currentLong).DistanceTo(new Coordinates((double)lat, (double)@long), showDistanceIn);
        }

        public async Task<OfferDetailsViewModel> Details(string id, double currentLat, double currentLong, string unit)
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
                                        .Include(c => c.Categories)
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
                                                offer.OriginalImage,
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
                                                offer.OfferCategories,
                                                Category = offer.Categories
                                            })
                                        .Select(d => new OfferDetailsViewModel()
                                        {
                                            Id = d.Id,
                                            Name = d.OfferHeadline,
                                            Detail = d.OfferDescription,
                                            Terms = d.TermsAndConditions,
                                            Image = d.Image,
                                            OriginalImage = d.OriginalImage,
                                            OfferToken = d.Id,
                                            ValidFrom = d.ValidFrom,
                                            Category = new CategoryModel() { Id = d.Category.Id, Name = d.Category.Name, Image = d.Category.Image },
                                            Categories = d.OfferCategories.Select(c => new CategoryModel() { Id = c.Categories.Id, Name = c.Categories.Name, Image = c.Categories.Image }).ToList(),
                                            Coordinates = new CoordinatesShort() { Lat = d.Lat, Long = d.Long },
                                            Distance = new CoordinatesDistance() { DistanceIn = unit },
                                            Url = BaseUrlToSaveImages + "home/offerdetails?id=" + d.Id + "&unit=" + unit
                                        })
                                        .FirstOrDefaultAsync(c => c.Id == id);

            resultSet.Distance.Distance = CalculateCoordicates(resultSet.Coordinates.Lat, resultSet.Coordinates.Long, currentLat, currentLong, showDistanceIn);
            return resultSet;
        }

        public async Task<object> Update(OfferModel offer, string token)
        {
            var offerresult = await context.Offers.Include(c => c.OfferCategories).Include(c => c.OfferLocations).FirstOrDefaultAsync(c => c.Id == offer.Id && c.CreatedBy == token);
            offerresult.Active = true;
            offerresult.LastEditedBy = token;
            offerresult.LastEditedOn = DateTime.Now;
            offerresult.OfferDescription = offer.Detail;
            offerresult.OfferHeadline = offer.Heading;
            offerresult.TermsAndConditions = offer.Terms;
            offerresult.ValidFrom = offer.ValidFrom != null ? offer.ValidFrom.Value : DateTime.UtcNow;
            offerresult.Image = offer.Image;
            offerresult.OriginalImage = offer.OriginalImage;

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

            //if (offer.OfferLocations != null && offer.OfferLocations.Any())
            //{
            //    foreach (var item in offer.OfferLocations)
            //    {
            //        var newlyAdded = new OfferLocations();
            //        newlyAdded.CreatedBy = token;
            //        newlyAdded.CreatedOn = DateTime.Now;
            //        newlyAdded.LastEditedBy = token;
            //        newlyAdded.LastEditedOn = DateTime.Now;
            //        newlyAdded.Offers = offerresult;
            //        newlyAdded.PublisherLocations = await context.PublisherLocations.Include(c => c.Locations).FirstOrDefaultAsync(c => c.Id == item.Id);
            //        context.Entry(newlyAdded.PublisherLocations).State = EntityState.Unchanged;
            //        context.Entry(newlyAdded.PublisherLocations.Locations).State = EntityState.Unchanged;
            //        newlyAdded.Active = true;
            //    }
            //}

            await context.SaveChangesAsync();
            return new { offer.Id, offer.Heading };
        }

        public async Task<object> GetOne(string id, string token, bool v)
        {
            try
            {
                if (context.Offers.Any(c => c.CreatedBy == token))
                {
                    var offer = await context.Offers
                        .Include(c => c.OfferLocations).Include("OfferLocations.PublisherLocations").Include("OfferLocations.PublisherLocations.Locations")
                        .Include(c => c.Categories).Include("OfferCategories").Include("OfferCategories.Categories").FirstOrDefaultAsync(c => c.Id == id);
                    var location = offer.OfferLocations.FirstOrDefault();

                    var offerObject = new
                    {
                        offer.OfferHeadline,
                        offer.OfferDescription,
                        offer.TermsAndConditions,
                        offer.Image,
                        offer.OriginalImage,
                        offer.CreatedBy,
                        offer.CreatedOn,
                        offer.ValidFrom,
                        offer.ValidTill,
                        offer.Active,
                        offer.Id,
                        Locations = new { location.PublisherLocations.Id, location.PublisherLocations.Locations.DisplayAddress, location.PublisherLocations.Locations.Name, location.PublisherLocations.Locations.Lat, location.PublisherLocations.Locations.Long },
                        Category = new { offer.Categories.Id, offer.Categories.Name, offer.Categories.Image },
                        Categories = offer.OfferCategories.Select(c => new OfferCategoryModel { Id = c.Categories.Id, Name = c.Categories.Name, Selected = false, Image = c.Categories.Image }).ToList()
                    };

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
            return await context.Offers.Include(c => c.Categories).Include(c => c.OfferCategories).Include("OfferCategories.Categories")
                .Include(c => c.OfferLocations).Include("OfferLocations.PublisherLocations").Include("OfferLocations.PublisherLocations.Locations").Select(c => new
                {
                    c.OfferHeadline,
                    c.OfferDescription,
                    c.TermsAndConditions,
                    c.ValidFrom,
                    c.ValidTill,
                    c.Image,
                    c.OriginalImage,
                    c.CreatedBy,
                    c.CreatedOn,
                    c.Id,
                    c.Active,
                    Category = new { c.Categories.Id, c.Categories.Name, c.Categories.Image },
                    Categories = c.OfferCategories.Select(d => new { d.Categories.Id, d.Categories.Name, d.Categories.Image }),
                    Locations = c.OfferLocations.Select(d => new { d.PublisherLocations.Id, d.PublisherLocations.Locations.Name }).FirstOrDefault()
                }).Where(c => c.CreatedBy == token).ToListAsync();
        }
    }
}
