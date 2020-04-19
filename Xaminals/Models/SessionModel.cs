using System;

namespace Xaminals.Models
{
    public class SessionModel : NotificableObject
    {
        public delegate void AppSpaceChanged(bool isPublisher);
        public event AppSpaceChanged OnOnAppSpaceChanged;

        private bool _isLogib;
        public bool IsLoggedIn
        {
            get { return _isLogib; }
            set
            {
                if (_isLogib != value)
                {
                    _isLogib = value; 
                    OnPropertyChanged("IsLoggedIn");
                }
            }
        }


        private bool _IsPublisher;
        public bool IsPublisher
        {
            get { return _IsPublisher; }
            set
            {
                if (_IsPublisher != value)
                {
                    _IsPublisher = value;
                    OnPropertyChanged("IsPublisher");
                    OnOnAppSpaceChanged?.Invoke(value);
                }
            }
        }

        private static readonly Lazy<TokenModel> _token = new Lazy<TokenModel>();
        public TokenModel Token
        {
            get
            {
                return _token.Value;
            }
        }

    }
}
