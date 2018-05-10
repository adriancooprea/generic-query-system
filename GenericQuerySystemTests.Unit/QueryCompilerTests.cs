using System;
using System.Collections.Generic;
using System.Linq;
using GenericQuerySystem;
using GenericQuerySystem.DTOs;
using GenericQuerySystem.Enums;
using GenericQuerySystem.Interfaces;
using Xunit;

namespace GenericQuerySystemTests.Unit
{
    public class QueryCompilerTests
    {
        public class QueryTestClass
        {
            public QueryTestClass(int number, string text)
            {
                Number = number;
                Text = text;
            }

            public bool Yes { get; } = true;

            public int Number { get; }

            public string Text { get; }
        }

        public class TheCompileRuleMethodTests
        {
            private readonly IQueryCompiler<QueryTestClass> _queryCompiler;
            private readonly IList<QueryTestClass> _testData;

            public TheCompileRuleMethodTests()
            {
                _queryCompiler = new QueryCompiler<QueryTestClass>();
                _testData = new List<QueryTestClass>
                                {
                                    new QueryTestClass(5, "mere"),
                                    new QueryTestClass(10, "pere"),
                                    new QueryTestClass(20, "banane")
                                };
            }

            [Fact]
            public void Should_return_null_if_the_targeted_class_does_not_contain_the_queried_field()
            {
                // Act & Assert
                Assert.Null(_queryCompiler.CompileRule(
                    new QueryRule("BadField", FieldOperation.Equal, "5")));
            }

            [Fact]
            public void Should_compile_the_rules()
            {
                // Arrange
                var eqFunc = new Func<QueryTestClass, bool>(x => x.Number == 5);
                var containsFunc = new Func<QueryTestClass, bool>(x => x.Text.Contains("ere"));
                var gteFunc = new Func<QueryTestClass, bool>(x => x.Number >= 10);
                var ltFunc = new Func<QueryTestClass, bool>(x => x.Number < 2);
                var trueFunc = new Func<QueryTestClass, bool>(x => x.Yes);

                // Act
                var eqResult =
                    _queryCompiler.CompileRule(new QueryRule("Number", FieldOperation.Equal, "5"));
                var containsResuls =
                    _queryCompiler.CompileRule(
                        new QueryRule("Text", FieldOperation.Contains, "ere"));
                var gteResult = _queryCompiler.CompileRule(new QueryRule("Number", FieldOperation.GreaterThanOrEqual, "10"));
                var ltResult =
                    _queryCompiler.CompileRule(new QueryRule("Number", FieldOperation.LessThan, "2"));
                var trueResult =
                    _queryCompiler.CompileRule(new QueryRule("Yes", FieldOperation.Equal, "true"));

                // Assert
                Assert.Equal(_testData.Count(x => eqFunc(x)), _testData.Count(x => eqResult(x)));
                Assert.Equal(_testData.Count(x => gteFunc(x)), _testData.Count(x => gteResult(x)));
                Assert.Equal(_testData.Count(x => ltFunc(x)), _testData.Count(x => ltResult(x)));
                Assert.Equal(_testData.Count(x => trueFunc(x)), _testData.Count(x => trueResult(x)));
                Assert.Equal(_testData.Count(x => containsFunc(x)), _testData.Count(x => containsResuls(x)));
            }
        }
    }
}