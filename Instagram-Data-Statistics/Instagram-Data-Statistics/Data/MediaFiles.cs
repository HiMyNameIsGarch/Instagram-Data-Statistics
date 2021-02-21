using Instagram_Data_Statistics.DataFromJson;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Instagram_Data_Statistics.Data
{
    public class MediaFiles : BaseJsonData<MediaJsonData>, IBaseData
    {
        public MediaFiles(string basePath) : base(basePath, "\\media.json")
        {
        }
        private string AdditionalInformation;
        private SortedDictionary<string, MediaJsonData> YearBasedData { get; set; }
        public void DisplayOptions()
        {
            while (true)
            {
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);
                ConsoleHelper.WriteAndColorLine(AdditionalInformation, ConsoleColor.Cyan);
                Console.WriteLine("What do you want to do next? " +
                    "\n1.Show media files based on year" +
                    "\n2.Show stories that include music" +
                    "\nEsc. Exit application");
                var action = Console.ReadKey(true).Key;
                switch (action)
                {
                    case ConsoleKey.D1:
                        var year = ConsoleHelper.GetChoice("\n1.Pick a year: ", YearBasedData.Keys.ToArray());
                        var yearData = YearBasedData[year];
                        Console.WriteLine($"\nIn {year} you posted: {yearData.stories.Count} stories | {yearData.photos.Count} photos | {yearData.profile.Count} profile photos | {yearData.videos.Count} videos");
                        var response = ConsoleHelper.GetChoice($"Choose what media type do you wanna see: " +
                            $" \n1.Stories \n2.Photos \n3.Profile photos \n4.Videos", new ConsoleKey[] {ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4 });
                        var maxNum = 0;
                        IEnumerable<MediaModel> list = null;
                        switch (response)
                        {
                            case ConsoleKey.D1://stories
                                if(yearData.stories.Count > 0)
                                {
                                    maxNum = ConsoleHelper.GetNum($"\nHow many stories do you want to see, max: {yearData.stories.Count}", yearData.stories.Count);
                                    list = yearData.stories;
                                }
                                break;
                            case ConsoleKey.D2://photos
                                if (yearData.photos.Count > 0)
                                {
                                    maxNum = ConsoleHelper.GetNum($"\nHow many photos do you want to see, max: {yearData.stories.Count}", yearData.stories.Count);
                                    list = yearData.photos;
                                }
                                break;
                            case ConsoleKey.D3://profile photos
                                if (yearData.profile.Count > 0)
                                {
                                    maxNum = ConsoleHelper.GetNum($"\nHow many profile photos do you want to see, max: {yearData.stories.Count}", yearData.stories.Count);
                                    list = yearData.profile;
                                }
                                break;
                            case ConsoleKey.D4://videos
                                if (yearData.videos.Count > 0)
                                {
                                    maxNum = ConsoleHelper.GetNum($"\nHow many videos do you want to see, max: {yearData.stories.Count}", yearData.stories.Count);
                                    list = yearData.videos;
                                }
                                break;
                        }
                        if(list != null)
                        {
                            foreach (var item in list.Take(maxNum))
                            {
                                Console.WriteLine("\nTitle: {0}", string.IsNullOrEmpty(item.caption) ? "none" : item.caption);
                                Console.WriteLine("It was taken at {0}", ConvertToDateTimeString(item.taken_at));
                                Console.WriteLine("And you can find it at \"{0}\"", item.path);
                            }
                        }
                        break;
                    case ConsoleKey.D2://Show stories that include music
                        var storiesWithMusic = Data.stories.Where(s => s.music_genres != null);
                        int maxStories = storiesWithMusic.Count();
                        var maxNumOfStories = ConsoleHelper.GetNum($"\nHow many stories that include music  do you want to see, max: {maxStories}", maxStories);
                        foreach (var story in storiesWithMusic.Take(maxNumOfStories))
                        {
                            Console.WriteLine("\nTitle: {0}", string.IsNullOrEmpty(story.caption) ? "none" : story.caption);
                            Console.WriteLine("It was taken at {0}", ConvertToDateTimeString(story.taken_at));
                            Console.WriteLine("{0}as music genres", story.ShowMusicGenres());
                            Console.WriteLine("And you can find it at \"{0}\"", story.path);
                        }
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        return;
                    default:
                        Console.Clear();
                        continue;
                }
                ConsoleHelper.WriteAndColorLine(Delimitator, ConsoleColor.Green);

                var respone = WantUserToContinue("media data type");
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
            int photos = Data.photos.Count();
            int stories = Data.stories.Count();
            int profile = Data.profile.Count();
            int videos = Data.videos.Count();
            int total = photos + stories + profile + videos;
            AdditionalInformation = $"\nOn your Instagram account you put: \n{photos} Photos \n{stories} Stories \n{profile} Profile Pictures \n{videos} Videos \nAnd a total of {total} Media Files\n";
            YearBasedData = StoreData(Data);
        }
        private SortedDictionary<string, MediaJsonData> StoreData(MediaJsonData data)
        {
            var baseDic = new SortedDictionary<string, MediaJsonData>();
            //add stories
            foreach (var story in data.stories)
            {
                var year = story.taken_at.Substring(0, 4);
                if (baseDic.ContainsKey(year))
                    baseDic[year].stories.Add(story);
                else
                    baseDic.Add(year, new MediaJsonData() { stories = new List<MediaModel>() { story } });
            }
            //add stories
            foreach (var photo in data.photos)
            {
                var year = photo.taken_at.Substring(0, 4);
                if (baseDic.ContainsKey(year))
                    baseDic[year].photos.Add(photo);
                else
                    baseDic.Add(year, new MediaJsonData() { photos = new List<MediaModel>() { photo } });
            }
            //add stories
            foreach (var profilePhoto in data.profile)
            {
                var year = profilePhoto.taken_at.Substring(0, 4);
                if (baseDic.ContainsKey(year))
                    baseDic[year].profile.Add(profilePhoto);
                else
                    baseDic.Add(year, new MediaJsonData() { profile = new List<MediaModel>() { profilePhoto } });
            }
            //add stories
            foreach (var video in data.videos)
            {
                var year = video.taken_at.Substring(0, 4);
                if (baseDic.ContainsKey(year))
                    baseDic[year].videos.Add(video);
                else
                    baseDic.Add(year, new MediaJsonData() { videos = new List<MediaModel>() { video } });
            }
            return baseDic;
        }
    }
}
