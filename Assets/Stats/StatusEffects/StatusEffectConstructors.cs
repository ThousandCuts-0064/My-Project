using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[type: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Nested concrete classes begin with '_'")]
public partial class StatusEffect
{
    public static StatusEffect New(string name) => new(name);

    public StatusEffect Clone()
    {
        StatusEffect statusEffect = new(_name);

        foreach (var component in _components)
            statusEffect.Add(component.Clone());

        foreach (var finisher in _finishers)
            statusEffect.Add(finisher.Clone());

        return statusEffect;
    }

    public StatusEffect FlatMod(FlatStatType flatStatType, float value) => Add(new _FlatModFlat(flatStatType, value));
    public StatusEffect FlatMod(MultStatType multStatType, float value) => Add(new _FlatModMult(multStatType, value));
    public StatusEffect MultMod(FlatStatType flatStatType, float value) => Add(new _MultModFlat(flatStatType, value));
    public StatusEffect MultMod(MultStatType multStatType, float value) => Add(new _MultModMult(multStatType, value));

    public StatusEffect EmbeddedResourceOverTime(Element element, float value, out FlatStat flatStat) => Add(new _EmbeddedResourceOverTime(element, value, out flatStat));
    public StatusEffect InternalResourceOverTime(Element element, float value, out FlatStat flatStat) => Add(new _InternalResourceOverTime(element, value, out flatStat));
    public StatusEffect ExternalResourceOverTime(Element element, float value, out FlatStat flatStat) => Add(new _ExternalResourceOverTime(element, value, out flatStat));

    public StatusEffect Timer(float time) => Add(new _Timer(time));
}
