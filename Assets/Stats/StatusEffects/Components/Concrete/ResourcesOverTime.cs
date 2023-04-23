using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    private protected class _EmbeddedResourceOverTime : ResourceOverTime
    {
        internal _EmbeddedResourceOverTime(Element element, float baseValue, out FlatStat flatStat) : base(element, baseValue, out flatStat) { }

        internal override Component Clone() => new _EmbeddedResourceOverTime(Element, FlatStat.Value, out _);
        protected override IReadOnlyList<Resource> SelectResourceList(Stats stats) => stats.EmbeddedInternal;
    }

    private protected class _InternalResourceOverTime : ResourceOverTime
    {
        internal _InternalResourceOverTime(Element element, float baseValue, out FlatStat flatStat) : base(element, baseValue, out flatStat) { }

        internal override Component Clone() => new _InternalResourceOverTime(Element, FlatStat.Value, out _);
        protected override IReadOnlyList<Resource> SelectResourceList(Stats stats) => stats.InternalsInternal;
    }

    private protected class _ExternalResourceOverTime : ResourceOverTime
    {
        internal _ExternalResourceOverTime(Element element, float baseValue, out FlatStat flatStat) : base(element, baseValue, out flatStat) { }

        internal override Component Clone() => new _ExternalResourceOverTime(Element, FlatStat.Value, out _);
        protected override IReadOnlyList<Resource> SelectResourceList(Stats stats) => stats.ExternalsInternal;
    }
}
