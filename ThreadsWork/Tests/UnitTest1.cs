using NUnit.Framework;
using System;
using System.Collections.Generic;
using ThreadsWork;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void MostPopularStringsByPLinq_InputFiveStringsWithThreeSame_DictionarySizeEquals2()
        {
            
            List<string>Strings = new List<String>();
            List<KeyValuePair<string,int>>answerStrings = new List<KeyValuePair<string, int>>();

            Strings.Add("Moon");
            Strings.Add("Sun");
            Strings.Add("Sun");
            Strings.Add("Moon");
            Strings.Add("Moon");

            //act
            answerStrings=ThreadsFunction.MostPopularStringsByPLinq(Strings,2,3);

            //assert
            Assert.AreEqual(answerStrings.Count, 2);
        }
        [Test]
        public void MostPopularStringsByThreads_InputFiveStringsWithThreeSame_DictionarySizeEquals2()
        {
            
            List<string>Strings = new List<String>();
            List<KeyValuePair<string,int>>answerStrings = new List<KeyValuePair<string, int>>();

            Strings.Add("Moon");
            Strings.Add("Sun");
            Strings.Add("Sun");
            Strings.Add("Moon");
            Strings.Add("Moon");

            //act
            answerStrings=ThreadsFunction.MostPopularStringsByThreads(Strings,2,3);

            //assert
            Assert.AreEqual(answerStrings.Count, 2);
        }
        [Test]
        public void MostPopularStringsByPLinq_CompareTimeOfDifferentAmountOfThreadsWorking_SecondTimeIsGreater()
        {
            List<string>Strings = AdditionalClass.GenerateStringsList(200);
            List<KeyValuePair<string,int>>answerStrings = new List<KeyValuePair<string, int>>();
            
            //act
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            ThreadsFunction.MostPopularStringsByPLinq(Strings,5,5);
            watch.Stop();
            var elapsedMs1 = watch.ElapsedMilliseconds;

            watch.Start();
            ThreadsFunction.MostPopularStringsByPLinq(Strings,5,200);
            watch.Stop();
            var elapsedMs2 = watch.ElapsedMilliseconds;

            //assert
            Assert.Less(elapsedMs1, elapsedMs2);
        }
        [Test]
        public void MostPopularStringsByThreads_CompareTimeOfDifferentAmountOfThreadsWorking_SecondTimeIsGreater()
        {
            List<string>Strings = AdditionalClass.GenerateStringsList(200);
            List<KeyValuePair<string,int>>answerStrings = new List<KeyValuePair<string, int>>();
            
            //act
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            ThreadsFunction.MostPopularStringsByThreads(Strings,5,5);
            watch.Stop();
            var elapsedMs1 = watch.ElapsedMilliseconds;

            watch.Start();
            ThreadsFunction.MostPopularStringsByThreads(Strings,5,200);
            watch.Stop();
            var elapsedMs2 = watch.ElapsedMilliseconds;

            //assert
            Assert.Less(elapsedMs1, elapsedMs2);
        }
    }
}