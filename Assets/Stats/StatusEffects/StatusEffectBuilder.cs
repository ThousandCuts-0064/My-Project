using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class StatusEffect
{
    public class Builder : IComponents, IFinishers
    {
        private static readonly Builder _singleton = new();
        private _Component _component;
        private _Finisher _finisher;
        private string _name;

        public static Builder ClearSingleton => _singleton.Clear();

        public IComponents Name(string name)
        {
            _name = name;
            return this;
        }

        private StatusEffect Build()
        {
            StatusEffect statusEffect = new(_name);
            statusEffect._components = new List<_Component>() { _component };
            if (_finisher is not null)
                statusEffect._finishers = new List<_Finisher>() { _finisher };

            Clear();
            return statusEffect;
        }

        private Builder Clear()
        {
            _component = null;
            _finisher = null;
            _name = null;
            return this;
        }


        IFinishers IComponents.Mod(FlatStatType flatStatType, StatType modType, float value)
        {
            _component = modType switch
            {
                StatType.Flat => new _FlatModFlat(flatStatType, value),
                StatType.Mult => new _MultModFlat(flatStatType, value),

                _ => throw EnumException.NoneOrNotDefined(nameof(modType), modType)
            };
            return this;
        }

        IFinishers IComponents.Mod(MultStatType multStatType, StatType modType, float value)
        {
            _component = modType switch
            {
                StatType.Flat => new _FlatModMult(multStatType, value),
                StatType.Mult => new _MultModMult(multStatType, value),

                _ => throw EnumException.NoneOrNotDefined(nameof(modType), modType)
            };
            return this;
        }

        IFinishers IComponents.OverTime(ResourceLayer resourceLayer, Element element, float value, out FlatStat flatStat)
        {
            _component = new _ResourceOverTime(resourceLayer, element, value, out flatStat);
            return this;
        }


        StatusEffect IFinishers.Timer(float time)
        {
            _finisher = new _Timer(time);
            return Build();
        }

        StatusEffect IFinishers.Permanent() => Build();
    }
}
