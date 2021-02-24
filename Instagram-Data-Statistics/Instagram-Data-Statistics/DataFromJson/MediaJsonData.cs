using Instagram_Data_Statistics.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Instagram_Data_Statistics.DataFromJson
{
    public class MediaJsonData
    {
        public MediaJsonData()
        {
        }
        public MediaJsonData(MediaModel item, MediaType type)
        {
            Add(item,type);
        }
        public void Add(MediaModel item, MediaType type)
        {
            switch (type)
            {
                case MediaType.Photos:
                    photos.Add(item);
                    break;
                case MediaType.Stories:
                    stories.Add(item);
                    break;
                case MediaType.Profile:
                    profile.Add(item);
                    break;
                case MediaType.Videos:
                    videos.Add(item);
                    break;
            }
        }
        public ICollection<MediaModel> photos { get; set; } = new List<MediaModel>();
        public ICollection<MediaModel> stories { get; set; } = new List<MediaModel>();
        public ICollection<MediaModel> profile { get; set; } = new List<MediaModel>();
        public ICollection<MediaModel> videos { get; set; } = new List<MediaModel>();
    }
    public class MediaModel
    {
        public string caption { get; set; }
        public IEnumerable<string> music_genres { get; set; }
        public string taken_at { get; set; }
        public string path { get; set; }
        public string ShowMusicGenres()
        {
            if(music_genres != null && music_genres?.Count() > 0)
            {
                string musicGenres = string.Empty;
                foreach (var music in music_genres)
                {
                    musicGenres += music + ", ";
                }
                return musicGenres;
            }
            return string.Empty;
        }
    }
}
