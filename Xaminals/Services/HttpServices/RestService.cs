using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xaminals.Infra.Results;
using Xaminals.Models;

namespace Xaminals.Services.HttpServices
{
    public class RestService : Service, IRestService
    {
        public string Token { get; set; }

        HttpClient _client;

        public RestService()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            _client = new HttpClient();
        }

        public async Task<T> CreateUser<T>(object item) where T : IHttpResult
        {
            return await Create<T>(item, UrlConstants.CreateUrl, false);
        }

        public async Task<T> UpdateUser<T>(object item) where T : IHttpResult
        {
            return await Put<T>(item, UrlConstants.UpdateuserUrl);
        }

        public async Task<T> GeneratePasswordRequest<T>(object item) where T : IHttpResult
        {
            return await Create<T>(item, UrlConstants.ForgotPasswordUrl, false);
        }

        public async Task<T> ResetPasswordRequest<T>(object item) where T : IHttpResult
        {
            return await Put<T>(item, UrlConstants.ResetPasswordUrl);
        }

        public async Task<T> UpdateUserSnapshot<T>(object item) where T : IHttpResult
        {
            return await Put<T>(item, UrlConstants.UpdateuserSnapshotUrl);
        }

        public async Task<T> ValidateToken<T>() where T : IHttpResult
        {
            return await Get<T>(UrlConstants.VerifyTokenUrl);
        }

        public async Task<T> UpdatePassword<T>(object item) where T : IHttpResult
        {
            return await Put<T>(item, UrlConstants.UpdatePasswordUrl);
        }

        public async Task<T> CreateOffer<T>(object item) where T : IHttpResult
        {
            return await Create<T>(item, UrlConstants.CreateOfferUrl, true);
        }

        internal async Task<T> Publishers<T>() where T : IHttpResult
        {
            return await Get<T>(UrlConstants.MyPublishersUrl);
        }

        internal async Task<T> SearchLocations<T>(string keyword) where T : IHttpResult
        {
            var queryStrings = new List<QueryString>()
            {
                new QueryString{key= "keyword", value=keyword }
            };
            return await Get<T>(UrlConstants.SearchLocationsUrl, queryStrings);
        }

        public async Task<T> UpdateOffer<T>(object item) where T : IHttpResult
        {
            return await Put<T>(item, UrlConstants.UpdateOfferUrl);
        }

        public async Task<T> UpdateConfiguration<T>(object item) where T : IHttpResult
        {
            return await Put<T>(item, UrlConstants.UpdateConfigurationUrl);
        }

        public async Task<T> CreatePublisher<T>(object item) where T : IHttpResult
        {
            return await Create<T>(item, UrlConstants.CreatePublisherUrl, true);
        }

        public async Task<T> UpdatePublisher<T>(object item) where T : IHttpResult
        {
            return await Put<T>(item, UrlConstants.UpdatePublisherUrl);
        }

        public async Task<T> GetOfferToEdit<T>(string id) where T : IHttpResult
        {
            return await Get<T>(UrlConstants.MyEditOfferUrl + "/" + id);
        }

        internal async Task<T> PublisherLocation<T>(string id) where T : IHttpResult
        {
            return await Get<T>(UrlConstants.PublisherLocationUrl + "/" + id);
        }

        public async Task<T> GetPublisher<T>(string id) where T : IHttpResult
        {
            return await Get<T>(UrlConstants.OnePublishersUrl + "/" + id);
        }

        public async Task<T> ActivateOffers<T>(object item) where T : IHttpResult
        {
            return await Put<T>(item, UrlConstants.ActivateOfferUrl);
        }

        public async Task<T> AddPublisherLocation<T>(object valueToSend) where T : IHttpResult
        {
            return await Create<T>(valueToSend, UrlConstants.AddPublisherLocationUrl, true);
        }

        public async Task<T> StartChat<T>(string id, string groupName) where T : IHttpResult
        {
            var queryStringList = new List<QueryString>() { new QueryString() { key = "groupName", value = groupName } };
            return await Get<T>(UrlConstants.StartChatUrl + "/" + id, queryStringList);
        }

        public async Task<T> LoadChat<T>(string name) where T : IHttpResult
        {
            return await Get<T>(UrlConstants.LoadChatUrl + "/" + name);
        }

        public async Task<T> LoadChats<T>() where T : IHttpResult
        {
            return await Get<T>(UrlConstants.LoadChatsUrl);
        }

        public async Task<T> MarkMessageAsRead<T>(object value) where T : IHttpResult
        {
            return await Create<T>(value, UrlConstants.MarkChatsReadUrl, true);
        }

        public async Task<T> Star<T>(object obj) where T : IHttpResult
        {
            return await Create<T>(obj, UrlConstants.StarOfferUrl, true);
        }

        public async Task<T> Report<T>(object obj) where T : IHttpResult
        {
            return await Create<T>(obj, UrlConstants.ReportOfferUrl, true);
        }

        public async Task<T> MyOffers<T>() where T : IHttpResult
        {
            return await Get<T>(UrlConstants.MyOfferListUrl);
        }

        public async Task<T> LoadConfiguration<T>() where T : IHttpResult
        {
            return await Get<T>(UrlConstants.LoadConfigurationUrl);
        }

        public async Task<T> UserInfo<T>() where T : IHttpResult
        {
            return await Get<T>(UrlConstants.UserApplicationDetailUrl);
        }

        internal async Task<T> UserDetail<T>() where T : IHttpResult
        {
            return await Get<T>(UrlConstants.MyDetailUrl);
        }

        public async Task<T> SearchOffers<T>(double latVal, double longVal, int maxDistanceInMeters, string distanceUnit, string[] categories, int pageNumber) where T : IHttpResult
        {
            var queryStrings = new List<QueryString>()
            {
                new QueryString{key= "currentLat", value=latVal.ToString() },
                new QueryString{key= "currentLong",value= longVal.ToString() } ,
                new QueryString{key= "maximumDistanceInMeters",value= maxDistanceInMeters.ToString() },
                new QueryString{key= "distanceUnit",value= distanceUnit },
                new QueryString{key= "pageNumber",value= pageNumber.ToString()}
            };

            if (categories != null && categories.Length > 0)
                foreach (var item in categories)
                    queryStrings.Add(new QueryString() { key = "categories", value = item });


            return await Get<T>(UrlConstants.OfferSearchUrl, queryStrings);
        }

        public async Task<T> Starred<T>(double latVal, double longVal, string unit) where T : IHttpResult
        {
            return await Get<T>(UrlConstants.StarredOfferUrl, new List<QueryString>() {
                new QueryString{key=  "currentLat", value=latVal.ToString() },
                new QueryString{key= "currentLong", value = longVal.ToString() } ,
                new QueryString{key=  "distanceUnit",value=unit}
            });
        }

        public async Task<T> OfferDetail<T>(string id, double currentLat, double currentLong, string unit) where T : IHttpResult
        {
            return await Get<T>(UrlConstants.MyDetailOfferUrl, new List<QueryString>() {
                new QueryString{key=  "id", value=id},
                new QueryString{key=  "currentLat", value=currentLat.ToString() },
                new QueryString{key= "currentLong", value = currentLong.ToString() } ,
                new QueryString{key=  "unit",value=unit}
            });
        }

        public async Task<T> OfferCategories<T>() where T : IHttpResult
        {
            return await Get<T>(UrlConstants.OfferCategoriesUrl);
        }

        public async Task<T> UserCategories<T>() where T : IHttpResult
        {
            return await Get<T>(UrlConstants.UserCategoriesUrl);
        }

        public async Task<T> PublicCategories<T>() where T : IHttpResult
        {
            return await Get<T>(UrlConstants.PublicOffers);
        }

        public async Task<T> SaveCategories<T>(object model) where T : IHttpResult
        {
            return await Put<T>(model, UrlConstants.UserCategoriesSaveUrl);
        }

        public async Task<T> PublisherLocations<T>() where T : IHttpResult
        {
            return await Get<T>(UrlConstants.PublisherLocationUrl);
        }

        public async Task<T> OfferLocations<T>() where T : IHttpResult
        {
            return await Get<T>(UrlConstants.PublisherLocationsUrl);
        }

        private void EnsureAuthotized()
        {
            var tokenToPass = Token ?? Context?.SessionModel?.Token?.token;
            var authHeader = new AuthenticationHeaderValue("bearer", tokenToPass);
            _client.DefaultRequestHeaders.Authorization = authHeader;
        }


        public async Task<TokenModel> Login(Dictionary<string, string> dict)
        {
            var uri = new Uri(UrlConstants.baseUrl + UrlConstants.LoginUrl);
            //var json = JsonConvert.SerializeObject(item);
            //var content = new StringContent(json, Encoding.UTF8, "application/json");
            //var formContent = new MultipartFormDataContent();
            //formContent.Add(content);


            var formContent = new FormUrlEncodedContent(dict);
            formContent.Headers.Add("grant_type", "password");
            HttpResponseMessage response = await _client.PostAsync(uri, formContent);
            var result = await response.Content.ReadAsStringAsync();
            Debug.Write(@"\tLogin successfull.");
            return JsonConvert.DeserializeObject<TokenModel>(result);
        }

        public async Task<ImageModel> UploadImageAsynch(Stream image, string fileName)
        {
            if (image != null)
            {
                var fileStreamContent = new StreamContent(image);
                fileStreamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(fileStreamContent);
                    this.EnsureAuthotized();
                    var response = await _client.PostAsync(UrlConstants.baseUrl + UrlConstants.StorageUploadUrl, formData);

                    var result = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine(@"\tLogin successfull.");
                    return JsonConvert.DeserializeObject<ImageModel>(result);
                }
            }

            return null;
        }

        public async Task<T> Get<T>(string url, List<QueryString> queryStrings = null) where T : IHttpResult
        {
            try
            {
                Debug.WriteLine("=========================== Trying to get the data from {0} ==============================", UrlConstants.baseUrl + url);
                EnsureAuthotized();

                var builder = new UriBuilder(UrlConstants.baseUrl + url);
                if (queryStrings != null && queryStrings.Count > 0)
                {
                    builder.Port = -1;
                    var query = HttpUtility.ParseQueryString(builder.Query);
                    foreach (var item in queryStrings)
                    {
                        query[item.key] = item.value;
                    }
                    builder.Query = query.ToString();

                }

                var Uri = new Uri(UrlConstants.baseUrl + url + builder.Query.ToString());
                //_client.DefaultRequestHeaders
                //      .Accept
                //      .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

                var response = await _client.GetAsync(Uri);
                if (response == null)
                {
                    Debug.WriteLine("=========================== Null Response Error while getting data from {0} ==============================", UrlConstants.baseUrl + url);
                }

                Debug.WriteLine("=========================== Success while getting data from {0} ==============================", UrlConstants.baseUrl + url);
                Debug.WriteLine("=========================== Successfully got data from {0} ==============================", UrlConstants.baseUrl + url);
                return await response.ToGenericObject<T>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("=========================== Error while getting data from {0} ==============================", UrlConstants.baseUrl + url);
                Debug.WriteLine("An error while processing {0} url, error details {1}", url, ex.Message);
            }

            return default;
        }

        private async Task<T> Put<T>(object item, string createOfferUrl) where T : IHttpResult
        {
            try
            {
                var uri = new Uri(UrlConstants.baseUrl + createOfferUrl);
                var content = PrepareHttpClientContent(item);

                EnsureAuthotized();

                var response = await _client.PutAsync(uri, content);
                return await response.ToGenericObject<T>();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tError:" + ex.HResult + "-" + ex.Message);
            }

            return default;
        }

        private async Task<T> Create<T>(object item, string createUrl, bool isSecure) where T : IHttpResult
        {
            try
            {
                var uri = new Uri(UrlConstants.baseUrl + createUrl);
                var content = PrepareHttpClientContent(item);

                if (isSecure)
                    EnsureAuthotized();

                var response = await _client.PostAsync(uri, content);
                return await response.ToGenericObject<T>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tError:" + ex.HResult + "-" + ex.Message);
            }

            return default(T);
        }

        private HttpContent PrepareHttpClientContent(object item)
        {
            var json = JsonConvert.SerializeObject(item);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }

        public class QueryString
        {
            public string key;
            public string value;
        }
    }
}
