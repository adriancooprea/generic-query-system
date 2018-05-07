using GenericQuerySystem.Enums;
using GenericQuerySystem.Utils;

namespace GenericQuerySystem.DTOs
{
    public class QueryRule
    {
        public QueryRule(string field, string condition, string value, LogicalOperation logicalOperation)
        {

            ConditionChecker.Requires(string.IsNullOrEmpty(field) == false);
            ConditionChecker.Requires(string.IsNullOrEmpty(condition) == false);
            ConditionChecker.Requires(string.IsNullOrEmpty(value) == false);

            Field = field;
            Condition = condition;
            Value = value;
            LogicalOperation = logicalOperation;
        }

        public QueryRule(string field, string condition, string value) : this(field, condition, value, LogicalOperation.And) {}

        public long Id { get; set; }

        public string Field { get; set; }

        public string Condition { get; set; }

        public string Value { get; set; }

        public LogicalOperation LogicalOperation { get; set; }
    }
}
