using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ThreadsWork
{
    public class ThreadsParametrs
    {
        public int portion;
        public int begin;
        public List<string> strings;
        public ConcurrentDictionary<string, int> dictionary;
        public ThreadsParametrs(int p, int b, List<string> s, ConcurrentDictionary<string, int> d)
        {
            portion = p;
            begin = b;
            strings = s;
            dictionary = d;
        }
    }
    public class Com : IComparer
    {
        int IComparer.Compare(Object x, Object y)
        {
            KeyValuePair<string, int> a = (KeyValuePair<string, int>)x;
            KeyValuePair<string, int> b = (KeyValuePair<string, int>)y;
            if (a.Value > b.Value)
                return -1;
            if (a.Value < b.Value)
                return 1;
            return 0;
        }
    }
    public static class AdditionalClass
    {
        public static List<KeyValuePair<string, int>> ChooseMostPopular(
            List<ConcurrentDictionary<string, int>> dictionaries,int popularStringsNumber)
        {
            ConcurrentDictionary<string, int> sortedStrings = new ConcurrentDictionary<string, int>();
            string key;
            foreach (ConcurrentDictionary<string, int> dict in dictionaries)
                for (int i = 0; i < dict.Count; i++)
                {
                    key = dict.ElementAt(i).Key;
                    if (sortedStrings.ContainsKey(key))
                        sortedStrings.TryUpdate(key, 
                            sortedStrings.GetValueOrDefault(key) + dict.GetValueOrDefault(key), 
                            sortedStrings.GetValueOrDefault(key));
                    else
                        sortedStrings.TryAdd(key, dict.GetValueOrDefault(key));
                }
            List<KeyValuePair<string, int>> answerStrings = new List<KeyValuePair<string, int>>();
            Array arraySortedStrings = sortedStrings.ToArray();
            int answerStringsNumber;
            if (popularStringsNumber > arraySortedStrings.Length)
                answerStringsNumber = arraySortedStrings.Length;
            else
                answerStringsNumber = popularStringsNumber;
            Array.Sort(arraySortedStrings, new Com());
            for (int i = 0; i < answerStringsNumber; i++)
                answerStrings.Add((KeyValuePair<string, int>)arraySortedStrings.GetValue(i));
            return answerStrings;
        }
        public static  List<string> InitializeStringsList()
        {
            List<string> Strings = new List<string>();
            Console.WriteLine("Input the number of strings for the list.");
            int number = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            Console.WriteLine("Input the strings one by one.");
            for (int i = 0; i < number; i++)
                Strings.Add(Console.ReadLine());
            return Strings;
        }
        public static List<string> GenerateStringsList(int lowerBound)
        {
            List<string> Strings = new List<string>();
            Random random = new Random();
            int stringsNumber = random.Next(lowerBound, 1000);
            for (int i = 0; i < stringsNumber; i++)
            {
                int stringLength = random.Next(1, 100);
                string currentString = "";
                for (int j = 0; j < stringLength; j++)
                {
                    int symbolCode = random.Next(48, 126);
                    currentString += Convert.ToChar(symbolCode);
                }
                Strings.Add(currentString);
            }
            return Strings;
        }
    }
    
}