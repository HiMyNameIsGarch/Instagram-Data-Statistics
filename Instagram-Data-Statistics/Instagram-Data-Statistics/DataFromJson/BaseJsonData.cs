using Instagram_Data_Statistics.Interfaces;
using Newtonsoft.Json;
using System.IO;

namespace Instagram_Data_Statistics.DataFromJson
{
    public class BaseJsonData<K> : IBaseJsonData<K>
    {
        public BaseJsonData(string basePath, string fileName)
        {
            PathToJson = basePath + fileName;
            ReadText();
        }
        public string PathToJson { get; protected set; }
        public string DataFromFile { get; protected set; }
        public K Data { get; private set; }
        public void DeserializeJson()
        {
            var dataFromFile = JsonConvert.DeserializeObject<K>(DataFromFile);
            Data = dataFromFile;
        }
        public void ReadText()
        {
            if (File.Exists(PathToJson))
            {
                //read data from file
                DataFromFile = File.ReadAllText(PathToJson);
                DeserializeJson();
            }
        }
    }
}
