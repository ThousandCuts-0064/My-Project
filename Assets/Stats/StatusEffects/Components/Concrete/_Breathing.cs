using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    private protected class _Breathing : FixedUpdatable
    {
        private readonly FlatStat _flatStat;
        private readonly Element _element;
        private Resource _resource;

        internal _Breathing(Element element, float baseValue, out FlatStat flatStat)
        {
            flatStat = new FlatStat(baseValue);
            _flatStat = flatStat;
            _element = element;
        }

        internal override Component Clone() => new _Breathing(_element, _flatStat.Value, out _);

        internal override bool TryInitialize(Stats stats) => TryFind(stats.EmbeddedInternal, _element, out _resource);

        protected override void FixedUpdate() => _resource.Current += _flatStat.Value;
    }
}
