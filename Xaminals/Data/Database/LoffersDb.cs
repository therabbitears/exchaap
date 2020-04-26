using exchaup.Models;
using exchaup.Views.Offer_Public.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xaminals.Models;
using Xaminals.Views.Offers.ViewModels;

namespace Xaminals.Data.Database
{
    public class LoffersDb
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(DbConstants.DatabasePath, DbConstants.Flags);
        });

        public static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public LoffersDb()
        {
            InitializeAsync().SafeFireAndForget(false, onExceptio);
        }

        private void onExceptio(Exception obj)
        {
            // Do nothing for now.
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(TokenModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(TokenModel)).ConfigureAwait(false);
                }

                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(SearchLocationItemViewModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(SearchLocationItemViewModel)).ConfigureAwait(false);
                }

                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(ApplicationStateModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(ApplicationStateModel)).ConfigureAwait(false);
                }

                //if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(OfferListItemViewModel).Name))
                //{
                //    await Database.CreateTablesAsync(CreateFlags.None, typeof(OfferListItemViewModel)).ConfigureAwait(false);
                //}

                initialized = true;
            }
        }


        public Task<List<OfferListItemViewModel>> GetItemsAsync()
        {
            return Database.Table<OfferListItemViewModel>().ToListAsync();
        }

        public Task<List<TokenModel>> AllTokens()
        {
            return Database.Table<TokenModel>().ToListAsync();
        }

        public Task<List<OfferListItemViewModel>> GetItemsNotDoneAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<OfferListItemViewModel>("SELECT * FROM [OfferListItemModel] WHERE [Done] = 0");
        }

        public Task<OfferListItemViewModel> GetItemAsync(string id)
        {
            return Database.Table<OfferListItemViewModel>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(OfferListItemViewModel item)
        {
            if (!string.IsNullOrEmpty(item.Id))
                return Database.UpdateAsync(item);
            else
                return Database.InsertAsync(item);
        }

        public Task<int> SaveItemAsync(List<OfferListItemViewModel> item)
        {
            return Database.InsertAllAsync(item);
        }

        public Task<int> SaveTokenAsync(TokenModel item)
        {
            return Database.InsertAsync(item);
        }

        public Task<SearchLocationItemViewModel> FindSingle(SearchLocationItemViewModel item)
        {
            return Database.Table<SearchLocationItemViewModel>().Where(c => c.Name == item.Name && c.Lat == item.Lat && c.Long == item.Long).FirstOrDefaultAsync();
        }

        public Task<ApplicationStateModel> FindLastState()
        {
            return Database.Table<ApplicationStateModel>().FirstOrDefaultAsync();
        }

        public Task<int> DeleteStates()
        {
            return Database.DeleteAllAsync<ApplicationStateModel>();
        }

        public Task<int> SaveLastState(ApplicationStateModel item)
        {
            return Database.InsertAsync(item);
        }

        public Task<int> UpdateLastState(ApplicationStateModel item)
        {
            return Database.UpdateAsync(item);
        }

        public Task<int> SaveLocationAsync(SearchLocationItemViewModel item)
        {
            return Database.InsertAsync(item);
        }

        public Task<List<SearchLocationItemViewModel>> GetAllLocationsAsync()
        {
            return Database.Table<SearchLocationItemViewModel>().ToListAsync();
        }

        public Task<int> DeleteTokenAsync()
        {
            return Database.DeleteAllAsync<TokenModel>();
        }



        public Task<int> DeleteItemAsync(OfferListItemViewModel item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
