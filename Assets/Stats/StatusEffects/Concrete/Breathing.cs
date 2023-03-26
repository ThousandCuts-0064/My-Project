using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
internal class Breathing : UpdatingStatusEffect, IElementalStatusEffect
{
    private Resource _resource;
    [field: SerializeField] public Element Element { get; set; }

    internal override bool TryStart(Stats stats)
    {
        _resource = ResourceOfElement(stats.EmbeddedInternal, Element);
        if (_resource == null)
            return false;

        base.TryStart(stats);
        return true;
    }

    protected override void FixedUpdate()
    {
        _resource.Current += Stat.Value / _resource.Current;
    }
}
