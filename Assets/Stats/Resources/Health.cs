using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(Health), menuName = nameof(ScriptableObject) + "/" + nameof(Resource) + "/" + nameof(Health))]
public class Health : Resource
{
    public override Color Color => Color.red;
}
