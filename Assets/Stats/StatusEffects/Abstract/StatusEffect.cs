using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class StatusEffect
{
    internal StatusEffect() { }

    private protected static Resource ResourceOfElement(IReadOnlyList<Resource> resources, Element element)
    {
        for (int i = 0; i < resources.Count; i++)
            if (resources[i].Element == element)
                return resources[i];

        return null;
    }

    internal abstract void Start(Stats stats);
    internal abstract void Stop();
}
