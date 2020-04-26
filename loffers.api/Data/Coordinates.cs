namespace Loffers.Server.Data
{
    public class Coordinates
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public Coordinates(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }

    public class CoordinatesShort 
    {
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
    }
}
