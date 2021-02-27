using System;
using System.Collections.Generic;
using System.Linq;

namespace Instagram_Data_Statistics
{
    public static class StatsHelper
    {
        public static Tuple<string, int> GetMostActiveYear<T>(Dictionary<string, ICollection<T>> value)
        {
            KeyValuePair<string, ICollection<T>> currentValue = value.First();
            if(value.Count == 1)
                return new Tuple<string, int>(currentValue.Key, currentValue.Value.Count);
            foreach (var year in value)
            {
                if (year.Value.Count > currentValue.Value.Count)
                {
                    currentValue = year;
                }
            }
            return new Tuple<string, int>(currentValue.Key, currentValue.Value.Count);
        }
        public static Tuple<string, int> GetLessActiveYear<T>(Dictionary<string, ICollection<T>> value)
        {
            KeyValuePair<string, ICollection<T>> currentValue = value.First();
            if (value.Count == 1)
                return new Tuple<string, int>(currentValue.Key, currentValue.Value.Count);
            foreach (var year in value)
            {
                if (year.Value.Count < currentValue.Value.Count)
                {
                    currentValue = year;
                }
            }
            return new Tuple<string, int>(currentValue.Key, currentValue.Value.Count);
        }
        public static Tuple<string, int> GetLessActiveYear<T,V>(Dictionary<string, Dictionary<T,V>> value)
        {
            KeyValuePair<string, Dictionary<T,V>> currentValue = value.First();
            if (value.Count == 1)
                return new Tuple<string, int>(currentValue.Key, currentValue.Value.Count);
            foreach (var year in value)
            {
                if (year.Value.Count < currentValue.Value.Count)
                {
                    currentValue = year;
                }
            }
            return new Tuple<string, int>(currentValue.Key, currentValue.Value.Count);
        }
        public static Tuple<string, int> GetMostActiveYear<T,V>(Dictionary<string, Dictionary<T,V>> value)
        {
            KeyValuePair<string, Dictionary<T,V>> currentValue = value.First();
            if (value.Count == 1)
                return new Tuple<string, int>(currentValue.Key, currentValue.Value.Count);
            foreach (var year in value)
            {
                if (year.Value.Count > currentValue.Value.Count)
                {
                    currentValue = year;
                }
            }
            return new Tuple<string, int>(currentValue.Key, currentValue.Value.Count);
        }
    }
}
