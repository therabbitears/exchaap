namespace Xaminals.Services.HttpServices
{
    public class UrlConstants
    {
        //public const string baseUrl = "http://localhost:1234/";
        public const string baseUrl = "https://exchaup.sklative.com/";
        //public const string baseUrl = "http://localhost:64224/";


        public const string CreateUrl = "api/account/register";
        public const string UpdateuserUrl = "api/account/update";
        public const string VerifyTokenUrl = "api/account/verifytoken";
        public const string UpdatePasswordUrl = "api/account/updatepassword";
        public const string ForgotPasswordUrl = "api/account/forgot";
        public const string ResetPasswordUrl = "api/account/resetpassword";
        public const string CreateOfferUrl = "api/offers/create";
        public const string MyOfferListUrl = "api/offers/my";
        public const string OfferSearchUrl = "api/offers/search";
        public const string MyEditOfferUrl = "api/offers/edit";
        public const string MyDetailOfferUrl = "api/offers/details";
        public const string UpdateOfferUrl = "api/offers/update";
        public const string OfferCategoriesUrl = "api/categories/offer";
        public const string UserCategoriesUrl = "api/categories/subscribed";
        public const string UserCategoriesSaveUrl = "api/categories/save";
        public const string PublisherLocationsUrl = "api/publishers/locations";
        public const string PublisherLocationUrl = "api/publishers/locations";
        public const string CreatePublisherUrl = "api/publishers/create";
        public const string UpdatePublisherUrl = "api/publishers/update";
        public const string MyPublishersUrl = "api/publishers/my";
        public const string OnePublishersUrl = "api/publishers/one";
        public const string AddPublisherLocationUrl = "api/publishers/location";
        public const string StarOfferUrl = "api/offers/star";
        public const string StarredOfferUrl = "api/offers/starred";
        public const string ReportOfferUrl = "api/offers/report";
        public const string StorageUploadUrl = "api/storage/upload";
        public const string LoginUrl = "token";
        public const string UserApplicationDetailUrl = "api/subscription/detail";
        public const string MyDetailUrl = "api/account/detail";
        public const string UpdateConfigurationUrl = "api/users/updateconfiguration";
        public const string LoadConfigurationUrl = "api/users/settings";
        public const string UpdateuserSnapshotUrl = "api/users/updatesnapshot";
        public const string StartChatUrl = "api/conversations/start";
        public const string LoadChatUrl = "api/conversations/loadchat";
        public const string LoadChatsUrl = "api/conversations/all";
        public const string MarkChatsReadUrl = "api/conversations/markread";

        public const string SearchLocationsUrl = "api/locations/search";
        public const string PublicOffers = "api/categories/list";
        public const string ActivateOfferUrl = "api/offers/activate";
    }
}
