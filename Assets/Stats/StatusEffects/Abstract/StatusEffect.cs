using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[Serializable]
public abstract class StatusEffect
{
#if UNITY_EDITOR
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for serialization")]
    [SerializeField, HideInInspector] private string _name;
#endif
    [field: SerializeField] internal Stat Stat { get; private set; } = new Stat(1);

    internal StatusEffect() 
    {
#if UNITY_EDITOR
        _name = GetType().Name;
#endif
    }

    private protected static Resource ResourceOfElement(IReadOnlyList<Resource> resources, Element element)
    {
        for (int i = 0; i < resources.Count; i++)
            if (resources[i].Element == element)
                return resources[i];

        return null;
    }

    internal abstract bool TryStart(Stats stats);
    internal abstract void Stop();
}
