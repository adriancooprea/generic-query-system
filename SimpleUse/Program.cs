using System;
using System.Collections.Generic;
using System.Linq;
using GenericQuerySystem;
using GenericQuerySystem.DTOs;
using GenericQuerySystem.Interfaces;

namespace SimpleUse
{
    public class Program
    {
        static void Main(string[] args)
        {
            var someList = new DummyDataGenerator().GenerateDummyData();

            IQueryCompiler<SimpleModel> compiler = new QueryCompiler<SimpleModel>();
            IQueryBuilder<SimpleModel> builder = new QueryBuilder<SimpleModel>(compiler);
            IQueryFilterEngine<SimpleModel> engine = new QueryFilterEngine<SimpleModel>(builder);
           
            var onlyEnabledOnesRule = new QueryRule("Enabled", "Equal", "false");

            var onlyEnabledOnes = new QueryGroup(new List<QueryRule>
            {
                onlyEnabledOnesRule
            });

            var result = engine.FilterCollection(someList, onlyEnabledOnes);

            Console.WriteLine(result.Count());
            Console.ReadKey();
        }
       
    }
}
