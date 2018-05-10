using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericQuerySystem;
using GenericQuerySystem.DTOs;
using GenericQuerySystem.Enums;
using Newtonsoft.Json;
using Xunit;

namespace SimpleUse
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var someList = new DummyDataGenerator().GenerateDummyData();
            var engine = QueryFactory<SimpleModel>.BuildQueryFilterEngine();

            #region Filtering

            // boolean field
            var onlyEnabledOnesRule = new QueryRule("Enabled", FieldOperation.Equal, false);
            var onlyEnabledOnes = new QueryGroup(onlyEnabledOnesRule);

            var result = engine.FilterCollection(someList, onlyEnabledOnes);
            Console.WriteLine("----Testing against boolean fields----");
            Assert.Equal(1, result.Count());

            // number field
            var onlySomeIdsRule = new QueryRule("Id", FieldOperation.GreaterThan, 2);
            var onlySomeIds = new QueryGroup(onlySomeIdsRule);

            result = engine.FilterCollection(someList, onlySomeIds);
            Console.WriteLine("----Testing against integer fields----");
            Assert.Equal(1, result.Count());

            // string fields
            var startsWithRule = new QueryRule("Name", FieldOperation.StartsWith, "A");
            var endsWithRule = new QueryRule("Name", FieldOperation.EndsWith, "f");
            var startsEnds = new QueryGroup().And(startsWithRule).Or(endsWithRule);

            result = engine.FilterCollection(someList, startsEnds);
            Console.WriteLine("----Testing against string fields----");
            Assert.Equal(2, result.Count());

            // datetime fields
            var datesGreaterThanRule = new QueryRule("Date", FieldOperation.GreaterThan, new DateTime(2017, 1, 1));
            var datesGreaterThan = new QueryGroup().And(datesGreaterThanRule);

            result = engine.FilterCollection(someList, datesGreaterThan);
            Console.WriteLine("----Testing against datetime fields----");
            Assert.Equal(1, result.Count());

            // timespan
            var timeSpanGreaterThanRule = new QueryRule("TimeSpan", FieldOperation.GreaterThan, new DateTime(2017, 1, 1, 8, 0, 0).TimeOfDay);
            var timeSpanLessThanRule = new QueryRule("TimeSpan", FieldOperation.LessThan, new DateTime(2018, 12, 1, 23, 0, 0).TimeOfDay);
            var timeSpanGreaterLess = new QueryGroup().And(timeSpanGreaterThanRule).And(timeSpanLessThanRule);

            result = engine.FilterCollection(someList, timeSpanGreaterLess);
            Console.WriteLine("----Testing against timespan fields----");
            Assert.Equal(1, result.Count());

            // enums
            var enumEqualRule = new QueryRule("SimpleEnum", FieldOperation.Equal, SimpleEnum.Option0);
            var enumEqual = new QueryGroup().And(enumEqualRule);

            result = engine.FilterCollection(someList, enumEqual);
            Console.WriteLine("----Testing against enum fields----");
            Assert.Equal(2, result.Count());

            #endregion Filtering

            #region Sorting

            // ascending
            var ascendingByDateRule = new QuerySorter("Date", SortingOperation.Ascending);
            var ascendingByDate = new QueryGroup().AscendingBy(ascendingByDateRule);

            result = engine.SortCollection(someList, ascendingByDate);
            Console.WriteLine("----Testing against ascending sorting----");
            Assert.StrictEqual(JsonConvert.SerializeObject(someList.OrderBy(x => x.Date)), JsonConvert.SerializeObject(result));

            // desceding
            var descendingByDateRule = new QuerySorter("Date", SortingOperation.Descending);
            var descendingById = new QuerySorter("Id", SortingOperation.Descending);
            var descendingByName = new QuerySorter("Name", SortingOperation.Descending);

            var multipleDescending = new QueryGroup().DescendingBy(descendingByDateRule).DescendingBy(descendingById).DescendingBy(descendingByName);

            result = engine.SortCollection(someList, multipleDescending);
            Console.WriteLine("----Testing against descending sorting----");
            Assert.StrictEqual(JsonConvert.SerializeObject(someList.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id).ThenByDescending(x => x.Name)), JsonConvert.SerializeObject(result));

            #endregion Sorting

            #region Filtering Fields

            var fieldsToShow = new string[] { "Id", "Name", "SimpleEnum" };
            var filteredResult = engine.FilterFields(someList, fieldsToShow);

            Console.WriteLine("----Testing against filtering fields----");
            Assert.Equal(3, filteredResult.Count());
            Assert.Equal(2, filteredResult.Count(x => x.SimpleEnum == SimpleEnum.Option0.ToString()));

            #endregion Filtering Fields

            Console.WriteLine("---------------------------------All tests passed------------------------------------");
            Console.ReadKey();
        }
    }
}