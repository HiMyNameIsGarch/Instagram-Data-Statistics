using Instagram_Data_Statistics.Enums;
using System;
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
            var data = GetCurrentMedia(type);
            var allData = data.Item1;
            var yearBasedData = data.Item2;
            foreach (var account in list)
            {
                var year = account.timestamp.Substring(0,4);
                if (allData.ContainsKey(year))
                {
                    var currentYear = allData[year];
                    if (currentYear.ContainsKey(account.author))
                        currentYear[account.author]++;
                    else
                        currentYear.Add(account.author, 1);
                }
                else
                    allData.Add(year, new Dictionary<string, int>() { { account.author, 1 } });

                if (yearBasedData.ContainsKey(account.author))
                    yearBasedData[account.author]++;
                else
                    yearBasedData.Add(account.author, 1);
            }
        }
        public Tuple<Dictionary<string, Dictionary<string, int>>, Dictionary<string,int>> GetCurrentMedia(SeenContentType type)
        {
            switch (type)
            {
                case SeenContentType.Posts:
                    return new Tuple<Dictionary<string, Dictionary<string, int>>, Dictionary<string, int>>(YearBasedVideos, Videos);
                case SeenContentType.Videos:
                    return new Tuple<Dictionary<string, Dictionary<string, int>>, Dictionary<string, int>>(YearBasedPosts, Posts);
            }
            return null;
        }
        Dictionary<string, Dictionary<string, int>> YearBasedVideos { get; set; } = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, Dictionary<string, int>> YearBasedPosts { get; set; } = new Dictionary<string, Dictionary<string, int>>();
        Dictionary<string, int> Videos { get; set; } = new Dictionary<string, int>();
        Dictionary<string, int> Posts { get; set; } = new Dictionary<string, int>();
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
