﻿using Instagram_Data_Statistics.DataFromJson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Instagram_Data_Statistics.Data
{
    public class MediaComments : BaseJsonData<CommentsJsonData>, IBaseData
    {
        public MediaComments(string basePath) : base(basePath, "comments")
        {
        }
        private int TotalComments;
        private Tuple<string, string> LongestComment;
        private Tuple<string, string> ShortestComment;
        private Dictionary<string, Dictionary<string, ICollection<string>>> YearBasedComments { get; set; }
        private Dictionary<string, ICollection<string>> AllTimeComments { get; set; }
        public void DisplayOptions()
        {
            while (true)
            {
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);
                ConsoleHelper.WriteAndColorLine(AdditionalInformation, ConsoleColor.Cyan);
                Console.WriteLine($"\nWhat do you want to do next? " +
                    "\n1.Show longest and shortest comment" +
                    "\n2.Show top accounts with most comments" +
                    "\n3.Show top accounts with most comments in a specific year" +
                    "\n4.Show comments based on an account" +
                    "\n5.Show comments based on an account in a specific year" +
                    "\n6.Go to main menu" +
                    "\nEsc. Exit application");
                var action = Console.ReadKey(true).Key;
                switch (action)
                {
                    case ConsoleKey.D1://Show both longest / shortest comment
                        Console.WriteLine($"\n1. Your longest comments is \n\"{LongestComment.Item2}\" \nOn account: \"{LongestComment.Item1}\" " +
                            $"\nYour shortest comments is \n\"{ShortestComment.Item2}\" \nOn account: \"{ShortestComment.Item1}\"");
                        break;
                    case ConsoleKey.D2://Show top accounts with most comments
                        var maxNum = ConsoleHelper.GetNum($"\n2.How many accounts do you want so see? max: {AllTimeComments.Count}", AllTimeComments.Count);
                        var commentsD4 = OrderAllTimeComments(maxNum);
                        ConsoleHelper.WriteAndColorLine($"\nTop {maxNum} accounts with most comments\n", ConsoleColor.Cyan);
                        PrintComments(commentsD4);
                        break;
                    case ConsoleKey.D3://Show top accounts with most comments in a specific year
                        string currentYear = ConsoleHelper.GetChoice("\n3.Pick a year: ", YearBasedComments.Keys.ToArray());
                        var currentYearValue = YearBasedComments[currentYear];
                        int maxNumD5 = ConsoleHelper.GetNum($"\nHow many accounts do you want so see? max: {currentYearValue.Count}", currentYearValue.Count);
                        var commentsD5 = OrderAllTimeComments(maxNumD5);
                        ConsoleHelper.WriteAndColorLine($"\nTop {maxNumD5} accounts with most comments in {currentYear}\n", ConsoleColor.Cyan);
                        PrintComments(commentsD5);
                        break;
                    case ConsoleKey.D4://Show all comments based on an account
                        while (true)
                        {
                            var name = ConsoleHelper.GetValueWithColor("\n4.Input your account name: ", ConsoleColor.Cyan);
                            if (name == ExitKeyword) break;
                            if (AllTimeComments.ContainsKey(name))
                            {
                                var comments = AllTimeComments[name];
                                Console.WriteLine("\nYour comments from {0} are:", name);
                                ConsoleHelper.ShowList(comments);
                                break;
                            }
                            ConsoleHelper.WaitAndClearLines(4, 2000, "Ooops, that account doesn't exist!");
                            continue;
                        }
                        break;
                    case ConsoleKey.D5://Show comments based on an account in a specific year
                        var response = ConsoleHelper.GetChoice("\n5.Pick a year: ", YearBasedComments.Keys.ToArray());
                        while (true)
                        {
                            var name = ConsoleHelper.GetValueWithColor("Input your account name: ", ConsoleColor.Cyan);
                            if (name == ExitKeyword) break;
                            var currentYearD7 = YearBasedComments[response];
                            if (currentYearD7.ContainsKey(name))
                            {
                                var comments = currentYearD7[name];
                                Console.WriteLine("\nYour comments from {0} are:", name);
                                ConsoleHelper.ShowList(comments);
                                break;
                            }
                            ConsoleHelper.WaitAndClearLines(3, 2000, "Ooops, that account doesn't exist!");
                            continue;
                        }
                        break;
                    case ConsoleKey.D6:
                        return;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        return;
                    default:
                        Console.Clear();
                        continue;
                }
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);
                var respone = WantUserToContinue("media comments");
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
            var organizedData = StoreData(Data);
            YearBasedComments = organizedData.Item2;
            AllTimeComments = organizedData.Item1;
            AdditionalInformation = $"\nYou have a total of {TotalComments} comments";
        }
        private void PrintComments(IEnumerable<Tuple<string, IEnumerable<string>>> comments)
        {
            foreach (var comment in comments)
            {
                Console.WriteLine("You commented on {0} posts, {1} times", comment.Item1, comment.Item2.Count());
            }
        }
        private IEnumerable<Tuple<string, IEnumerable<string>>> OrderAllTimeComments(int maxNum)
        {
            return AllTimeComments.Select(c => new Tuple<string, IEnumerable<string>>(c.Key, c.Value)).OrderByDescending(s => s.Item2.Count()).Take(maxNum).ToList();
        }
        private Tuple<Dictionary<string, ICollection<string>>, Dictionary<string, Dictionary<string, ICollection<string>>>> StoreData(CommentsJsonData baseData)
        {
            TotalComments = baseData.media_comments.Count;
            var globalDic = new Dictionary<string, ICollection<string>>();
            var yearBasedDic = new Dictionary<string, Dictionary<string, ICollection<string>>>();
            string longestComment = string.Empty;
            string longestCommentAccount = string.Empty;
            string shortestComment = baseData.media_comments[0][1];
            string shortestCommentAccount = string.Empty;
            foreach (var mediaComment in baseData.media_comments)
            {
                //take current values
                var year = mediaComment[0].Substring(0,4);
                var comment = mediaComment[1];
                var account = mediaComment[2];

                //set longest and shortest comments
                if(longestComment.Length < comment.Length)
                {
                    longestComment = comment;
                    longestCommentAccount = account;
                }
                if (shortestComment.Length > comment.Length)
                {
                    shortestComment = comment;
                    shortestCommentAccount = account;
                }

                //add values to dictionaries
                if (yearBasedDic.ContainsKey(year))
                {
                    var currentValue = yearBasedDic[year];
                    if (currentValue.ContainsKey(account))
                        currentValue[account].Add(comment);
                    else
                        currentValue.Add(account, new List<string>() { comment });
                }
                else
                    yearBasedDic.Add(year, new Dictionary<string, ICollection<string>>() {{account, new List<string>() { comment } } });
                
                if (globalDic.ContainsKey(account))
                    globalDic[account].Add(comment);
                else
                    globalDic.Add(account, new List<string>() { comment});

            }
            LongestComment = new Tuple<string, string>(longestCommentAccount, longestComment);
            ShortestComment = new Tuple<string, string>(shortestCommentAccount, shortestComment);
            return new Tuple<Dictionary<string, ICollection<string>>, Dictionary<string, Dictionary<string, ICollection<string>>>>(globalDic, yearBasedDic);
        }
    }
}
