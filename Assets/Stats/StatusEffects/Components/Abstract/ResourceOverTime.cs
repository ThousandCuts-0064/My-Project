using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    private protected abstract class ResourceOverTime : FixedUpdatable
    {
        private Resource _resource;
        protected FlatStat FlatStat { get; }
        protected Element Element { get; }
        protected Resource Resource => _resource;

        internal ResourceOverTime(Element element, float baseValue, out FlatStat flatStat)
        {
            flatStat = new FlatStat(baseValue);
            FlatStat = flatStat;
            Element = element;
        }

        internal override bool TryInitialize(Stats stats) => TryFind(SelectResourceList(stats), Element, out _resource);

        protected abstract IReadOnlyList<Resource> SelectResourceList(Stats stats);
        
        protected override void FixedUpdate() => Resource.Current += FlatStat.Value;
    }
}
