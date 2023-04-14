using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    private protected abstract class Finisher : Component
    {
        internal abstract bool IsFinished();
    }
}
