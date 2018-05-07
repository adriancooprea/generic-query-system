using System.Collections.Generic;
using GenericQuerySystem.Interfaces;
using GenericQuerySystem.Utils;

namespace GenericQuerySystem.DTOs
{
    public class MultipleValuesQueryField : QueryField
    {
        public MultipleValuesQueryField(
            string name, 
            string type, 
            IList<KeyValuePair<string, object>> values) : base(name, type)
        {
            ConditionChecker.Requires(values != null);
            this.Values = values;
        }

        public IList<KeyValuePair<string, object>> Values { get; set; }
    }
}
