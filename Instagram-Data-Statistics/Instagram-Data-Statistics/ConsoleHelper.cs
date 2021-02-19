using System;
using System.Collections.Generic;
using System.Threading;

namespace Instagram_Data_Statistics
{
    public static class ConsoleHelper 
    {
        public static string GetValue(string text)
        {
            while (true)
            {
                Console.WriteLine(text);
                string value = Console.ReadLine();
                if (!string.IsNullOrEmpty(value)) return value;
                ClearLines(2);
            }
        }
        public static int GetNum(string text, int maxValue = int.MaxValue, int minValue = 0)
        {
            Console.WriteLine(text);
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int numberOfLikes) && (numberOfLikes > 0 && numberOfLikes <= maxValue))
                {
                    return numberOfLikes;
                }
                ClearLines();
            }
        }
        public static void ShowList(IEnumerable<KeyValuePair<string, int>> list)
        {
            foreach (var item in list)
            {
                if (item.Value == 1)
                    Console.WriteLine("You liked one post from {0} ", item.Key);
                else
                    Console.WriteLine("You liked {1} posts from {0}", item.Key, item.Value);
            }
        }
        public static void ShowList(IEnumerable<KeyValuePair<string, int>> list, int numOfElemToShow)
        {
            int i = 0;
            foreach (var item in list)
            {
                if (item.Value == 1)
                    Console.WriteLine("You liked one post from {0} ", item.Key);
                else
                    Console.WriteLine("You liked {1} posts from {0}", item.Key, item.Value);
                i++;
                if (i == numOfElemToShow)
                    return;
            }
        }
        public static ConsoleKey GetChoice(string question, ConsoleKey[] bounds)
        {
            while (true)
            {
                Console.WriteLine(question);
                var keyFromUser = Console.ReadKey(true).Key;
                foreach (var key in bounds)
                {
                    if (key == keyFromUser) return key;
                }
                ClearLines(bounds.Length + 1);
            }
        }
        public static string GetChoice(string question, string[] options)
        {
            Console.WriteLine(question);
            foreach (var option in options)
            {
                Console.WriteLine(option);
            }
            while (true)
            {
                var response = Console.ReadLine();
                foreach (var option in options)
                {
                    if(option == response)
                    {
                        return response;
                    }
                }
                WaitAndClearLines(2, 2000, "Ooops, that response is invalid!");
            }
        }
        public static void WaitAndClearLines(int linesToClear, int timeToWait, string msgToShow)
        {
            Console.WriteLine(msgToShow);
            Thread.Sleep(timeToWait);
            ClearLines(linesToClear);
        }
        public static void ClearLines(int lines = 1)
        {
            if (lines > 0)
            {
                Console.SetCursorPosition(0, Console.CursorTop - lines);
                for (int i = 0; i < lines; i++)
                {
                    Console.Write(new string(' ', Console.BufferWidth));
                }
                Console.SetCursorPosition(0, Console.CursorTop - (lines - 1));
            }
        }
    }
}
