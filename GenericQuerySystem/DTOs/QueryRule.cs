using System.Diagnostics.Contracts;
using GenericQuerySystem.Enums;

namespace GenericQuerySystem.DTOs
{
    public class QueryRule
    {
        public QueryRule(string field, string condition, string value, LogicalOperation logicalOperation)
        {

            Contract.Requires(string.IsNullOrEmpty(field) == false);
            Contract.Requires(string.IsNullOrEmpty(condition) == false);
            Contract.Requires(string.IsNullOrEmpty(value) == false);

            Field = field;
            Condition = condition;
            Value = value;
            LogicalOperation = logicalOperation;
        }

        public QueryRule(string field, string condition, string value) : this(field, condition, value, LogicalOperation.AND) {}

        // Mocking
        protected QueryRule() {}

        public long Id { get; set; }

        public string Field { get; set; }

        public string Condition { get; set; }

        public string Value { get; set; }

        public LogicalOperation LogicalOperation { get; set; }
    }
}
