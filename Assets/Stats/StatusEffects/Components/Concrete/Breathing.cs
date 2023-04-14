using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    private protected class Breathing : FixedUpdatable
    {
        private readonly FlatStat _flatStat;
        private readonly Element _element;

        internal Breathing(Element element, float baseValue, out FlatStat flatStat)
        {
            flatStat = new FlatStat(baseValue);
            _flatStat = flatStat;
            _element = element;
        }

        internal override Component Clone() => new Breathing(_element, _flatStat.Base, out _);

        internal override bool TryInitialize(Stats stats)
        {
            throw new System.NotImplementedException();
        }

        protected override void FixedUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}
