using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Utility
{
    private static readonly StringBuilder _stringBuilder = new();
    public static StringBuilder ClearStringBuilder => _stringBuilder.Clear();

    public static string ToBackingField(string name) => ClearStringBuilder
        .Append("<")
        .Append(name)
        .Append(">k__BackingField")
        .ToString();

    public static string NameFromField(string name) => 
        name[0] == '_' 
        ? char.ToUpper(name[1]) + name[2..] 
        : name[0] == '<' 
            ? name[1..name.IndexOf('>')] 
            : name;

    public static string SeparateWords(string str)
    {
        Span<char> chars = stackalloc char[str.Length * 2 - 1];
        chars[0] = str[0];
        int charCount = 1;
        for (int i = 1; i < str.Length; i++)
        {
            if (char.IsUpper(str[i]))
                chars[charCount++] = ' ';

            chars[charCount++] = str[i];
        }
        return new string(chars);
    }
}
