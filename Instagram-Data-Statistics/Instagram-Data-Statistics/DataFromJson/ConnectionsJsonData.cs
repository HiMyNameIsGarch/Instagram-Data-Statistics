using Instagram_Data_Statistics.Enums;
using System.Collections.Generic;

namespace Instagram_Data_Statistics.DataFromJson
{
    public class ConnectionsJsonData
    {
        public Dictionary<string,string> blocked_users { get; set; }
        public Dictionary<string, string> follow_requests_sent { get; set; }
        public Dictionary<string, string> permanent_follow_requests { get; set; }
        public Dictionary<string,string> followers { get; set; }
        public Dictionary<string,string> following { get; set; }
        public Dictionary<string,string> following_hashtags { get; set; }
        public Dictionary<string,string> dismissed_suggested_users { get; set; }
    }
    public class YearBasedConnectionsData
    {
        //functions
        public void AddDictionary(Dictionary<string,string> dic, ConnectionType type)
        {
            foreach (var item in dic)
            {
                var year = item.Value.Substring(0,4);
                var currentData = GetCurrentUsers(type);
                if (currentData.ContainsKey(year))
                    currentData[year].Add(item.Key);
                else
                    currentData.Add(year, new List<string>() { item.Key });
            }
        }
        public Dictionary<string, ICollection<string>> GetCurrentUsers(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.BlockedUsers:
                    return blocked_users;
                case ConnectionType.FollowRequestsSent:
                    return follow_requests_sent;
                case ConnectionType.PermanetFollowRequests:
                    return permanent_follow_requests;
                case ConnectionType.Followers:
                    return followers;
                case ConnectionType.Following:
                    return following;
                case ConnectionType.FollowingHashtags:
                    return following_hashtags;
                case ConnectionType.DismissedSuggestedUsers:
                    return dismissed_suggested_users;
            }
            return null;
        }
        //props
        public Dictionary<string, ICollection<string>> blocked_users { get; set; } = new Dictionary<string, ICollection<string>>();
        public Dictionary<string, ICollection<string>> follow_requests_sent { get; set; } = new Dictionary<string, ICollection<string>>();
        public Dictionary<string, ICollection<string>> permanent_follow_requests { get; set; } = new Dictionary<string, ICollection<string>>();
        public Dictionary<string, ICollection<string>> followers { get; set; } = new Dictionary<string, ICollection<string>>();
        public Dictionary<string, ICollection<string>> following { get; set; } = new Dictionary<string, ICollection<string>>();
        public Dictionary<string, ICollection<string>> following_hashtags { get; set; } = new Dictionary<string, ICollection<string>>();
        public Dictionary<string, ICollection<string>> dismissed_suggested_users { get; set; } = new Dictionary<string, ICollection<string>>();
    }
}
