using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class StatusEffect
{
    public virtual bool IsTemporary => false;

    internal abstract void Start();
    internal abstract void Stop();
}
