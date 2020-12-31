using System;
using System.Collections.Generic;
using System.IO;
using Instagram_Data_Statistics.Enums;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Linq;
using System.Threading;

namespace Instagram_Data_Statistics
{
    class Program
    {
        static string PathToJson = string.Empty;
        static LikesData MediaLikes = new LikesData();
        static LikesData CommentLikes = new LikesData();
        static LikesType LikesType;
        static LikesData CurrentLikes
        {
            get
            {
                if (LikesType == LikesType.Comment)
                    return CommentLikes;
                else if (LikesType == LikesType.Media)
                    return MediaLikes;
                else return null;
            }
        }
        static void Main(string[] args)
        {

            Console.WriteLine("Hello, first of all you need to put the full path of your likes.json, \n\nif you are on windows, the path should look like this: \nC:\\Users\\username\\Desktop\\likes.json \n\nif you are on linux the path should look like this: \nusername/Desktop/likes.json \n\nif your are on MacOS the path should look like this: \nUsers/username/Desktop/likes.json \n\nIt's not mandatory to put the file...");
            string likesText = string.Empty;
            while (true)
            {
                PathToJson = Console.ReadLine();
                if (!PathToJson.Contains("likes.json"))
                {
                    char lastC = PathToJson.Last();
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        if (lastC != '\\') PathToJson += '\\';
                    }
                    else
                    {
                        if (lastC != '/') PathToJson += '/';
                    }
                    PathToJson += "likes.json";
                }
                if (File.Exists(PathToJson))
                {
                    //read data from file
                    likesText = File.ReadAllText(PathToJson);
                    break;
                }
                else
                {
                    Console.WriteLine("The file don't exists, try again...");
                    Thread.Sleep(2000);
                    Console.SetCursorPosition(0, Console.CursorTop - 2);
                    Console.Write(new string(' ', Console.BufferWidth));
                    Console.Write(new string(' ', Console.BufferWidth));
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                }
            }
            Console.WriteLine("\nDone!");
            //convert data to object via jsonconverter
            InstagramData dataFromFile = JsonConvert.DeserializeObject<InstagramData>(likesText);

            //add values to dictionaries
            AddValueToDic(CommentLikes, dataFromFile.comment_likes);
            AddValueToDic(MediaLikes, dataFromFile.media_likes);

            //display to user after it is done
            Console.WriteLine();
            Console.WriteLine("Done, now to have {0} comment likes and {1} media likes", dataFromFile.comment_likes.Count, dataFromFile.media_likes.Count);
            Console.WriteLine();
            LikesType = ChangeLikesType();
            while (true)
            {
                ShowLikes();
                Console.WriteLine();
                while (true)
                {
                    Console.WriteLine("What do you want to do next? \n1.I want more \n2.Exit \nPress a number to get started!");
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.D1)
                    {
                        Console.Clear();
                        break;
                    }
                    else if (key.Key == ConsoleKey.D2)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Console.SetCursorPosition(0, Console.CursorTop - 4);
                    }
                }
            }
        }
        static void ShowLikes()
        {
            Console.WriteLine();
            Console.WriteLine("What do you want to do next?" +
                " \n1.Show all likes" +
                " \n2.Show just a number of likes" +
                " \n3.Show likes from an account" +
                " \n4.Show account, based on likes number" +
                " \n5.Show likes based on years \nEsc.To exit \nOther key to change likes type");
            var action = Console.ReadKey(true).Key;
            Console.WriteLine();
            switch (action)
            {
                case ConsoleKey.D1://show all likes
                    switch (GetChoice("How do you want to be? \n1.Ordered \n2.Descended ordered", new ConsoleKey[] { ConsoleKey.D1, ConsoleKey.D2 }))
                    {
                        case ConsoleKey.D1://ordered
                            ShowList(CurrentLikes.Account.OrderByDescending(l => l.Value).ToList());
                            break;
                        case ConsoleKey.D2://descended ordered
                            ShowList(CurrentLikes.Account.OrderBy(l => l.Value).ToList());
                            break;
                    }
                    break;
                case ConsoleKey.D2:
                    break;
                case ConsoleKey.D3:
                    break;
                case ConsoleKey.D4:
                    break;
                case ConsoleKey.D5:
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
                default:
                    Console.Clear();
                    LikesType = ChangeLikesType();
                    break;
            }
        }
        static void ShowList(List<KeyValuePair<string, int>> list)
        {
            foreach (var item in list)
            {
                if (item.Value == 1)
                    Console.WriteLine("You liked post from {0} one time", item.Key);
                else
                    Console.WriteLine("You liked post from {0} {1} times", item.Key, item.Value);
            }
        }
        static ConsoleKey GetChoice(string QAndA, ConsoleKey[] bounds)
        {
            while (true)
            {
                Console.WriteLine(QAndA);
                var keyFromUser = Console.ReadKey(true).Key;
                foreach (var key in bounds)
                {
                    if (key == keyFromUser) return key;
                }
                ClearLines(bounds.Length + 1);
            }
        }
        static void ClearLines(int lines = 1)
        {
            if (lines > 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop - lines);
                for (int i = 0; i < lines; i++)
                {
                    Console.Write(new string(' ', Console.BufferWidth));
                }
                Console.SetCursorPosition(0, Console.CursorTop - lines);
            }
        }
        static LikesType ChangeLikesType()
        {
            while (true)
            {
                Console.WriteLine("What likes you'd like to show? \n 1.Comment likes \n 2.Media likes \nPress a number to get started!");
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.D1)
                {
                    return LikesType.Comment;
                }
                else if (key.Key == ConsoleKey.D2)
                {
                    return LikesType.Media;
                }
                Console.SetCursorPosition(0, Console.CursorTop - 4);
            }
        }
        static void AddValueToDic(LikesData likes, List<List<string>> value)
        {
            foreach (var like in value)
            {
                //store accounts
                StoreKey(likes.Account, like[1]);
                //store media likes based on years
                StoreKey(likes.YearBased, like[0].Substring(0, 4), like[1]);
            }
        }
        static void StoreKey(Dictionary<string, Dictionary<string, int>> dictionary, string mainKey, string secondKey)
        {
            if (dictionary.ContainsKey(mainKey))
            {
                if (dictionary[mainKey].ContainsKey(secondKey))
                    dictionary[mainKey][secondKey]++;
                else
                    dictionary[mainKey].Add(secondKey, 1);
            }
            else
                dictionary.Add(mainKey, new Dictionary<string, int>() { { secondKey, 1 } });
        }
        static void StoreKey(Dictionary<string, int> dictionary, string key)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key]++;
            else
                dictionary.Add(key, 1);
        }
    }
}
