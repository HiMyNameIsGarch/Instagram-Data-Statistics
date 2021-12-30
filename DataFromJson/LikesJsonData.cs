using System.Collections.Generic;

namespace Instagram_Data_Statistics.DataFromJson
{
    public class LikesJsonData
    {
        public List<List<string>> media_likes { get; set; }
        public List<List<string>> comment_likes { get; set; }
    }
}
