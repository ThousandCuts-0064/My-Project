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

    internal override void Start(Stats stats)
    {
        base.Start(stats);
        _resource = ResourceOfElement(stats.EmbeddedInternal, Element);
    }

    protected override void FixedUpdate()
    {
        _resource.Current += Stat.Value / _resource.Current;
    }
}
