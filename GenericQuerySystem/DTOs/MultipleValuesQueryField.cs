using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace GenericQuerySystem.DTOs
{
    public class MultipleValuesQueryField : QueryField
    {
        public MultipleValuesQueryField(
            string name, 
            string type, 
            IList<KeyValuePair<string, object>> values) : base(name, type)
        {
            Contract.Requires(values != null);
            this.Values = values;
        }

        // Mocking
        protected MultipleValuesQueryField() {}

        public IList<KeyValuePair<string, object>> Values { get; set; }
    }
}
