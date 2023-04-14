using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[Serializable]
public partial class StatusEffect
{
#if UNITY_EDITOR
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for serialization")]
    [SerializeField, HideInInspector] private string _name;
#endif
    private List<Component> _components;
    private List<Finisher> _finishers;

    internal StatusEffect()
    {
#if UNITY_EDITOR
        _name = _components?[0].GetType().Name ?? "Error";
#endif
    }

    private protected static bool TryFind(IReadOnlyList<Resource> resources, Element element, out Resource resource)
    {
        for (int i = 0; i < resources.Count; i++)
            if (resources[i].Element == element)
            {
                resource = resources[i];
                return true;
            }

        resource = null;
        return false;
    }

    internal bool TryStart(Stats stats)
    {
        if (!TryInitialize(stats))
            return false;

        Start();
        return true;
    }

    internal virtual void Stop()
    {
        foreach (var component in _components)
            component.Stop();
    }

    private protected virtual bool TryInitialize(Stats stats)
    {
        foreach (var component in _components)
            if (!component.TryInitialize(stats))
                return false;

        if (_finishers != null)
            foreach (var finisher in _finishers)
                if (!finisher.TryInitialize(stats))
                    return false;

        return true;
    }

    private protected virtual void Start()
    {
        foreach (var component in _components)
            component.Start();

        if (_finishers != null)
            foreach (var finisher in _finishers)
                finisher.Start();
    }

    private protected StatusEffect Add(Component component)
    {
        if (_components is null)
            _components = new();

        _components.Add(component);
        return this;
    }

    private protected StatusEffect Add(Finisher finisher)
    {
        if (_finishers is null)
            _finishers = new();

        _finishers.Add(finisher);
        return this;
    }
}
