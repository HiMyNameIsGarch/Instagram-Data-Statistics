using System.Collections.Generic;

namespace Instagram_Data_Statistics
{
    public class LikesData
    {
        public Dictionary<string, int> Account { get; set; } = new Dictionary<string, int>();//store all accounts and likes no matter year
        public Dictionary<string, Dictionary<string, int>> YearBased { get; set; } = new Dictionary<string, Dictionary<string, int>>();//store accounts likes based on year
    }
}
