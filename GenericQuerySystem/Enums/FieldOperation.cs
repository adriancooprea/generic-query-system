using System.ComponentModel;

namespace GenericQuerySystem.Enums
{
    public enum FieldOperation
    {
        [Description("Bool,Int,Int32,Int64,Double,Decimal,Float,TimeSpan,DateTime,Select")]
        Equal = 0,

        [Description("Int,Int32,Int64,Double,Decimal,Float,TimeSpan,DateTime")]
        GreaterThan = 1,

        [Description("Int,Int32,Int64,Double,Decimal,Float,TimeSpan,DateTime")]
        GreaterThanOrEqual = 2,

        [Description("Int,Int32,Int64,Double,Decimal,Float,TimeSpan,DateTime")]
        LessThan = 3,

        [Description("Int,Int32,Int64,Double,Decimal,Float,TimeSpan,DateTime")]
        LessThanOrEqual = 4,

        [Description("String")]
        Contains = 5,

        [Description("String")]
        StartsWith = 6,

        [Description("String")]
        EndsWith = 7
    }
}
