using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    public static StatusEffect FlatMod(FlatStatType flatStatType, float value) => 
        new StatusEffect()
        .Add(new FlatModFlat(flatStatType, value));

    public static StatusEffect MultMod(FlatStatType flatStatType, float value) =>
        new StatusEffect()
        .Add(new MultModFlat(flatStatType, value));

    public static StatusEffect FlatMod(MultStatType multStatType, float value) =>
        new StatusEffect()
        .Add(new FlatModMult(multStatType, value));

    public static StatusEffect MultMod(MultStatType multStatType, float value) =>
        new StatusEffect()
        .Add(new MultModMult(multStatType, value));

    public StatusEffect Clone()
    {
        StatusEffect statusEffect = new();

        foreach (var component in _components)
            statusEffect.Add(component.Clone());

        foreach (var finisher in _finishers)
            statusEffect.Add(finisher.Clone());

        return statusEffect;
    }

    public StatusEffect Timer(float time) => Add(new TimerFinisher(time));
}
