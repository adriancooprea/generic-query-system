using GenericQuerySystem.Interfaces;
using GenericQuerySystem.Utils;

namespace GenericQuerySystem.DTOs
{
    public class SingleValueQueryField : QueryField
    {
        public SingleValueQueryField(string name, string type, object value) : base(name, type)
        {
            ConditionChecker.Requires(value != null);
            this.Value = value;
        }

        public object Value { get; set; }
    }
}
