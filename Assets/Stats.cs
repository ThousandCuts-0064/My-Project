using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(Stats), menuName = nameof(ScriptableObject) + "/" + nameof(Stats))]
public class Stats : ScriptableObject
{
    [field: SerializeField] public int Health { get; private set; }

}
