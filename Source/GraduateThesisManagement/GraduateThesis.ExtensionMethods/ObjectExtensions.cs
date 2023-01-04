﻿
namespace GraduateThesis.ExtensionMethods;

public static class ObjectExtensions
{
    public static bool IsNumber(this object value)
    {
        return value is sbyte
                || value is byte
                || value is short
                || value is ushort
                || value is int
                || value is uint
                || value is long
                || value is ulong
                || value is float
                || value is double
                || value is decimal;
    }

    public static bool IsString(this object value)
    {
        return value is string;
    }

    public static bool IsBool(this object value)
    {
        return value is bool;
    }
}
