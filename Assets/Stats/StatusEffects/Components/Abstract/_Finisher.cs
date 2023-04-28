using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    [Serializable]
    private protected abstract class _Finisher : _Component
    {
        internal abstract bool IsFinished();
    }
}
