using System;
using System.IO;
using Instagram_Data_Statistics.Data;
using Newtonsoft.Json;

namespace Instagram_Data_Statistics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, first of all you need to put the full path of your folder with your Instagram data, it shoud look like this:\nC:\\Users\\your username\\Desktop\\InstagramData");

            string basePath = @"C:\Users\gabri\Desktop\Darta";//Console.ReadLine();

            Console.WriteLine("\nGood!\nNow choose what type of data do you wanna see.\n");
            Console.WriteLine("1.Likes (posts and comments that you liked)\n" +
                "2.Account history (login history and registration info)\n" +
                "3.Comments (all comments that you typed)\n" +
                "4.Media (includes all the files that you sent (stories, profile pictures, photos, videos)\n" +
                "5.Connections (all blocked users, follow request send, permanent follow request, followers, following, hastags, dimissed suggested user)\n" +
                "6.Messages\n" +
                "7.Saved (collections and media)\n" +
                "8.Searched content (all accounts that you searched)\n" +
                "9.Seen content (all posts that you saw)\n" +
                "0.Stories (polls, emoji sliders, questions, countdowns, quizzes)\n" +
                "Esc. To exit!");
            var response = Console.ReadKey(true).Key;
            IBaseData baseData = null;
            switch (response)
            {
                case ConsoleKey.D1:
                    baseData = new Likes(basePath);
                    break;
                case ConsoleKey.D2:
                    baseData = new AccountHistory(basePath);
                    break;
                case ConsoleKey.D3:
                    baseData = new MediaComments(basePath);
                    break;
                case ConsoleKey.D4:
                    baseData = new MediaFiles(basePath);
                    break;
                case ConsoleKey.D5:
                    baseData = new Connections(basePath);
                    break;
                case ConsoleKey.D6:
                    break;
                case ConsoleKey.D7:
                    break;
                case ConsoleKey.D8:
                    break;
                case ConsoleKey.D9:
                    break;
                case ConsoleKey.D0:
                    baseData = new Stories(basePath);
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }

            baseData.OrganizeDataFromObject();
            baseData.DisplayOptions();

            Console.WriteLine("\nWhere back baby!");
        }
    }
}
