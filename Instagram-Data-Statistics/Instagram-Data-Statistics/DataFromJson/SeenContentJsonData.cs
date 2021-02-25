using Instagram_Data_Statistics.Enums;
using System.Collections.Generic;

namespace Instagram_Data_Statistics.DataFromJson
{
    public class SeenContentJsonData
    {
        public IEnumerable<ChainingModel> chaining_seen { get; set; }//account that you saw but never interact with
        public IEnumerable<SeenModel> videos_watched { get; set; }
        public IEnumerable<SeenModel> posts_seen { get; set; }
    }
    public class YearBasedSeenContent
    {
        public void AddList(IEnumerable<SeenModel> list, SeenContentType type)
        {
            var currentData = GetCurrentMedia(type);
            foreach (var account in list)
            {
                var year = account.timestamp.Substring(0,4);
                if (currentData.ContainsKey(year))
                {
                    var currentYear = currentData[year];
                    if (currentYear.ContainsKey(account.author))
                        currentYear[account.author]++;
                    else
                        currentYear.Add(account.author, 1);
                }
                else
                    currentData.Add(year, new Dictionary<string, int>() { { account.author, 1 } });
            }
        }
        public Dictionary<string, Dictionary<string, int>> GetCurrentMedia(SeenContentType type)
        {
            switch (type)
            {
                case SeenContentType.Posts:
                    return Posts;
                case SeenContentType.Videos:
                    return Videos;
            }
            return null;
        }
        Dictionary<string, Dictionary<string, int>> Videos { get; set; } = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, Dictionary<string, int>> Posts { get; set; } = new Dictionary<string, Dictionary<string, int>>();
    }
    public class SeenModel
    {
        public string timestamp { get; set; }
        public string author { get; set; }
    }
    public class ChainingModel
    {
        public string timestamp { get; set; }
        public string username { get; set; }
    }
}
