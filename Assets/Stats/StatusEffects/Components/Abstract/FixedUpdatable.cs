using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    private protected abstract class FixedUpdatable : Component
    {
        internal override void Start() => GameManager.FixedUpdateEvent += FixedUpdate;
        internal override void Stop() => GameManager.FixedUpdateEvent -= FixedUpdate;

        protected abstract void FixedUpdate();
    }
}

