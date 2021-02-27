using Instagram_Data_Statistics.DataFromJson;
using Instagram_Data_Statistics.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Instagram_Data_Statistics.Data
{
    public class SeenContent : BaseJsonData<SeenContentJsonData>, IBaseData
    {
        public SeenContent(string basePath) : base(basePath, "seen_content")
        {
        }
        private YearBasedSeenContent UserData = null;
        private Dictionary<string, ICollection<ChainingModel>> YearBasedModel = new Dictionary<string, ICollection<ChainingModel>>();
        private SeenContentType SeenType;
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
                        var response = ConsoleHelper.GetChoice("\n1.Choose what do you want to see:" +
                            " \n1.Search for an account" +
                            " \n2.Search for an account in a specific year"+
                            " \n3.See stats based on years", new ConsoleKey[] { ConsoleKey.D1 , ConsoleKey.D2, ConsoleKey.D3 });
                        switch (response)
                        {
                            case ConsoleKey.D1://search for an account
                                DisplayAccount(Data.chaining_seen);
                                break;
                            case ConsoleKey.D2:
                                var userYear = ConsoleHelper.GetChoice("\n2.Pick a year: ", YearBasedModel.Keys.ToArray());
                                var currentYear = YearBasedModel[userYear];
                                DisplayAccount(currentYear);
                                break;
                            case ConsoleKey.D3:
                                Console.WriteLine("\nTotal accounts seen: {0}", Data.chaining_seen.Count());
                                var activeYear = StatsHelper.GetMostActiveYear<ChainingModel>(YearBasedModel);
                                Console.WriteLine("Most active year was {0} with {1} accounts", activeYear.Item1, activeYear.Item2);
                                var nonActiveYear = StatsHelper.GetLessActiveYear<ChainingModel>(YearBasedModel);
                                Console.WriteLine("Less active year was {0} with {1} accounts", nonActiveYear.Item1, nonActiveYear.Item2);
                                Console.WriteLine();
                                foreach (var year in YearBasedModel)
                                {
                                    Console.WriteLine($"In {year.Key}: You saw {year.Value.Count} accounts but never interact!");
                                }
                                break;
                        }
                        break;
                    case ConsoleKey.D2:
                        SeenType = SeenContentType.Posts;
                        DisplayOptionsFor();
                        break;
                    case ConsoleKey.D3:
                        SeenType = SeenContentType.Videos;
                        DisplayOptionsFor();
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
        private void DisplayAccount(IEnumerable<ChainingModel> models)
        {
            while (true)
            {
                string accountName = ConsoleHelper.GetValue("\nInput an account name: ");
                string date = string.Empty;
                bool accFound = false;
                foreach (var account in models)
                {
                    if (account.username == accountName)
                    {
                        accFound = true;
                        date = ConvertToDateTimeString(account.timestamp);
                    }
                }
                if (accFound)
                {
                    Console.WriteLine("You saw {0} profile on {1}", accountName, date);
                    break;
                }
                else
                {
                    ConsoleHelper.WaitAndClearLines(4, 2000, "Ooops, that account does not exist, try again!");
                    continue;
                }
            }
        }
        public void DisplayOptionsFor()
        {
            while (true)
            {
                Console.WriteLine($"\nChoose what {SeenType} do you want to see:" +
                    "\n1.Show top accounts" +
                    "\n2.Show top accounts per year " +
                    "\n3.Search for an account" +
                    "\n4.Search for an account in a specific year" +
                    "\n5.Show stats based on years" +
                    "\n6.Change seen type"+
                    "\n7.Go back" +
                    "\nEsc.Exit application");
                var action = Console.ReadKey(true).Key;
                var baseData = UserData.GetCurrentMedia(SeenType);
                switch (action)
                {
                    case ConsoleKey.D1:
                        var data = baseData.Item2;
                        int maxNum = data.Count;
                        int maxAccToShow = ConsoleHelper.GetNum($"\n1.Type how many account do you want to see: max {maxNum}",maxNum);
                        DisplayDic(data, maxAccToShow);
                        break;
                    case ConsoleKey.D2:
                        var yearBasedData = baseData.Item1;
                        string currentYear = ConsoleHelper.GetChoice("\n2.Pick a year: ",yearBasedData.Keys.ToArray());
                        var yearData = yearBasedData[currentYear];
                        int maxAcc = ConsoleHelper.GetNum($"\nType how many account do you want to see: max {yearData.Count}", yearData.Count);
                        Console.WriteLine("In {0}: ", currentYear);
                        DisplayDic(yearData, maxAcc);
                        break;
                    case ConsoleKey.D3:
                        SearchForAccount(baseData.Item2);
                        break;
                    case ConsoleKey.D4:
                        string year = ConsoleHelper.GetChoice("\n4.Pick a year: ", baseData.Item1.Keys.ToArray());
                        SearchForAccount(baseData.Item1[year]);
                        break;
                    case ConsoleKey.D5:
                        Console.WriteLine("\nTotal {1} seen: {0}", baseData.Item2.Count, SeenType);
                        var mostActiveYear = StatsHelper.GetMostActiveYear<string, int>(baseData.Item1);
                        Console.WriteLine("\nMost active year was: {0} with {1} {2} seen", mostActiveYear.Item1, SeenType, mostActiveYear.Item2);
                        var lessActiveYear = StatsHelper.GetLessActiveYear<string, int>(baseData.Item1);
                        Console.WriteLine("Less active year was: {0} with {1} {2} seen", lessActiveYear.Item1, SeenType, lessActiveYear.Item2);
                        foreach (var yearD5 in baseData.Item1)
                        {
                            Console.WriteLine($"In {yearD5.Key} you saw {yearD5.Value.Count} {SeenType}");
                        }
                        break;
                    case ConsoleKey.D6:
                        ConsoleHelper.ClearLines(10);
                        SeenType = SeenType == SeenContentType.Videos ? SeenContentType.Posts : SeenContentType.Videos;
                        continue;
                    case ConsoleKey.D7:
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
        private void DisplayDic(Dictionary<string, int> dic, int maxElem)
        {
            foreach (var account in dic.OrderByDescending(s => s.Value).Take(maxElem))
            {
                Console.WriteLine($"You saw {account.Value} {SeenType} from {account.Key}");
            }
        }
        private void SearchForAccount(Dictionary<string, int> data)
        {
            while (true)
            {
                string accountName = ConsoleHelper.GetValue("\nInput an account name");
                bool accFound = false;
                int accValue = 0;
                foreach (var account in data)
                {
                    if (account.Key == accountName)
                    {
                        accValue = account.Value;
                        accFound = true;
                    }
                }
                if (accFound)
                {
                    Console.WriteLine($"You saw {accValue} {SeenType} from {accountName}");
                    break;
                }
                else
                {
                    ConsoleHelper.WaitAndClearLines(4, 2000, "Ooops, that account does not exists, try again!");
                    continue;
                }
            }
        }
        public void OrganizeDataFromObject()
        {
            foreach (var account in Data.chaining_seen)
            {
                var year = account.timestamp.Substring(0, 4);
                if (YearBasedModel.ContainsKey(year))
                    YearBasedModel[year].Add(account);
                else
                    YearBasedModel.Add(year, new List<ChainingModel>() { new ChainingModel() { timestamp = account.timestamp, username = account.username} });
            }
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
