using Instagram_Data_Statistics.Enums;
using System.Collections.Generic;

namespace Instagram_Data_Statistics.DataFromJson
{
    public class StoriesJsonData
    {
       public List<List<string>> polls { get; set; }
       public List<List<string>> emoji_sliders { get; set; }
       public List<List<string>> countdowns { get; set; }
       public List<List<string>> quizzes { get; set; }
    }
    public class YearBasedStoriesData
    {
        public void AddStoryType(List<List<string>> list, StoryType type)
        {
            foreach (var item in list)
            {
                var mainKey = item[0].Substring(0, 4);
                var secondKey = item[1];
                var currentStory = GetCurrentStory(type);
                if (currentStory.ContainsKey(mainKey))
                {
                    if (currentStory[mainKey].ContainsKey(secondKey))
                        currentStory[mainKey][secondKey]++;
                    else
                        currentStory[mainKey].Add(secondKey, 1);
                }
                else
                    currentStory.Add(mainKey, new Dictionary<string, int>() { { secondKey, 1 } });

            }
        }
        public Dictionary<string, Dictionary<string, int>> GetCurrentStory(StoryType type)
        {
            switch (type)
            {
                case StoryType.Polls:
                    return polls;
                case StoryType.EmojiSliders:
                    return emoji_sliders;
                case StoryType.Countdowns:
                    return countdowns;
                case StoryType.Quizzes:
                    return quizzes;
            }
            return null;
        }
        public Dictionary<string, Dictionary<string, int>> polls { get; set; } = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, Dictionary<string, int>> emoji_sliders { get; set; } = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, Dictionary<string, int>> countdowns { get; set; } = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, Dictionary<string, int>> quizzes { get; set; } = new Dictionary<string, Dictionary<string, int>>();
    }
}
