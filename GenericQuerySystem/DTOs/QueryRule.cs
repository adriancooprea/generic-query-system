using GenericQuerySystem.Enums;
using GenericQuerySystem.Utils;

namespace GenericQuerySystem.DTOs
{
    public class QueryRule
    {
        public QueryRule(string field, FieldOperation fieldOperation, object value, LogicalOperation logicalOperation)
        {
            ConditionChecker.Requires(string.IsNullOrEmpty(field) == false);
            ConditionChecker.Requires(value != null);

            Field = field;
            FieldOperation = fieldOperation;
            Value = value;
            LogicalOperation = logicalOperation;
        }

        public QueryRule(string field, FieldOperation fieldOperation, object value) : this(field, fieldOperation, value, LogicalOperation.And)
        {
        }

        public long Id { get; set; }

        public string Field { get; set; }

        public FieldOperation FieldOperation { get; set; }

        public object Value { get; set; }

        public LogicalOperation LogicalOperation { get; set; }
    }
}