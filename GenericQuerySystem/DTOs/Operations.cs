using System.Collections.Generic;
using GenericQuerySystem.Enums;

namespace GenericQuerySystem.DTOs
{
    internal class Operations
    {
        private static readonly Dictionary<FieldOperation, string> FieldOperations;

        static Operations()
        {
            FieldOperations = new Dictionary<FieldOperation, string>
            {
                {FieldOperation.Equal, "Equal" },
                {FieldOperation.GreaterThan, "GreaterThan" },
                {FieldOperation.GreaterThanOrEqual, "GreaterThanOrEqual" },
                {FieldOperation.LessThan, "LessThan" },
                {FieldOperation.LessThanOrEqual, "LessThanOrEqual" },
                {FieldOperation.Contains, "Contains" },
                {FieldOperation.StartsWith, "StartsWith" },
                {FieldOperation.EndsWith, "EndsWith" }
            };
        }

        internal static string Get(FieldOperation fieldOperation)
        {
            return FieldOperations[fieldOperation];
        }
    }
}