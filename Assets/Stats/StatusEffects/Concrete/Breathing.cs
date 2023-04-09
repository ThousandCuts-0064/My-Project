using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
internal class Breathing : UpdatingEffect, IElementalStatusEffect, IStatStatusEffect<FlatStat>
{
    private Resource _resource;
    [field: SerializeField] public Element Element { get; set; }
    public FlatStat Stat { get; }

    private protected override bool TryInitialize(Stats stats)
    {
        var state = Element.GetState();
        return (state == ElementType.Liquid 
            || state == ElementType.Gas)
            && TryFind(stats.EmbeddedInternal, Element, out _resource); 
    }

    protected override void FixedUpdate() => _resource.Current += Stat.Value;
}
