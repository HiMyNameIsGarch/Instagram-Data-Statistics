using Instagram_Data_Statistics.DataFromJson;
using Instagram_Data_Statistics.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Instagram_Data_Statistics.Data
{
    public class SeenContent : BaseJsonData<SeenContentJsonData>, IBaseData
    {
        public SeenContent(string basePath) : base(basePath, "seen_content")
        {
        }
        private YearBasedSeenContent UserData = null;
        public void DisplayOptions()
        {
            while (true)
            {
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);
                Console.WriteLine("There are 3 types of seen content: Chaining seen, Posts and Videos \nChoose what you want to do on: \n1.Chaining seen \n2.Posts \n3.Videos");
                var action = Console.ReadKey(true).Key;
                switch (action)
                {
                    case ConsoleKey.D1:
                        var response = ConsoleHelper.GetChoice("\n1.Choose what do you want to see: \n1.Search for an account \n2.See stats based on years", new ConsoleKey[] { ConsoleKey.D1 , ConsoleKey.D2 });
                        switch (response)
                        {
                            case ConsoleKey.D1:
                                break;
                            case ConsoleKey.D2:
                                break;
                        }
                        break;
                    case ConsoleKey.D2:
                        DisplayOptionsFor(SeenContentType.Posts);
                        break;
                    case ConsoleKey.D3:
                        DisplayOptionsFor(SeenContentType.Videos);
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        return;
                    default:
                        Console.Clear();
                        continue;
                }
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);
                var respone = WantUserToContinue("seen content");
                if (respone == ConsoleKey.D1)
                    continue;
                else if (respone == ConsoleKey.D2)
                    return;
                else
                    Environment.Exit(0);
            }
        }
        public void DisplayOptionsFor(SeenContentType type)
        {
            var currentData = UserData.GetCurrentMedia(type);
            while (true)
            {
                Console.WriteLine("\nChoose what do you want to see:" +
                    " \n1.Show top accounts" +
                    " \n2.Show top accounts per year " +
                    "\n3.Search for an account" +
                    "\n4.Search for an account in a specific year" +
                    "\n5.Show most active year " +
                    "\n6.Go back" +
                    "\nEsc.Exit application");
                var action = Console.ReadKey(true).Key;
                switch (action)
                {
                    case ConsoleKey.D1:
                        break;
                    case ConsoleKey.D2:
                        break;
                    case ConsoleKey.D3:
                        break;
                    case ConsoleKey.D4:
                        break;
                    case ConsoleKey.D5:
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
        public void OrganizeDataFromObject()
        {
            UserData = StoreData(Data);
        }
        private YearBasedSeenContent StoreData(SeenContentJsonData data)
        {
            var newData = new YearBasedSeenContent();
            newData.AddList(data.posts_seen, SeenContentType.Posts);
            newData.AddList(data.videos_watched, SeenContentType.Videos);
            return newData;
        }
    }
}
