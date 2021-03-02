using Instagram_Data_Statistics.DataFromJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Instagram_Data_Statistics.Data
{
    public class Saved : BaseJsonData<SavedDataJson>, IBaseData
    {
        public Saved(string basePath) : base(basePath, "saved")
        {
        }
        private YearBasedSavedData UserData = new YearBasedSavedData();
        public void DisplayOptions()
        {
            while (true)
            {
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);
                ConsoleHelper.WriteAndColorLine(AdditionalInformation, ConsoleColor.Cyan);
                Console.WriteLine("\nChoose what media do you want to see next: " +
                    "\n1.Collections " +
                    "\n2.All media " +
                    "\n3.Go to main menu"+
                    "\nEsc. Exit Application");
                var action = Console.ReadKey(true).Key;
                switch (action)
                {
                    case ConsoleKey.D1:
                        if(Data.saved_collections.Count == 0)
                        {
                            Console.WriteLine("You don't have any collections media");
                            Thread.Sleep(2000);
                            Console.Clear();
                            continue;
                        }
                        else
                        {
                            int i = 1;
                            Console.WriteLine("Press the number in front of collection to see its media");
                            foreach (var collection in Data.saved_collections)
                            {
                                Console.WriteLine($"\n{i}.{collection.name} " +
                                    $"\nYou created this collection on {ConvertToDateTimeString(collection.created_at)} " +
                                    $"\nYour last update was on {ConvertToDateTimeString(collection.updated_at)}" +
                                    $" \nYou have a total of {collection.media.Count()} posts saved");
                                i++;
                            }
                            int dataIndex = -1;
                            while (true)
                            {
                                string collectionNum = Console.ReadLine();
                                if(int.TryParse(collectionNum, out int newNum))
                                {
                                    if(newNum > 0 && newNum <= Data.saved_collections.Count)
                                    {
                                        dataIndex = newNum;
                                        break;
                                    }
                                    else
                                    {
                                        ConsoleHelper.ClearLines();
                                        continue;
                                    }
                                }
                                else
                                {
                                    ConsoleHelper.ClearLines();
                                    continue;
                                }
                            }
                            DisplayOptionsFor(new Tuple<Dictionary<string, int>, Dictionary<string, Dictionary<string, int>>>(UserData.GetYearBasedColl(dataIndex), UserData.GetCollection(dataIndex)));
                        }
                        break;
                    case ConsoleKey.D2:
                        DisplayOptionsFor(new Tuple<Dictionary<string, int>, Dictionary<string, Dictionary<string, int>>>(UserData.Media, UserData.YearBasedMedia));
                        break;
                    case ConsoleKey.D3:
                        return;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        return;
                    default:
                        Console.Clear();
                        continue;
                }
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);
                var respone = WantUserToContinue("saved");
                if (respone == ConsoleKey.D1)
                    continue;
                else if (respone == ConsoleKey.D2)
                    return;
                else
                    Environment.Exit(0);
            }
        }
        private void DisplayOptionsFor(Tuple<Dictionary<string,int>, Dictionary<string, Dictionary<string,int>>> list)
        {
            if(list.Item2.Count < 1)
            {
                Console.WriteLine("You have no items here!");
                return;
            }
            while (true)
            {
                Console.WriteLine("\nChoose what do you want to do: " +
                    "\n1.Top accounts that you saved their posts" +
                    "\n2.Top accounts in a specific year" +
                    "\n3.Top accounts per year"+
                    "\n4.Search for an account"+
                    "\n5.Search for an account in a specific year"+
                    "\n6.Show stats based on years"+
                    "\n7.Go to main menu" +
                    "\nEsc. Exit application");
                var action = Console.ReadKey(true).Key;
                switch (action)
                {
                    case ConsoleKey.D1://Top accounts that you saved their posts
                        int maxNum = list.Item1.Count;
                        int maxAccToShow = ConsoleHelper.GetNum($"\n1.How many accounts do you want to see, max: {maxNum}", maxNum);
                        DisplayTopAccountsFrom(list.Item1, maxAccToShow);
                        break;
                    case ConsoleKey.D2://Top accounts in a specific year
                        string currentYear = ConsoleHelper.GetChoice("\n2.Pick a year: ", list.Item2.Keys.ToArray());
                        var yearData = list.Item2[currentYear];
                        int maxAccountToShow = yearData.Count;
                        int numberOfAccToShow = ConsoleHelper.GetNum($"\n1.How many accounts do you want to see, max: {maxAccountToShow}", maxAccountToShow);
                        Console.WriteLine("In {0}", currentYear);
                        DisplayTopAccountsFrom(yearData, numberOfAccToShow);
                        break;
                    case ConsoleKey.D3://Search for an account
                        int maxAccounts = ConsoleHelper.GetNum("3.How many accounts do you want to see?");
                        foreach (var currentYearD3 in list.Item2)
                        {
                            Console.WriteLine("\nIn {0}", currentYearD3.Key);
                            DisplayTopAccountsFrom(list.Item2[currentYearD3.Key], maxAccounts);
                        }
                        break;
                    case ConsoleKey.D4://Search for an account
                        DisplayAccount(list.Item1);
                        break;
                    case ConsoleKey.D5://Search for an account in a specific year
                        string year = ConsoleHelper.GetChoice("\n5.Pick a year: ", list.Item2.Keys.ToArray());
                        var currentYearData = list.Item2[year];
                        DisplayAccount(currentYearData);
                        break;
                    case ConsoleKey.D6://Show stats based on years
                        Tuple<string, int> mostActive = StatsHelper.GetMostActiveYear<string,int>(list.Item2);
                        Tuple<string, int> lessActive = StatsHelper.GetLessActiveYear<string,int>(list.Item2);
                        Console.WriteLine();
                        Console.WriteLine($"{mostActive.Item1} was the year with most active saved posts ({mostActive.Item2} posts saved)");
                        Console.WriteLine($"{lessActive.Item1} was the year with less active saved posts({lessActive.Item2} posts saved)");
                        break;
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
        private void DisplayTopAccountsFrom(Dictionary<string,int> list, int maxAcc)
        {
            foreach (var item in list.OrderByDescending(s => s.Value).Take(maxAcc))
            {
                Console.WriteLine($"You saved {item.Value} posts from {item.Key}");
            }
        }
        private void DisplayAccount(Dictionary<string,int> data)
        {
            while (true)
            {
                var name = ConsoleHelper.GetValue("\nInput an account name");
                if (name == ExitKeyword) break;
                var account = GetAccountFrom(data, name);
                if (account != null)
                {
                    ConsoleHelper.WriteAndColorLine($"You saved {account.Item2} posts from {account.Item1}", ConsoleColor.Green);
                    return;
                }
                else
                {
                    ConsoleHelper.WaitAndClearLines(4, 2000, "Ooops, that account does not exist, try again!");
                    continue;
                }
            }
        }
        private Tuple<string,int> GetAccountFrom(Dictionary<string,int> data, string accName)
        {
            Tuple<string, int> account = null;
            foreach (var acc in data)
            {
                if(acc.Key == accName)
                {
                    return new Tuple<string, int>(acc.Key, acc.Value);
                }
            }
            return account;
        }
        public void OrganizeDataFromObject()
        {
            AdditionalInformation = $"You have a total of {Data.saved_media.Count()} posts saved and {Data.saved_collections.Count} colections";
            UserData.StoreData(Data);
        }
    }
}
