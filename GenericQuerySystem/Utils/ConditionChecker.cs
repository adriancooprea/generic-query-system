using System;

namespace GenericQuerySystem.Utils
{
    public static class ConditionChecker
    {

        public static void Requires(bool condition)
        {
            if (!condition)
            {
                throw new ArgumentException("Condition not satisfied.");
            }
        }

        public static void Requires(bool condition, string message)
        {
            if (!condition)
            {
                throw new ArgumentException(message);
            }
        }
    }
}