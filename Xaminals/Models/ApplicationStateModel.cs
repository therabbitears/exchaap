namespace exchaup.Models
{
    public class ApplicationStateModel
    {
        public bool SkipIntro { get; set; }
        public string LastLocationName { get; set; }
        public bool CustomLocation { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
    }
}
