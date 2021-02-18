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
            //Account = new Dictionary<string, int>();//store all accounts and likes no matter year
            //YearBased = new Dictionary<string, Dictionary<string, int>>();//store accounts likes based on year
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
        //D:\Data about social accounts\instagram
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
                    case ConsoleKey.D1:
                        int maxValue = CurrentValue.Item1.Count();
                        var number = ConsoleHelper.GetNum($"\nHow many account do you want to see, max: {maxValue}", maxValue);
                        ConsoleHelper.ShowList(CurrentValue.Item1.OrderByDescending(s => s.Value).Take(number).ToList());
                        var respone =  ConsoleHelper.GetChoice("\nDone, Choose what you wanna do next: \n1.I want more on likes data type \n2.Take me back to main menu \nEsc. Exit appliction", 
                                                   new ConsoleKey[] { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.Escape });
                        if(respone == ConsoleKey.D1)
                            break;
                        else if(respone == ConsoleKey.D2)
                            return;
                        else
                            Environment.Exit(0);
                        break;
                    case ConsoleKey.D2:
                        return;
                    case ConsoleKey.D3:
                        return;
                    case ConsoleKey.D4:
                        return;
                    case ConsoleKey.D5:
                        return;
                    case ConsoleKey.D6:
                        return;
                    case ConsoleKey.Escape:
                        return;
                    default:
                        Console.Clear();
                        CurrentLikesType = CurrentLikesType == LikesType.Media ? LikesType.Comment : LikesType.Media;
                        break;
                }
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
            return new Tuple<Dictionary<string, int>, Dictionary<string, Dictionary<string, int>>>(allLikesDic, yearLikesDic);
        }
    }
}
