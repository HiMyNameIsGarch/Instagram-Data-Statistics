using Instagram_Data_Statistics.Interfaces;
using Newtonsoft.Json;
using System;
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
        public const string Delimitator = "\n------------------------------------------------------------";
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
        protected ConsoleKey WantUserToContinue(string optionName)
        {
           return ConsoleHelper.GetChoice($"\nDone, Choose what you wanna do next: \n1.I want more on {optionName} \n2.Take me back to main menu \nEsc. Exit application",
                           new ConsoleKey[] { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.Escape });
        }
        public string ConvertToDateTimeString(string dateTimeString)
        {
            var isSuccess = DateTime.TryParse(dateTimeString, out DateTime dateTime);
            if (isSuccess)
            {
                return dateTime.ToLongDateString();
            }
            return DateTime.Now.ToLongDateString();
        }
    }
}
