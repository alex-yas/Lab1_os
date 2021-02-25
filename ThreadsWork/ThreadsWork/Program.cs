using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ThreadsWork
{
    public class ThreadsFunction
    {
        public static List<KeyValuePair<string, int>> MostPopularStringsByThreads(List<string> Strings, 
            int popularStringsNumber, int threadsNumber)
        {
            int stringsNumber = Strings.Count;
            int stringsPortion = stringsNumber / threadsNumber + 1;
            int finalThreadsNumber;
            if (threadsNumber > stringsNumber)
                finalThreadsNumber = stringsNumber;
            else
                finalThreadsNumber = threadsNumber;
            List<ConcurrentDictionary<string, int>> threadsDictionaries = new List<ConcurrentDictionary<string, int>>();
            for (int i = 0; i < finalThreadsNumber; i++)
                threadsDictionaries.Add(new ConcurrentDictionary<string, int>());

            Thread[] threads = new Thread[finalThreadsNumber];
            for (int i = 0; i < finalThreadsNumber; i++)
            {
                threads[i] = new Thread((object p)=>
                {
                    ThreadsParametrs parametrs = (ThreadsParametrs) p;
                    int stringsPortion=parametrs.portion;
                    int begin = parametrs.begin;
                    int end = begin + stringsPortion;
                    List<string> strings = parametrs.strings;
                    ConcurrentDictionary<string,int> dictionary = parametrs.dictionary;
                    if (begin >= strings.Count)
                        return;
                    if (end > strings.Count)
                        end = strings.Count;
                    for (int i = begin; i < end; i++)
                        if (!dictionary.TryAdd(strings[i], 1))
                        {
                            dictionary.TryGetValue(strings[i], out int value);
                            dictionary.TryUpdate(strings[i], value + 1, value);
                        }
                });
            }
            for (int i = 0; i < finalThreadsNumber; i++)
            {
                ThreadsParametrs parametrs = new ThreadsParametrs(stringsPortion,i*stringsPortion,
                    Strings,threadsDictionaries[i]);
                threads[i].Start(parametrs);
            }
            for (int i = 0; i < finalThreadsNumber; i++)
                threads[i].Join();
            return AdditionalClass.ChooseMostPopular(threadsDictionaries, popularStringsNumber);
        }
        public static List<KeyValuePair<string,int>> MostPopularStringsByPLinq
            (List<string> strings,int popularStringsNumber,int threadsNumber)
        {
            int stringsNumber = strings.Count;
            List<ConcurrentDictionary<string, int>> dictionaries = new List<ConcurrentDictionary<string, int>>();
            var pquery = from num in Enumerable.Range(0, threadsNumber)
                    .AsParallel().WithDegreeOfParallelism(threadsNumber) select num;
            int portion = stringsNumber / threadsNumber + 1;
            pquery.ForAll(dictionaryIndex =>
                {
                    int begin = portion * dictionaryIndex;
                    int end = begin + portion;
                    if (end > stringsNumber)
                        end = stringsNumber;
                    ConcurrentDictionary<string, int> curdictionary = new ConcurrentDictionary<string, int>();
                    for (int i = begin; i < end; i++)
                        if (!curdictionary.TryAdd(strings[i], 1))
                        {
                            curdictionary.TryGetValue(strings[i], out int value);
                            curdictionary.TryUpdate(strings[i], value + 1, value);
                        }
                    dictionaries.Add(curdictionary);
                });
            return AdditionalClass.ChooseMostPopular(dictionaries,popularStringsNumber);
        }
    }
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Input the number of strings to be written in the top-N.");
            int popularStringsNumber = 0;
            int threadsNumber = 0;
            try
            {
                popularStringsNumber = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
                Console.WriteLine("Input the number of threads.");
                threadsNumber = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            List<string> Strings = AdditionalClass.InitializeStringsList();
            //List<string> Strings = AdditionalClass.GenerateStringsList(1);
            
            //List<KeyValuePair<string,int>> answerStrings=ThreadsFunction.
                //MostPopularStringsByThreads(Strings,popularStringsNumber,threadsNumber);
           List<KeyValuePair<string,int>> answerStrings=ThreadsFunction.
              MostPopularStringsByPLinq(Strings,popularStringsNumber,threadsNumber);
            int answerStringsNumber = answerStrings.Count;
            
            Console.WriteLine("\nTop-" + answerStringsNumber + " most popular strings in the list:");
            for (int i = 0; i < answerStringsNumber; i++)
                Console.WriteLine((i + 1) + ".\tString: " + answerStrings[i].Key + ", frequency: " +
                                  answerStrings[i].Value);
        }
    }
}