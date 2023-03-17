using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class Breathing : UpdatingStatusEffect<IInternalStats>
{
    public Breathing(CharacterStats stats) : base(stats) { }

    protected override void FixedUpdate()
    {
        var res = Stats.Embedded.First(r => r.Element == Element.Air);
        res.Current += res.Max / 10;
    }
}
