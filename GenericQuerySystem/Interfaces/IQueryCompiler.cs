using System;
using GenericQuerySystem.DTOs;

namespace GenericQuerySystem.Interfaces
{
    internal interface IQueryCompiler<in T> where T : class
    {
        Predicate<T> CompileRule(QueryRule queryRule);
    }
}
