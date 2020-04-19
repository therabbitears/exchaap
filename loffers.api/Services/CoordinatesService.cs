using Loffers.Server.Data;
using System;

namespace Loffers.Server.Services
{
    public enum ShowDistanceIn
    {
        Kilometers,
        Miles,
        Meters
    }

    public class CoordinatesService
    {

    }

    public static class CoordinatesDistanceExtensions
    {
        public static double DistanceTo(this Coordinates baseCoordinates, Coordinates targetCoordinates, int unit = (int)ShowDistanceIn.Kilometers)
        {
            if (unit == (int)ShowDistanceIn.Miles)
                return DistanceTo(baseCoordinates, targetCoordinates, UnitOfLength.Miles);
            else if (unit == (int)ShowDistanceIn.Meters)
                return DistanceTo(baseCoordinates, targetCoordinates, UnitOfLength.Meters);

            return DistanceTo(baseCoordinates, targetCoordinates, UnitOfLength.Kilometers);
        }

        public static double DistanceTo(this Coordinates baseCoordinates, Coordinates targetCoordinates, UnitOfLength unitOfLength)
        {
            var baseRad = Math.PI * baseCoordinates.Latitude / 180;
            var targetRad = Math.PI * targetCoordinates.Latitude / 180;
            var theta = baseCoordinates.Longitude - targetCoordinates.Longitude;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return unitOfLength.ConvertFromMiles(dist);
        }
    }
}
