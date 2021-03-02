using Instagram_Data_Statistics.Interfaces;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Instagram_Data_Statistics.DataFromJson
{
    public class BaseJsonData<K> : IBaseJsonData<K>
    {
        public BaseJsonData(string basePath, string fileName)
        {
            //make sure that the path to file is correct
            if(basePath.Last() != '\\')
            {
                basePath += "\\";
            }
            if (fileName.StartsWith("\\"))
            {
                fileName = fileName.Remove(0, 1);
            }
            if (!fileName.Contains(FileExtension))
            {
                fileName += FileExtension;
            }
            PathToJson = basePath + fileName;
            ReadText();
        }
        private const string FileExtension = ".json";
        public const string Delimitator = "\n------------------------------------------------------------";
        public const string ExitKeyword = "EXIT";
        public string PathToJson { get; protected set; }
        public string DataFromFile { get; protected set; }
        public K Data { get; private set; }
        public void DeserializeJson()
        {
            K dataFromFile = default(K);
            try
            {
                dataFromFile = JsonConvert.DeserializeObject<K>(DataFromFile);
            }
            catch
            {
                ConsoleHelper.WriteAndColorLine("We could not deserialize the object, that can be caused by modifying it or Instagram changing its structure (if that please leave an issue on Github!) \nPress any key to exit!", ConsoleColor.Red);
                Console.ReadKey();
                Environment.Exit(0);
            }
            Data = dataFromFile;
        }
        public void ReadText()
        {
            if (File.Exists(PathToJson))
            {
                //read data from file
                DataFromFile = File.ReadAllText(PathToJson);
                DeserializeJson();
                return;
            }
            ConsoleHelper.WriteAndColorLine("\nThe file does not exist double check for the path to it! \nPress any key to exit!", ConsoleColor.Red);
            Console.ReadKey();
            Environment.Exit(0);
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
