using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal abstract class TemporaryEffect : UpdatingEffect
{
    [field: SerializeField] public float Duration { get; private set; }

    internal event Action Finished;

    protected TemporaryEffect(float duration) => Duration = duration;

    protected override void FixedUpdate()
    {
        Duration -= Time.fixedDeltaTime;
        if (Duration <= 0)
        {
            Finished?.Invoke();
            Stop();
        }
    }
}
