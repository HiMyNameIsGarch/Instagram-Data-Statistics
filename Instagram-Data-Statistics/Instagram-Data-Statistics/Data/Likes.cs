using Instagram_Data_Statistics.DataFromJson;
using Instagram_Data_Statistics.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Instagram_Data_Statistics.Data
{
    public class Likes : BaseJsonData<LikesJsonData> , IBaseData
    {
        public Likes(string basePath) : base(basePath, "likes")
        {
        }
        //props
        protected Tuple<Dictionary<string,int>, Dictionary<string, Dictionary<string, int>>> CurrentLikes
        { 
            get
            {
                return LikesType == LikesType.Comment ? CommentLikes : MediaLikes;
            }
        }
        private Tuple<Dictionary<string, int>, Dictionary<string, Dictionary<string, int>>> MediaLikes { get; set; }
        private Tuple<Dictionary<string, int>, Dictionary<string, Dictionary<string, int>>> CommentLikes { get; set; }
        private LikesType LikesType = LikesType.Media;
        //methods
        public void DisplayOptions()
        {
            while (true)
            {
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);
                ConsoleHelper.WriteAndColorLine(AdditionalInformation, ConsoleColor.Cyan);
                Console.WriteLine("\nWhat do you want to do next?" +
                    " \n1.Show top accounts of all time" +
                    " \n2.Show top accounts per year" +
                    " \n3.Show top accounts based on year" +
                    " \n4.Show how many posts/comments you liked from a specific account" +
                    " \n5.Show how many posts/comments you liked from a specific account based on year" +
                    " \n6.Show media likes based on years"+
                    " \n7.Go to main menu"+
                    " \nEsc.To exit " +
                    $"\nPress another key to change likes type! Current likes type: {LikesType}");
                var action = Console.ReadKey(true).Key;
                switch (action)
                {
                    case ConsoleKey.D1://Show top accounts of all time
                        int maxValue = CurrentLikes.Item1.Count;
                        var number = ConsoleHelper.GetNum($"\n1.How many accounts do you want to see, max: {maxValue}", maxValue);
                        ConsoleHelper.ShowList(CurrentLikes.Item1, number);
                        break;
                    case ConsoleKey.D2://Show top accounts per year
                        var numberYear = ConsoleHelper.GetNum("\n2.How many accounts do you want to see:");
                        foreach (var year in CurrentLikes.Item2)
                        {
                            ConsoleHelper.WriteAndColorLine($"\nTop {numberYear} accounts in {year.Key}: ", ConsoleColor.Cyan);
                            ConsoleHelper.ShowList(year.Value, numberYear);
                        }
                        break;
                    case ConsoleKey.D3://Show top accounts based on year
                        var response = ConsoleHelper.GetChoice("\n3.Pick a year: ", CurrentLikes.Item2.Keys.ToArray());
                        var currentYear3 = CurrentLikes.Item2[response];
                        var accNum = ConsoleHelper.GetNum($"\nHow many accounts do you want to see, max: {currentYear3.Count}", currentYear3.Count);
                        ConsoleHelper.WriteAndColorLine($"In {response}: ", ConsoleColor.Cyan);
                        ConsoleHelper.ShowList(currentYear3, accNum);
                        break;
                    case ConsoleKey.D4://Show how many posts/comments you liked from a specific account
                        while (true)
                        {
                            var name = ConsoleHelper.GetValueWithColor("\n4.Input your account name: ", ConsoleColor.Cyan);
                            if (name == ExitKeyword) break;
                            if (CurrentLikes.Item1.ContainsKey(name)) 
                            {
                                var likesNum = CurrentLikes.Item1[name];
                                if (likesNum == 1)
                                    Console.WriteLine("You liked one post from {0} ", name);
                                else
                                    Console.WriteLine("You liked {1} posts from {0}", name, likesNum);
                                break;
                            }
                            ConsoleHelper.WaitAndClearLines(4, 2000, "Ooops, that account doesn't exist!");
                            continue;
                        }
                        break;
                    case ConsoleKey.D5://Show how many posts/comments you liked from a specific account based on year
                        var responseD5 = ConsoleHelper.GetChoice("\n5.Pick an year: ", CurrentLikes.Item2.Keys.ToArray());
                        while (true)
                        {
                            var nameD5 = ConsoleHelper.GetValueWithColor("Input your account name: ", ConsoleColor.Cyan);
                            if (nameD5 == ExitKeyword) break;
                            if (CurrentLikes.Item2[responseD5].ContainsKey(nameD5))
                            {
                                var likesNum = CurrentLikes.Item2[responseD5][nameD5];
                                if (likesNum == 1)
                                    Console.WriteLine("In {1} you liked one post from {0} ", nameD5, responseD5);
                                else
                                    Console.WriteLine("In {2} you liked {1} posts from {0}", nameD5, likesNum, responseD5);
                                break;
                            }
                            ConsoleHelper.WaitAndClearLines(4, 2000, "Ooops, that account doesn't exist!");
                            continue;
                        }
                        break;
                    case ConsoleKey.D6://Show media likes based on year
                        int sum = 0;
                        foreach (var year in CurrentLikes.Item2)
                        {
                            sum += CurrentLikes.Item2[year.Key].Count;
                        }
                        float mediaLikes = sum / CurrentLikes.Item2.Count;
                        ConsoleHelper.WriteAndColorLine($"\n6.You liked near {mediaLikes} posts in a single year!", ConsoleColor.Blue);
                        break;
                    case ConsoleKey.D7:
                        return;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        return;
                    default:
                        Console.Clear();
                        LikesType = LikesType == LikesType.Media ? LikesType.Comment : LikesType.Media;
                        continue;
                }
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);
                var respone = WantUserToContinue("likes data type");
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
            AdditionalInformation = $"\nYou liked {Data.media_likes.Count} posts and {Data.comment_likes.Count} comments a total of {Data.media_likes.Count + Data.comment_likes.Count} likes!";
            if(MediaLikes is null)
                MediaLikes = StoreData(Data.media_likes);
            if(CommentLikes is null)
                CommentLikes = StoreData(Data.comment_likes);
        }
        private Tuple<Dictionary<string, int>, Dictionary<string, Dictionary<string, int>>> StoreData(List<List<string>> data)
        {
            var allLikesDic = new Dictionary<string,int>();
            var yearLikesDic = new Dictionary<string, Dictionary<string, int>>();
            var baseData = DictionaryHelper.GetMultipleDic(data);
            yearLikesDic = baseData.Item2;
            allLikesDic = baseData.Item1;
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
