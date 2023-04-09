using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class StatusEffectFromComponents : StatusEffect
{
    private protected StatusEffectFromComponents(Component component) => AddComponent(component);
}
