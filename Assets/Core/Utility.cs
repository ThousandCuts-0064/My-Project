using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class Utility
{
    private static readonly StringBuilder _stringBuilder = new();
    public static StringBuilder ClearedStringBuilder => _stringBuilder.Clear();
}
