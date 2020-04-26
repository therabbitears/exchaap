using System;
using System.Collections.Generic;
using Android.OS;
using exchaup;
using Firebase.Analytics;
using Xamarin.Forms;
using Xaminals.Droid;

[assembly: Dependency(typeof(FirebaseAnalyticsCommon))]
namespace Xaminals.Droid
{

    public class FirebaseAnalyticsCommon : IFirebaseAnalytics
    {
        public void SendEvent(string eventId)
        {
            SendEvent(eventId, null);
        }

        public void SendEvent(string eventId, string paramName, string value)
        {
            SendEvent(eventId, new Dictionary<string, string>
            {
                {paramName, value}
            }); ;
        }

        public void SendEvent(string eventId, IDictionary<string, string> parameters)
        {
            var firebaseAnalytics = FirebaseAnalytics.GetInstance(Forms.Context);

            if (parameters == null)
            {
                firebaseAnalytics.LogEvent(eventId, null);
                return;
            }

            var bundle = new Bundle();
            foreach (var param in parameters)
            {
                bundle.PutString(param.Key, param.Value);
            }

            firebaseAnalytics.LogEvent(eventId, bundle);
        }
    }
}
