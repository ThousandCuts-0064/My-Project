using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComponents
{
    IFinishers Mod(FlatStatType flatStatType, StatType modType, float value);
    IFinishers Mod(MultStatType multStatType, StatType modType, float value);
    IFinishers OverTime(ResourceLayer resourceLayer, Element element, float value, out FlatStat flatStat);
}
