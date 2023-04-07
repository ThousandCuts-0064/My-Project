using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Slowness : TemporaryEffect
{
    public Slowness(float duration) : base(duration) 
    {
        AddComponent(new MultModFlat());
    }
}