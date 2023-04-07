using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[Serializable]
public abstract partial class StatusEffect
{
#if UNITY_EDITOR
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for serialization")]
    [SerializeField, HideInInspector] private string _name;
#endif
    private List<Component> _components;

    internal StatusEffect() 
    {
#if UNITY_EDITOR
        _name = GetType().Name;
#endif
    }

    private protected void AddComponent(Component component)
    {
        if (_components is null)
            _components = new();

        _components.Add(component);
    }

    private protected static Resource ResourceOfElement(IReadOnlyList<Resource> resources, Element element)
    {
        for (int i = 0; i < resources.Count; i++)
            if (resources[i].Element == element)
                return resources[i];

        return null;
    }

    internal virtual bool TryStart(Stats stats)
    {
        foreach (var component in _components)
            if (!component.TryInitialize(stats))
                return false;

        foreach (var component in _components)
            component.Start();

        return true;
    }

    internal virtual void Stop()
    {
        foreach (var component in _components)
            component.Stop();
    }
}
