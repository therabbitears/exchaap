using System;
using Xaminals.Data.Database;
using Xaminals.Models;
using Xaminals.ViewModels.Offers;
using Xaminals.ViewModels.Settings;

namespace Xaminals.Infra.Context
{
    /// <summary>
    /// Search model
    /// </summary>
    public class SingletonLoffersContext
    {
        private static readonly Lazy<SingletonLoffersContext> InstaceVariable = new Lazy<SingletonLoffersContext>();
        public static SingletonLoffersContext Context
        {
            get
            {
                return InstaceVariable.Value;
            }
        }
       

        private static readonly Lazy<SessionModel> _session = new Lazy<SessionModel>();
        public SessionModel SessionModel
        {
            get
            {
                return _session.Value;
            }
        }

        private static readonly Lazy<SearchViewModel> SearchInstance = new Lazy<SearchViewModel>();
        public SearchViewModel SearchModel
        {
            get
            {
                return SearchInstance.Value;
            }
        }

        private static readonly Lazy<LoffersDb> DatabaseInstance = new Lazy<LoffersDb>();
        public LoffersDb Database
        {
            get
            {
                return DatabaseInstance.Value;
            }
        }

        private static readonly Lazy<SettingPageViewModel> SettingsInstance = new Lazy<SettingPageViewModel>();
        public SettingPageViewModel SettingsModel
        {
            get
            {
                return SettingsInstance.Value;
            }
        }
    }
}
