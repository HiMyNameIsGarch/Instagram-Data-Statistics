using Instagram_Data_Statistics.DataFromJson;
using Instagram_Data_Statistics.Enums;
using System;
using System.Linq;

namespace Instagram_Data_Statistics.Data
{
    public class Stories : BaseJsonData<StoriesJsonData>, IBaseData
    {
        public Stories(string basePath) : base(basePath, "stories_activities")
        {
        }
        private string AdditionalInformation = string.Empty;
        private StoryType StoryType = StoryType.Countdowns;
        private YearBasedStoriesData YearBasedStories = null;
        public void DisplayOptions()
        {
            while (true)
            {
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);
                ConsoleHelper.WriteAndColorLine(AdditionalInformation, ConsoleColor.Cyan);
                Console.WriteLine("\nChoose what story type do you wanna see: " +
                    "\n1.Polls " +
                    "\n2.Emoji Sliders " +
                    "\n3.Countdowns " +
                    "\n4.Quizzes " +
                    "\nEsc. Exit Application");
                var action = Console.ReadKey(true).Key;
                switch (action)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("Polls:");
                        StoryType = StoryType.Polls;
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine("Emoji Sliders:");
                        StoryType = StoryType.EmojiSliders;
                        break;
                    case ConsoleKey.D3:
                        Console.WriteLine("Countdowns:");
                        StoryType = StoryType.Countdowns;
                        break;
                    case ConsoleKey.D4:
                        Console.WriteLine("Quizzes:");
                        StoryType = StoryType.Quizzes;
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        continue;
                }
                DisplayOptionsFor(StoryType);
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);
                var respone = WantUserToContinue("stories");
                if (respone == ConsoleKey.D1)
                    continue;
                else if (respone == ConsoleKey.D2)
                    return;
                else
                    Environment.Exit(0);
            }
        }
        public void OrganizeDataFromObject()
        {
            AdditionalInformation = $"Your responded to {Data.polls.Count} polls \nSlided to {Data.emoji_sliders.Count} emoji sliders \nPut {Data.countdowns.Count} countdowns \nAnd answered to {Data.quizzes.Count} quizzes";
            YearBasedStories = StoreData(Data);
        }
        private void DisplayOptionsFor(StoryType type)
        {
            var currentData = YearBasedStories.GetCurrentStory(type);
            while (true)
            {
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Blue);
                Console.WriteLine("\nWhat do you want to do next? " +
                     "\n1.See how many responses you gave based on years" +
                     "\n2.See how many responses you gave in a specific year" +
                     "\n3.See how many responses you gave to a specific account" +
                     "\n4.See how many responses you gave to a specific account based on years" +
                     "\n5.See how many responses you gave to a specific account in a specific year" +
                     "\n6.Go back to stories type" +
                     "\nEsc. Exit Application");
                var action = Console.ReadKey(true).Key;
                switch (action)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("\n1.");
                        foreach (var year in currentData)
                        {
                            int totalResponses = 0;
                            foreach (var acc in year.Value)
                            {
                                totalResponses += acc.Value;
                            }
                            Console.WriteLine("In {0} you gave {1} responses", year.Key, totalResponses);
                        }
                        break;
                    case ConsoleKey.D2:
                        if(currentData.Keys.Count == 1)
                        {
                            var year = currentData.First();
                            Console.WriteLine("\n2.In {0} you gave {1} responses!", year.Key, year.Value.Count);
                            break;
                        }
                        var currentYear = ConsoleHelper.GetChoice("\n2.Pick a year: ", currentData.Keys.ToArray());
                        var responsesNum = 0;
                        foreach (var acc in currentData[currentYear])
                        {
                            responsesNum += acc.Value;
                        }
                        Console.WriteLine("\nIn {0} you gave {1} responses!", currentYear, responsesNum);
                        break;
                    case ConsoleKey.D3:
                        while (true)
                        {
                            string accName = ConsoleHelper.GetValue("\n3.Input an account name: ");
                            int totalResponsesInAcc = 0;
                            foreach (var year in currentData)
                            {
                                var currentYearValue = currentData[year.Key];
                                if (currentYearValue.ContainsKey(accName))
                                {
                                    totalResponsesInAcc += currentYearValue[accName];   
                                }
                            }
                            if(totalResponsesInAcc == 0)
                            {
                                ConsoleHelper.WaitAndClearLines(4, 2000, "Ooops, that account does not exist!, try again!");
                                continue;
                            }
                            else
                            {
                                Console.WriteLine("You gave {0} responses to {1}", totalResponsesInAcc, accName);
                                break;
                            }
                        }
                        break;
                    case ConsoleKey.D4:
                        string accountName = ConsoleHelper.GetValue("\n4.Input an account name: ");
                        foreach (var year in currentData)
                        {
                            int totalResponses = 0;
                            var currentYearValue = currentData[year.Key];
                            if (currentYearValue.ContainsKey(accountName))
                            {
                                totalResponses += currentYearValue[accountName];
                            }
                            if(totalResponses == 0)
                                Console.WriteLine("In {0} you didn't gave any responses to {1}", year.Key, accountName);
                            else
                                Console.WriteLine("In {0} you gave {1} responses to {2}", year.Key, totalResponses, accountName);
                        }
                        break;
                    case ConsoleKey.D5:
                        var yearD5 = ConsoleHelper.GetChoice("\n5.Pick a year: ", currentData.Keys.ToArray());
                        while (true)
                        {
                            string accNameD5 = ConsoleHelper.GetValue("\nInput an account name: ");
                            var yearData = currentData[yearD5];
                            if (yearData.ContainsKey(accNameD5))
                            {
                                Console.WriteLine("In {0} you gave {1} responses to {2}", yearD5, yearData[accNameD5], accNameD5);
                                break;
                            }
                            else
                            {
                                ConsoleHelper.WaitAndClearLines(4, 2000, "Ooops, that account does not exists, try again!");
                                continue;
                            }
                        }
                        break;
                    case ConsoleKey.D6:
                        return;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        continue;
                }
            }
        }
        private YearBasedStoriesData StoreData(StoriesJsonData data)
        {
            var newData = new YearBasedStoriesData();
            newData.AddStoryType(data.countdowns, StoryType.Countdowns);
            newData.AddStoryType(data.emoji_sliders, StoryType.EmojiSliders);
            newData.AddStoryType(data.polls, StoryType.Polls);
            newData.AddStoryType(data.quizzes, StoryType.Quizzes);
            return newData;
        }
    }
}
