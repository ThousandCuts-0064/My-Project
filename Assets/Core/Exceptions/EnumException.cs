using System;

public class EnumException : ArgumentOutOfRangeException
{
    protected EnumException(string paramName, string message) : base(paramName, message) { }
    protected EnumException(string paramName, Enum value, string message) : base(paramName, value, message) { }

    public static EnumException NoneOrNotDefined(string paramName, Enum value) =>
        value.GetHashCode() == 0
        ? new EnumNoneException(paramName)
        : new EnumNotDefinedException(paramName, value);
}
