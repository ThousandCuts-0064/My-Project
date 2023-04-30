using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    public static StatusEffect New(string name) => new(name);

    public StatusEffect Mod(FlatStatType flatStatType, StatType modType, float value) => modType switch
    {
        StatType.Flat => Add(new _FlatModFlat(flatStatType, value)),
        StatType.Mult => Add(new _MultModFlat(flatStatType, value)),

        _ => throw EnumException.NoneOrNotDefined(nameof(modType), modType)
    };

    public StatusEffect Mod(MultStatType multStatType, StatType modType, float value) => modType switch
    {
        StatType.Flat => Add(new _FlatModMult(multStatType, value)),
        StatType.Mult => Add(new _MultModMult(multStatType, value)),

        _ => throw EnumException.NoneOrNotDefined(nameof(modType), modType)
    };

    public StatusEffect OverTime(ResourceLayer resourceLayer, Element element, float value, out FlatStat flatStat) =>
        Add(new _ResourceOverTime(resourceLayer, element, value, out flatStat));

    public StatusEffect Timer(float time) => Add(new _Timer(time));
}
