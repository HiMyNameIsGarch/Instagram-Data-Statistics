using Instagram_Data_Statistics.DataFromJson;
using Instagram_Data_Statistics.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Instagram_Data_Statistics.Data
{
    public class Likes : BaseJsonData<LikesJsonData> , IBaseData
    {
        public Likes(string basePath) : base(basePath, "\\likes.json")
        {
        }
        //props
        public Tuple<Dictionary<string,int> ,Dictionary<string, Dictionary<string, int>>> CurrentValue
        { 
            get
            {
                return CurrentLikesType == LikesType.Comment ? CommentLikes : MediaLikes;
            }
        }
        public Tuple<Dictionary<string, int>, Dictionary<string, Dictionary<string, int>>> MediaLikes { get; set; }
        public Tuple<Dictionary<string, int>, Dictionary<string, Dictionary<string, int>>> CommentLikes { get; set; }

        private LikesType CurrentLikesType = LikesType.Media;
        //methods
        public void DisplayOptions()
        {
            while (true)
            {
                Console.WriteLine("\nWhat do you want to do next?" +
                    " \n1.Show top accounts of all time" +
                    " \n2.Show top accounts per year" +
                    " \n3.Show top accounts based on year" +
                    " \n4.Show how many posts/comments you liked from a specific account" +
                    " \n5.Show how many posts/comments you liked from a specific account based on year" +
                    " \n6.Show media likes based on year"+
                    " \nEsc.To exit " +
                    $"\nPress another key to change likes type! Current likes type: {CurrentLikesType}");
                var action = Console.ReadKey(true).Key;
                switch (action)
                {
                    case ConsoleKey.D1://Show top accounts of all time
                        int maxValue = CurrentValue.Item1.Count();
                        var number = ConsoleHelper.GetNum($"\nHow many account do you want to see, max: {maxValue}", maxValue);
                        ConsoleHelper.ShowList(CurrentValue.Item1, number);
                        break;
                    case ConsoleKey.D2://Show top accounts per year
                        var numberYear = ConsoleHelper.GetNum("\nHow many account do you want to see:");
                        foreach (var year in CurrentValue.Item2)
                        {
                            Console.WriteLine("\nTop accounts in {0}:",year.Key);
                            ConsoleHelper.ShowList(year.Value, numberYear);
                        }
                        break;
                    case ConsoleKey.D3://Show top accounts based on year
                        var response = ConsoleHelper.GetChoice("Pick an year: ", CurrentValue.Item2.Keys.ToArray());
                        var currentYear3 = CurrentValue.Item2[response].Count;
                        var accNum = ConsoleHelper.GetNum($"\nHow many account do you want to see, max: {currentYear3}", currentYear3);
                        Console.WriteLine("In {0}: ", response);
                        ConsoleHelper.ShowList(CurrentValue.Item2[response], accNum);
                        break;
                    case ConsoleKey.D4://Show how many posts/comments you liked from a specific account
                        var name = ConsoleHelper.GetValue("Input your account name: ");
                        if (CurrentValue.Item1.ContainsKey(name)) 
                        {
                            var likesNum = CurrentValue.Item1[name];
                            if (likesNum == 1)
                                Console.WriteLine("You liked one post from {0} ", name);
                            else
                                Console.WriteLine("You liked {1} posts from {0}", name, likesNum);
                        }
                        break;
                    case ConsoleKey.D5://Show how many posts/comments you liked from a specific account based on year
                        var responseD5 = ConsoleHelper.GetChoice("Pick an year: ", CurrentValue.Item2.Keys.ToArray());
                        var currentYearD3 = CurrentValue.Item2[responseD5].Count;
                        while (true)
                        {
                            var nameD5 = ConsoleHelper.GetValue("Input your account name: ");
                            if (CurrentValue.Item2[responseD5].ContainsKey(nameD5))
                            {
                                var likesNum = CurrentValue.Item2[responseD5][nameD5];
                                if (likesNum == 1)
                                    Console.WriteLine("In {1} you liked one post from {0} ", nameD5, responseD5);
                                else
                                    Console.WriteLine("In {2} you liked {1} posts from {0}", nameD5, likesNum, responseD5);
                                break;
                            }
                            ConsoleHelper.WaitAndClearLines(3, 2000, "Ooops, that account doesn't exist!");
                            continue;
                        }
                        break;
                    case ConsoleKey.D6:
                        foreach (var year in CurrentValue.Item2)
                        {
                            int totalLikes = 0;
                            var currentYear = CurrentValue.Item2[year.Key];
                            foreach (var account in currentYear)
                            {
                                totalLikes += account.Value;
                            }
                            float mediaLikes = totalLikes / currentYear.Count();
                            Console.WriteLine("In {0} your media likes was: {1}", year.Key, mediaLikes);
                        }
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        return;
                    default:
                        Console.Clear();
                        CurrentLikesType = CurrentLikesType == LikesType.Media ? LikesType.Comment : LikesType.Media;
                        continue;
                }
                var respone = ConsoleHelper.GetChoice("\nDone, Choose what you wanna do next: \n1.I want more on likes data type \n2.Take me back to main menu \nEsc. Exit application",
                           new ConsoleKey[] { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.Escape });
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
            MediaLikes = StoreData(Data.media_likes);
            CommentLikes = StoreData(Data.comment_likes);
        }
        private Tuple<Dictionary<string, int>, Dictionary<string, Dictionary<string, int>>> StoreData(List<List<string>> data)
        {
            var allLikesDic = new Dictionary<string,int>();
            var yearLikesDic = new Dictionary<string, Dictionary<string, int>>();
            foreach (var like  in data)
            {
                var mainKey = like[0].Substring(0, 4);
                var secondKey = like[1];
                if (yearLikesDic.ContainsKey(mainKey))
                {
                    if (yearLikesDic[mainKey].ContainsKey(secondKey))
                        yearLikesDic[mainKey][secondKey]++;
                    else
                        yearLikesDic[mainKey].Add(secondKey, 1);
                }
                else
                    yearLikesDic.Add(mainKey, new Dictionary<string, int>() { { secondKey, 1 } });

                if (allLikesDic.ContainsKey(secondKey))
                    allLikesDic[secondKey]++;
                else
                    allLikesDic.Add(secondKey, 1);
            }
            //order dictionaries
            allLikesDic = allLikesDic.OrderByDescending(s => s.Value).ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
            foreach (var year in yearLikesDic.ToList())
            {
                var currentYear = yearLikesDic[year.Key];
                yearLikesDic[year.Key] = currentYear.OrderByDescending(s => s.Value).ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);
            }
            return new Tuple<Dictionary<string, int>, Dictionary<string, Dictionary<string, int>>>(allLikesDic, yearLikesDic);
        }
    }
}
