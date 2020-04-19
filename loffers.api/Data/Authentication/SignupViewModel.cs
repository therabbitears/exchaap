namespace Loffers.Server.Data.Authentication
{
    public class SignupViewModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }


        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }


        private string _confirmPassword;

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { _confirmPassword = value; }
        }
    }
}
