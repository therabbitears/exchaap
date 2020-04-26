using System.Collections.Generic;

namespace exchaup.Models
{
    public class AnalyticsModel
    {
        public string EventName { get; set; }
        public IDictionary<string, string> Parameters { get; set; }

        private AnalyticsModel()
        {
            this.Parameters = new Dictionary<string, string>();
        }

        public static AnalyticsModel InstanceOf(string eventName)
        {
            return new AnalyticsModel() { EventName = eventName };
        }

        public static AnalyticsModel InstanceOf(string eventName, string parameter, string parameterValue)
        {
            return new AnalyticsModel()
            {
                EventName = eventName,
                Parameters = new Dictionary<string, string>() { { parameter, parameterValue } }
            };
        }

        public AnalyticsModel AddParameter(string parameter, string parameterValue)
        {
            this.Parameters.Add(parameter, parameterValue);
            return this;
        }

        public class EventNames
        {
            public const string PageViewEvent = "PageView";
            public const string Login = "login";
        }

        public class ParameterNames
        {
            public const string PageName = "PageName";
            public const string User = "PageName";
        }
    }
}
