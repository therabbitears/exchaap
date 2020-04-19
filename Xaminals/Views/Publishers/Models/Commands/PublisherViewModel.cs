using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Models;
using Xaminals.Services.HttpServices;
using static Xaminals.Services.HttpServices.RestService;

namespace Xaminals.Views.Publishers.Models
{
    public partial class PublisherViewModel
    {
        public ICommand SaveCommand { get; set; }
        public ICommand LoadPublisherCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            SaveCommand = new Command(Save);
            SelectImageCommand = new Command(SelectImage);
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            this.PropertyChanged += OnPropertryChanged;
            /// Id = "df122324-1018-40dd-961e-f83cb3865c75";
        }

        private void OnPropertryChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e != null && e.PropertyName == "Id")
            {
                LoadPublisherCommand = LoadPublisherCommand ?? new Command(async () => await ExecuteLoadPublisherCommand());
                LoadPublisherCommand.Execute(null);
            }
        }

        private async Task ExecuteLoadPublisherCommand()
        {
            try
            {
                var result = await new RestService().GetPublisher<HttpResult<PublisherListItemViewModel>>(Id);
                if (!result.IsError)
                {
                    this.Name.Value = result.Result.Name;
                    this.Description.Value = result.Result.Description;
                    this.Image = result.Result.Image;
                    this.SourceImage = ImageSource.FromUri(new Uri(result.Result.Image));
                }
                else
                {
                    await RaiseError(result.Errors.First().Description);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await RaiseError("An error occurred while loading publisher details.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void SelectImage(object obj)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                //await DisplayAlert("Error", "This is not support on your device.", "OK");
                return;
            }
            else
            {
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };

                _mediaFile = await CrossMedia.Current.PickPhotoAsync();
                if (_mediaFile == null) return;

                ImageStream = _mediaFile.GetStream();
                try
                {
                    SourceImage = ImageSource.FromStream(() => _mediaFile.GetStream());
                }
                catch (Exception ex)
                {
                }
            }
        }

        private async void Save(object obj)
        {
            if (Validate())
            {
                IsBusy = true;

                var service = new RestService();
                ImageModel imageModel = null;
                if (ImageStream != null)
                {
                    imageModel = await service.UploadImageAsynch(ImageStream, this.Name.Value.Replace(" ", string.Empty) + ".png");
                }

                var model = new
                {
                    Name = this.Name.Value,
                    Description = this.Description.Value,
                    Image = imageModel != null ? imageModel.url : this.Image,
                    Id
                };
                try
                {
                    var result = string.IsNullOrEmpty(this.Id) ? await service.CreatePublisher<HttpResult<object>>(model) : await service.UpdatePublisher<HttpResult<object>>(model);

                    if (!result.IsError)
                    {
                        await ExecuteCancelCommand();
                        await RaiseSuccess("The publisher has been saved successfully.");
                    }
                    else
                        await RaiseError(result.Errors.First().Description);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await RaiseError("An error occurred while creating publisher.");
                }
                finally
                {
                    IsBusy = false;
                }

                return;
            }
        }
    }
}
