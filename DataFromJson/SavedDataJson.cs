using System.Collections.Generic;
using System.Linq;

namespace Instagram_Data_Statistics.DataFromJson
{
    public class SavedDataJson
    {
        public List<CollectionMedia> saved_collections { get; set; }
        public IEnumerable<List<string>> saved_media { get; set; }
    }
    public class YearBasedSavedData
    {
        public Dictionary<string, Dictionary<string, int>> GetCollection(int collectionIndex)
        {
            if(collectionIndex < 0 || collectionIndex > Collections.Count) return null;
            return YearBasedCollections[collectionIndex - 1];
        }
        public Dictionary<string,int> GetYearBasedColl(int collectionIndex)
        {
            if (collectionIndex < 0 || collectionIndex > YearBasedCollections.Count) return null;
            return Collections[collectionIndex - 1];
        }
        public void StoreData(SavedDataJson data)
        {
            if (data is null) return;
            if(data.saved_collections.Count > 0)
            {
                foreach (var savedMedia in data.saved_collections)
                {
                    var newData = DictionaryHelper.GetMultipleDic(savedMedia.media);
                    Collections.Add(newData.Item1);
                    YearBasedCollections.Add(newData.Item2);
                }
            }
            var mediaData = DictionaryHelper.GetMultipleDic(data.saved_media);
            Media = mediaData.Item1;
            YearBasedMedia = mediaData.Item2;
        }
        public List<Dictionary<string, Dictionary<string, int>>> YearBasedCollections { get; set; } = new List<Dictionary<string, Dictionary<string, int>>>();
        public List<Dictionary<string, int>> Collections { get; set; } = new List<Dictionary<string, int>>();
        public Dictionary<string, Dictionary<string, int>> YearBasedMedia { get; set; } = new Dictionary<string, Dictionary<string, int>>();
        public Dictionary<string, int> Media { get; set; } = new Dictionary<string, int>();
    }
    public class CollectionMedia
    {
        public string name { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public IEnumerable<List<string>> media { get; set; }
    }
}
