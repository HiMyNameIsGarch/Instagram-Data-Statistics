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
        }
    }
}
