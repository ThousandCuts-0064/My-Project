using System;

public class EnumNoneException : EnumException
{
    public EnumNoneException(string paramName) : base(paramName, "Enum was None.") { }
}
