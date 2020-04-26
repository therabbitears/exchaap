using exchaup;
using System.Collections.Generic;
using Xamarin.Forms;
using Xaminals.iOS;

[assembly: Dependency(typeof(FirebaseAnalyticsCommon))]
namespace Xaminals.iOS
{
    public class FirebaseAnalyticsCommon : IFirebaseAnalytics
    {
        public void SendEvent(string eventId)
        {
            SendEvent(eventId, (IDictionary<string, string>)null);
        }

        public void SendEvent(string eventId, string paramName, string value)
        {
            SendEvent(eventId, new Dictionary<string, string>
                        {
                          { paramName, value }
                        });
        }

        public void SendEvent(string eventId, IDictionary<string, string> parameters)
        {
            #if !DEBUG
            if (parameters == null)
            {
                Analytics.LogEvent(eventId, parameters: null);
                return;
            }

            var keys = new List<NSString>();
            var values = new List<NSString>();
            foreach (var item in parameters)
            {
                keys.Add(new NSString(item.Key));
                values.Add(new NSString(item.Value));
            }

            var parametersDictionary = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);
            Analytics.LogEvent(eventId, parametersDictionary);
            #endif
        }
    }
}