using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Slowness : TemporaryEffect
{
    public Slowness(float duration, float baseValue) : base(duration) => 
        AddComponent(new MultModFlat(nameof(CharacterStats.MovementSpeed), baseValue));
}