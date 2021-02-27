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
        public static string GetValueWithColor(string text, ConsoleColor color)
        {
            while (true)
            {
                WriteAndColorLine(text, color);
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
                if (int.TryParse(Console.ReadLine(), out int numberOfLikes) && (numberOfLikes > minValue && numberOfLikes <= maxValue))
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
        public static void ShowList(IEnumerable<string> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine("\"{0}\"", item);
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
            if (options.Length < 2 && options.Length > 0) return options[0];
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
            WriteAndColorLine(msgToShow, ConsoleColor.Red);
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
        public static void WriteAndColorLine(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
