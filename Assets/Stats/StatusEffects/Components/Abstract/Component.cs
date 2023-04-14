using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    private protected abstract class Component
    {
        internal abstract Component Clone();
        internal abstract bool TryInitialize(Stats stats);
        internal abstract void Start();
        internal abstract void Stop();
    }
}
