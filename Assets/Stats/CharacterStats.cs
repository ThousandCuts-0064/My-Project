using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(CharacterStats), menuName = nameof(ScriptableObject) + "/" + nameof(Stats) + "/" + nameof(CharacterStats))]
public class CharacterStats : Stats
{
    [field: SerializeField] public Stat MovementSpeed { get; private set; }
    [field: SerializeField] public Stat JumpStrength { get; private set; }
}
