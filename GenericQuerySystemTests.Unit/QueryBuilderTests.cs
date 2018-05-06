using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using GenericQuerySystem;
using GenericQuerySystem.DTOs;
using GenericQuerySystem.Enums;
using GenericQuerySystem.Interfaces;
using Xunit;

namespace GenericQuerySystemTests.Unit
{
    public class QueryBuilderTests
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

        public class TheBuildRulesPredicateMethodTests
        {
            private readonly IQueryBuilder<QueryTestClass> queryBuilder;

            private readonly System.Collections.Generic.IList<QueryTestClass> _testData;
            private readonly IList<QueryRule> _ruleList;

            public TheBuildRulesPredicateMethodTests()
            {
                IQueryCompiler<QueryTestClass> queryCompiler = new QueryCompiler<QueryTestClass>();
                queryBuilder = new QueryBuilder<QueryTestClass>(queryCompiler);
                _testData = new List<QueryTestClass>
                                {
                                    new QueryTestClass(5, "mere"),
                                    new QueryTestClass(10, "pere"),
                                    new QueryTestClass(20, "banane"),
                                    new QueryTestClass(7, "rosii"),
                                    new QueryTestClass(9, "castraveti"),
                                    new QueryTestClass(10, "cirese"),
                                    new QueryTestClass(15, "nuci"),
                                    new QueryTestClass(5, "mango"),
                                    new QueryTestClass(6, "nuci de cocos"),
                                };

                _ruleList = new List<QueryRule>
                                {
                                    new QueryRule("Number", "Equal", "5"),
                                    new QueryRule("Text", "Contains", "ere")
                                };
            }

            [Fact]
            public void Should_return_false_if_one_of_the_rules_fail()
            {
                // Act
                var result = queryBuilder.BuildRulesPredicate(new List<QueryRule>
                {
                    new QueryRule("Number", "Equal", "7"),
                    new QueryRule("Text", "Contains", "ere")
                });

                var filteredData = _testData.Where(x => result(x));

                // Assert
                Assert.Equal(0, filteredData.Count());
            }

            [Fact]
            public void Should_build_the_predicate()
            {
                // Act
                var result = queryBuilder.BuildRulesPredicate(_ruleList);
                var badResult = queryBuilder.BuildRulesPredicate(
                    new List<QueryRule>
                        {
                            new QueryRule("Number", "Equal", "5"),
                            new QueryRule("Number", "Equal", "6")
                        });

                var filteredData = _testData.Where(x => result(x));
                var badFilteredData = _testData.Where(x => badResult(x));

                // Assert
                Assert.Equal(_testData.Count(x => x.Number == 5 && x.Text.Contains("ere")), filteredData.Count());
                Assert.Equal(0, badFilteredData.Count());
            }

            [Fact]
            public void Should_build_the_predicate_rule_by_rule()
            {
                // Arrange
                var rules = new List<QueryRule>{
                    new QueryRule("Number", "Equal", "5", 0),
                                                    new QueryRule(
                                                        "Number",
                                                        "Equal",
                                                        "10",
                            GenericQuerySystem.Enums.LogicalOperation.OR),
                                                    new QueryRule(
                                                        "Text",
                                                        "Contains",
                                                        "cirese",
                                                        GenericQuerySystem.Enums.LogicalOperation.AND),

                };

                var group = new QueryGroup(rules, new List<QueryGroup>(), LogicalOperation.AND);

                var predicate = queryBuilder.BuildRulesPredicate(group.Rules);

                // Act
                var result = _testData.Where(x => predicate(x));
                var expectedResult =
                    _testData.Where(x => (x.Number == 5 || x.Number == 10) && x.Text.Contains("cirese"));
                var unexpectedResult =
                    _testData.Where(x => x.Number == 5 || x.Number == 10 && x.Text.Contains("cirese"));

                // Assert
                Assert.Equal(result.Count(), expectedResult.Count());
                Assert.NotEqual(result.Count(), unexpectedResult.Count());
            }

            [Fact]
            public void Should_return_the_composed_predicate()
            {
                // Arrange
                Predicate<QueryTestClass> p1 = delegate (QueryTestClass test) { return test.Number == 5; };
                Predicate<QueryTestClass> p2 = delegate (QueryTestClass test) { return test.Number == 6; };

                // Act
                var resultPredicate = queryBuilder.BuildAndPredicate(p1, p2);

                // Assert
                Assert.Equal(_testData.Count(x => x.Number == 5 && x.Number == 6), _testData.Count(x => resultPredicate(x)));
            }
        }

        public class TheBuildOrPredicateMethodTest
        {
            private readonly IQueryBuilder<QueryTestClass> queryBuilder;

            private readonly IList<QueryTestClass> _testData;
            private readonly IList<QueryRule> _ruleList;

            public TheBuildOrPredicateMethodTest()
            {
                IQueryCompiler<QueryTestClass> queryCompiler = new QueryCompiler<QueryTestClass>();
                queryBuilder = new QueryBuilder<QueryTestClass>(queryCompiler);
                _testData = new List<QueryTestClass>
                                {
                                    new QueryTestClass(5, "mere"),
                                    new QueryTestClass(10, "pere"),
                                    new QueryTestClass(20, "banane"),
                                    new QueryTestClass(7, "rosii"),
                                    new QueryTestClass(9, "castraveti"),
                                    new QueryTestClass(10, "cirese"),
                                    new QueryTestClass(15, "nuci"),
                                    new QueryTestClass(5, "mango"),
                                    new QueryTestClass(6, "nuci de cocos"),
                                };

                _ruleList = new List<QueryRule>
                                {
                                    new QueryRule("Number", "Equal", "5"),
                                    new QueryRule("Text", "Contains", "ere")
                                };
            }

            [Fact]
            public void Should_return_the_composed_predicate()
            {
                // Arrange
                Predicate<QueryTestClass> p1 = delegate (QueryTestClass test) { return test.Number == 5; };
                Predicate<QueryTestClass> p2 = delegate (QueryTestClass test) { return test.Number == 6; };

                // Act
                var resultPredicate = queryBuilder.BuildOrPredicate(p1, p2);

                // Assert
                Assert.Equal(_testData.Count(x => x.Number == 5 || x.Number == 6), _testData.Count(x => resultPredicate(x)));
            }
        }

        public class TheBuildGroupPredicateMethodTests
        {
            private readonly IQueryBuilder<QueryTestClass> queryBuilder;

            private readonly IList<QueryTestClass> _testData;
            private readonly IList<QueryRule> _ruleList;

            public TheBuildGroupPredicateMethodTests()
            {
                IQueryCompiler<QueryTestClass> queryCompiler = new QueryCompiler<QueryTestClass>();
                queryBuilder = new QueryBuilder<QueryTestClass>(queryCompiler);
                _testData = new List<QueryTestClass>
                                {
                                    new QueryTestClass(5, "mere"),
                                    new QueryTestClass(10, "pere"),
                                    new QueryTestClass(20, "banane"),
                                    new QueryTestClass(7, "rosii"),
                                    new QueryTestClass(9, "castraveti"),
                                    new QueryTestClass(10, "cirese"),
                                    new QueryTestClass(15, "nuci"),
                                    new QueryTestClass(5, "mango"),
                                    new QueryTestClass(6, "nuci de cocos"),
                                };

                _ruleList = new List<QueryRule>
                                {
                                    new QueryRule("Number", "Equal", "5"),
                                    new QueryRule("Text", "Contains", "ere")
                                };
            }

            [Fact]
            public void When_the_group_has_no_children_groups_should_return_the_predicate_based_on_the_rules()
            {
                // Act
                var result = queryBuilder.BuildGroupPredicate(new QueryGroup((System.Collections.Generic.List<GenericQuerySystem.DTOs.QueryRule>)_ruleList));

                // Assert
                Assert.Equal(_testData.Count(x => x.Number == 5 && x.Text.Contains("ere")), _testData.Count(x => result(x)));
            }

            [Fact]
            public void When_the_group_has_only_subgroups_should_return_the_predicate_based_on_them()
            {
                // Arrange - and example
                var group1 = new QueryGroup();
                group1.Rules.Add(new QueryRule("Number", "Equal", "20"));
                group1.Rules.Add(new QueryRule("Text", "Contains", "ane"));

                var group2 = new QueryGroup();
                group2.Rules.Add(new QueryRule("Number", "Equal", "3"));

                // Arrange - or example
                var group3 = new QueryGroup();
                group3.Rules.Add(new QueryRule("Number", "Equal", "20"));
                group3.Rules.Add(new QueryRule("Text", "Contains", "ane"));

                var group4 = new QueryGroup();
                group4.LogicalOperation = LogicalOperation.OR;
                group4.Rules.Add(new QueryRule("Number", "Equal", "3"));

                // Act
                var resultAnd = queryBuilder.BuildGroupPredicate(
                    new QueryGroup { InnerGroups = new List<QueryGroup> { group1, group2 } });
                var resultOr = queryBuilder.BuildGroupPredicate(
                    new QueryGroup { InnerGroups = new List<QueryGroup> { group3, group4 } });

                // Assert
                Assert.Equal(_testData.Count(x => (x.Number == 5 && x.Text.Contains("ane")) && x.Number == 20), _testData.Count(x => resultAnd(x)));
                Assert.Equal(_testData.Count(x => (x.Number == 5 && x.Text.Contains("ane")) || x.Number == 20), _testData.Count(x => resultOr(x)));
            }

            [Fact]
            public void
                When_group_has_subgroups_and_also_rules_should_return_an_and_condition_between_rules_and_subgroups()
            {
                // Arrange
                var group1 = new QueryGroup();
                group1.Rules.Add(new QueryRule("Number", "Equal", "20"));
                group1.Rules.Add(new QueryRule("Text", "Contains", "ane"));

                var group2 = new QueryGroup();
                group2.Rules.Add(new QueryRule("Number", "Equal", "3"));

                // Act
                var result = queryBuilder.BuildGroupPredicate(
                    new QueryGroup
                    {
                        InnerGroups = new List<QueryGroup> { group1, group2 },
                        Rules = new List<QueryRule>
                                        {
                                            new QueryRule("Yes", "Equal", "false")
                                        }
                    });

                // Assert
                Assert.Equal(_testData.Count(x => !x.Yes && ((x.Number == 5 && x.Text.Contains("ane")) || x.Number == 20)), _testData.Count(x => result(x)));
            }
        }

        public class TheBuildGroupsPredicateMethodTests
        {
            private readonly IQueryBuilder<QueryTestClass> queryBuilder;

            private readonly IList<QueryTestClass> _testData;
            private readonly IList<QueryRule> _ruleList;

            public TheBuildGroupsPredicateMethodTests()
            {
                IQueryCompiler<QueryTestClass> queryCompiler = new QueryCompiler<QueryTestClass>();
                queryBuilder = new QueryBuilder<QueryTestClass>(queryCompiler);
                _testData = new List<QueryTestClass>
                                {
                                    new QueryTestClass(5, "mere"),
                                    new QueryTestClass(10, "pere"),
                                    new QueryTestClass(20, "banane"),
                                    new QueryTestClass(7, "rosii"),
                                    new QueryTestClass(9, "castraveti"),
                                    new QueryTestClass(10, "cirese"),
                                    new QueryTestClass(15, "nuci"),
                                    new QueryTestClass(5, "mango"),
                                    new QueryTestClass(6, "nuci de cocos"),
                                };

                _ruleList = new List<QueryRule>
                                {
                                    new QueryRule("Number", "Equal", "5"),
                                    new QueryRule("Text", "Contains", "ere")
                                };
            }

            [Fact]
            public void Should_build_the_predicated_based_on_the_group_list()
            {
                // Arrange - and groups
                var group1 = new QueryGroup();
                group1.Rules.Add(new QueryRule("Number", "Equal", "20"));
                group1.Rules.Add(new QueryRule("Text", "Contains", "ane"));

                var group2 = new QueryGroup();
                group2.Rules.Add(new QueryRule("Number", "Equal", "3"));

                var group3 = new QueryGroup();
                group3.Rules.Add(new QueryRule("Number", "Equal", "5"));

                // Arrange - or groups
                var group4 = new QueryGroup { LogicalOperation = LogicalOperation.OR };
                group4.Rules.Add(new QueryRule("Number", "Equal", "20"));
                group4.Rules.Add(new QueryRule("Text", "Contains", "ane"));

                var group5 = new QueryGroup { LogicalOperation = LogicalOperation.OR };
                group5.Rules.Add(new QueryRule("Number", "Equal", "3"));

                var group6 = new QueryGroup { LogicalOperation = LogicalOperation.AND };
                group6.Rules.Add(new QueryRule("Number", "Equal", "5"));

                // Act
                var resultAnd = queryBuilder.BuildGroupPredicate(
                    new QueryGroup
                    {
                    InnerGroups = new List<QueryGroup>
                        {
                            new QueryGroup
                            {
                            InnerGroups = new List<QueryGroup>
                                {
                                    group1,
                                    group2
                                }
                            },
                            new QueryGroup
                                {
                            InnerGroups = new List<QueryGroup>
                                    {
                                        group2,
                                        group3
                                    }
                                }
                            }
                    });

                var resultOr = queryBuilder.BuildGroupPredicate(
                    new QueryGroup
                    {
                    InnerGroups = new List<QueryGroup>
                                              {
                                                  new QueryGroup
                                                      {
                            InnerGroups = new List<QueryGroup>
                                                                            {
                                                                                group4,
                                                                                group5
                                                                            }
                                                      },
                                                  new QueryGroup
                                                      {
                            InnerGroups = new List<QueryGroup>
                                                                            {
                                                                                group5,
                                                                                group6
                                                                            }
                                                      }
                                              }
                    });

                // Assert
                Assert.Equal(
                    _testData.Count(x =>
                    ((x.Number == 20 && x.Text.Contains("ane")) && x.Number == 3) &&
                    (x.Number == 3 || x.Number == 5)),
                    _testData.Count(x => resultAnd(x)));

                Assert.Equal(
                    _testData.Count(x =>
                        ((x.Number == 20 && x.Text.Contains("ane")) || x.Number == 3) &&
                        (x.Number == 3 || x.Number == 5)),
                    _testData.Count(x => resultOr(x)));
            }
        }
    }
}
