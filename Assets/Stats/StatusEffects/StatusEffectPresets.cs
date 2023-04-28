using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatusEffectPresets
{
    public static StatusEffect Mod(FlatStatType flatStatType, StatType modType, float value) =>
        StatusEffect.New(GetName(flatStatType, modType, value))
        .Mod(flatStatType, modType, value);

    public static StatusEffect Mod(MultStatType multStatType, StatType modType, float value) =>
         StatusEffect.New(GetName(multStatType, modType, value))
         .Mod(multStatType, modType, value);

    private static string GetName(FlatStatType flatStatType, StatType modType, float value)
    {
        bool isPositive = value >= FlatStat.NEUTRAL;
        return flatStatType switch
        {
            FlatStatType.MovementSpeed => isPositive ? "Haste" : "Slow",
            FlatStatType.JumpStrength => isPositive ? "JumpStrength" : "JumpWeakness",

            _ => throw EnumException.NoneOrNotDefined(nameof(flatStatType), flatStatType)
        }
        + ModToString(modType, value, isPositive);
    }

    private static string GetName(MultStatType multStatType, StatType modType, float value)
    {
        bool isPositive = value >= MultStat.NEUTRAL;
        return multStatType switch
        {
            MultStatType.None => throw new EnumNoneException(nameof(multStatType)),

            _ => throw new EnumNotDefinedException(nameof(multStatType), multStatType)
        }
        + ModToString(modType, value, isPositive);
    }

    private static string ModToString(StatType modType, float value, bool isPositive) => modType switch
    {
        StatType.Flat => isPositive ? " + " : " - ",
        StatType.Mult => " * ",

        _ => throw EnumException.NoneOrNotDefined(nameof(modType), modType)
    }
    + Math.Abs(value).ToString();
}
