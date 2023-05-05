using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[type: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Nested concrete classes begin with '_'")]
[Serializable]
public partial class StatusEffect
{
    [SerializeReference, HideInInspector] private List<_Component> _components;
    [SerializeReference, HideInInspector] private List<_Finisher> _finishers;
    [SerializeField, HideInInspector] private string _name;
    public string Name => _name;

    private StatusEffect(string name) => _name = name;

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

    public StatusEffect Clone()
    {
        StatusEffect statusEffect = new(Name);

        foreach (var component in _components)
            statusEffect.Add(component.Clone());

        if (_finishers != null)
            foreach (var finisher in _finishers)
                statusEffect.Add(finisher.Clone());

        return statusEffect;
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

    private protected StatusEffect Add(_Component component)
    {
        if (_components is null)
            _components = new();

        _components.Add(component);
        return this;
    }

    private protected StatusEffect Add(_Finisher finisher)
    {
        if (_finishers is null)
            _finishers = new();

        _finishers.Add(finisher);
        return this;
    }
}
