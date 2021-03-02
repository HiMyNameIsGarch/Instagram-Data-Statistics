using System;
using System.Collections.Generic;
using System.Linq;

namespace Instagram_Data_Statistics
{
    public static class DictionaryHelper
    {
        public static Dictionary<string, Dictionary<string, int>> GetDictionaryFrom(IEnumerable<List<string>> list)
        {
            var dataDic = new Dictionary<string, Dictionary<string, int>>();
            if (list.Count() < 1) return dataDic;
            foreach (var item in list)
            {
                var mainKey = item[0].Substring(0, 4);
                var secondKey = item[1];
                if (dataDic.ContainsKey(mainKey))
                {
                    var currentYear = dataDic[mainKey];
                    if (currentYear.ContainsKey(secondKey))
                        currentYear[secondKey]++;
                    else
                        currentYear.Add(secondKey, 1);
                }
                else
                    dataDic.Add(mainKey, new Dictionary<string, int>() { { secondKey, 1 } });
            }
            return dataDic;
        }
        public static Dictionary<string, int> GetDictionary(IEnumerable<List<string>> list)
        {
            if (list.Count() < 1) return null;
            var dataDic = new Dictionary<string, int>();
            foreach (var item in list)
            {
                var secondKey = item[1];
                if (dataDic.ContainsKey(secondKey))
                    dataDic[secondKey]++;
                else
                    dataDic.Add(secondKey, 1);
            }
            return dataDic;
        }
        public static Tuple<Dictionary<string, int>, Dictionary<string, Dictionary<string, int>>> GetMultipleDic(IEnumerable<List<string>> list)
        {
            var dataDic = new Dictionary<string, int>();
            var yearBasedData = new Dictionary<string, Dictionary<string, int>>();
            foreach (var item in list)
            {
                var mainKey = item[0].Substring(0, 4);
                var secondKey = item[1];
                if (yearBasedData.ContainsKey(mainKey))
                {
                    var currentYear = yearBasedData[mainKey];
                    if (currentYear.ContainsKey(secondKey))
                        currentYear[secondKey]++;
                    else
                        currentYear.Add(secondKey, 1);
                }
                else
                    yearBasedData.Add(mainKey, new Dictionary<string, int>() { { secondKey, 1 } });

                if (dataDic.ContainsKey(secondKey))
                    dataDic[secondKey]++;
                else
                    dataDic.Add(secondKey, 1);
            }
            return new Tuple<Dictionary<string, int>, Dictionary<string, Dictionary<string, int>>>(dataDic, yearBasedData);
        }
    }
}
