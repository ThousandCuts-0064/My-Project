using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    [Serializable]
    private protected abstract class _Component
    {
        internal abstract _Component Clone();
        internal abstract bool TryInitialize(Stats stats);
        internal abstract void Start();
        internal abstract void Stop();
    }
}
