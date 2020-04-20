using System;

namespace Xaminals.Models
{
    public class SessionModel : NotificableObject
    {
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
