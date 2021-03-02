using Instagram_Data_Statistics.DataFromJson;
using Instagram_Data_Statistics.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Instagram_Data_Statistics.Data
{
    public class Connections : BaseJsonData<ConnectionsJsonData>, IBaseData
    {
        public Connections(string basePath) : base(basePath, "connections")
        {
        }
        private string AdditionalInformation = string.Empty;
        private YearBasedConnectionsData UserData = null;
        private ConnectionType CurrentConnectionType;
        public void DisplayOptions()
        {
            while (true)
            {
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);
                ConsoleHelper.WriteAndColorLine(AdditionalInformation, ConsoleColor.Cyan);
                Console.WriteLine("\nChoose one of those connection type:" +
                    "\n1.Followers" +
                    "\n2.Following" +
                    "\n3.Follow Requests Sent" +
                    "\n4.Permanent Follow Requests" +
                    "\n5.Following Hashtags" +
                    "\n6.Dismissed Suggested Users" +
                    "\n7.Blocked Users" +
                    "\n8.Go to main menu"+
                    "\nEsc. Exit Application");
                var action = Console.ReadKey(true).Key;
                switch (action)
                {
                    case ConsoleKey.D1:
                        CurrentConnectionType = ConnectionType.Followers;
                        break;
                    case ConsoleKey.D2:
                        CurrentConnectionType = ConnectionType.Following;
                        break;
                    case ConsoleKey.D3:
                        CurrentConnectionType = ConnectionType.FollowRequestsSent;
                        break;
                    case ConsoleKey.D4:
                        CurrentConnectionType = ConnectionType.PermanetFollowRequests;
                        break;
                    case ConsoleKey.D5:
                        CurrentConnectionType = ConnectionType.FollowingHashtags;
                        break;
                    case ConsoleKey.D6:
                        CurrentConnectionType = ConnectionType.DismissedSuggestedUsers;
                        break;
                    case ConsoleKey.D7:
                        CurrentConnectionType = ConnectionType.BlockedUsers;
                        break;
                    case ConsoleKey.D8:
                        return;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        continue;
                }
                DisplayOptionsFor(CurrentConnectionType);
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);
                var respone = WantUserToContinue("connection types");
                if (respone == ConsoleKey.D1)
                    continue;
                else if (respone == ConsoleKey.D2)
                    return;
                else
                    Environment.Exit(0);
            }
        }
        private void DisplayOptionsFor(ConnectionType type)
        {
            var data = UserData.GetCurrentUsers(type);
            while (true)
            {
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Blue);
                Console.WriteLine("\nWhat do you want to do next? " +
                    "\n1.Search for an account " +
                    "\n2.Search for an account in a specific year " +
                    "\n3.See stats based on years" +
                    "\n4.Go back to connections type" +
                    "\nEsc. Exit Application");
                var action = Console.ReadKey(true).Key;
                switch (action)
                {
                    case ConsoleKey.D1:
                        while (true)
                        {
                            var name = ConsoleHelper.GetValue("\n1.Input an account name");
                            if (name == ExitKeyword) break;
                            bool accFound = false;
                            foreach (var year in data)
                            {
                                if (!accFound)
                                {
                                    accFound = FindAccountIn(data[year.Key], name);
                                }
                            }
                            if (accFound)
                            {
                                ConsoleHelper.WriteAndColorLine("Account Found!", ConsoleColor.Green);
                                break;
                            }
                            else
                            {
                                ConsoleHelper.WaitAndClearLines(4, 2000, "Ooops, that account does not exist, try again!");
                                continue;
                            }
                        }
                        break;
                    case ConsoleKey.D2:
                        var response = ConsoleHelper.GetChoice("\n2.Pick an year: ", data.Keys.ToArray());
                        while (true)
                        {
                            var name = ConsoleHelper.GetValue("Input an account name");
                            if (name == ExitKeyword) break;
                            if (FindAccountIn(data[response], name))
                            {
                                ConsoleHelper.WriteAndColorLine("Account Found!", ConsoleColor.Green);
                                break;
                            }
                            else
                            {
                                ConsoleHelper.WaitAndClearLines(3, 2000, "Ooops, that account does not exist, try again!");
                                continue;
                            }
                        }
                        break;
                    case ConsoleKey.D3:
                        var lessActiveYearD5 = StatsHelper.GetLessActiveYear<string>(data);
                        var mostActiveYearD5 = StatsHelper.GetMostActiveYear<string>(data);
                        Console.WriteLine("\n5.{0} was the year with the most activity with a value of {1} \n{2} was the year with the less activity with a value of {3}!", 
                            mostActiveYearD5.Item1, mostActiveYearD5.Item2, lessActiveYearD5.Item1, lessActiveYearD5.Item2);
                        break;
                    case ConsoleKey.D4:
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
            AdditionalInformation = $"You have {Data.followers.Count} folowers " +
                $"\nYou follow {Data.following.Count} people " +
                $"\nYou sent {Data.follow_requests_sent.Count} temporar follow requests and {Data.permanent_follow_requests.Count} permanent follow request " +
                $"\nYou are following {Data.following_hashtags.Count} hashtags " +
                $"\nYou dimissed {Data.dismissed_suggested_users.Count} sugested users " +
                $"\nYou blocked {Data.blocked_users.Count} users.";

            UserData = StoreData(Data);
        }
        private bool FindAccountIn(ICollection<string> list, string account)
        {
            foreach (var acc in list)
            {
                if (acc == account)
                {
                    return true;
                }
            }
            return false;
        }
        private YearBasedConnectionsData StoreData(ConnectionsJsonData currentData)
        {
            var structuredData = new YearBasedConnectionsData();
            structuredData.AddDictionary(currentData.followers, ConnectionType.Followers);
            structuredData.AddDictionary(currentData.following, ConnectionType.Following);
            structuredData.AddDictionary(currentData.follow_requests_sent, ConnectionType.FollowRequestsSent);
            structuredData.AddDictionary(currentData.following_hashtags, ConnectionType.FollowingHashtags);
            structuredData.AddDictionary(currentData.blocked_users, ConnectionType.BlockedUsers);
            structuredData.AddDictionary(currentData.permanent_follow_requests, ConnectionType.PermanetFollowRequests);
            structuredData.AddDictionary(currentData.dismissed_suggested_users, ConnectionType.DismissedSuggestedUsers);
            return structuredData;
        }
    }
}
