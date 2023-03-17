using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal abstract class GenericStatusEffect<T> : StatusEffect where T : Stats
{
    protected T Stats { get; }

    protected GenericStatusEffect(T stats) => Stats = stats;
}
