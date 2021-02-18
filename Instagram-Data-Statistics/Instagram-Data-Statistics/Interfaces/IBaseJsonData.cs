namespace Instagram_Data_Statistics.Interfaces
{
    public interface IBaseJsonData<K>
    {
        string PathToJson { get; }
        string DataFromFile { get; }
        K Data { get; }
        void ReadText();
        void DeserializeJson();
    }
}
