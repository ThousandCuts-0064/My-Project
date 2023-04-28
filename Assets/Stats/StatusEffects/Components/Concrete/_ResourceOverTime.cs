using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    [Serializable]
    private protected class _ResourceOverTime : _FixedUpdatable
    {
        private Resource _resource;
        protected FlatStat FlatStat { get; }
        protected ResourceLayer ResourceLayer { get; }
        protected Element Element { get; }
        protected Resource Resource => _resource;

        internal _ResourceOverTime(ResourceLayer resourceLayer, Element element, float baseValue, out FlatStat flatStat)
        {
            flatStat = new FlatStat(baseValue);
            ResourceLayer = resourceLayer;
            FlatStat = flatStat;
            Element = element;
        }
        internal override _Component Clone() => new _ResourceOverTime(ResourceLayer, Element, FlatStat.Value, out _);

        internal override bool TryInitialize(Stats stats) => TryFind(stats.GetInternal(ResourceLayer), Element, out _resource);
        
        protected override void FixedUpdate() => Resource.Current += FlatStat.Value;

    }
}
