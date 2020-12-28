using System;
using System.Collections.Generic;
using System.IO;
using Instagram_Data_Statistics.Enums;
using Newtonsoft.Json;

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

            Console.WriteLine("Hello, first of all you need to put the full path of your likes.json, it should look like that: \nC:\\Users\\username\\Desktop\\likes.json \nIt doesn't matter if you put the file or not!");
            //PathToJson = GetValue("");

            //read data from file
            string likesText = File.ReadAllText(PathToJson);

            //convert data to object via jsonconverter
            InstagramData dataFromFile = JsonConvert.DeserializeObject<InstagramData>(likesText);
        }
    }
}
