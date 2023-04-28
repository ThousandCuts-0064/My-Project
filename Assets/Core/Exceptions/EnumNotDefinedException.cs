using System;

public class EnumNotDefinedException : EnumException
{
    public EnumNotDefinedException(string paramName, Enum value) : base(paramName, value, "Not defined.") { }
}
