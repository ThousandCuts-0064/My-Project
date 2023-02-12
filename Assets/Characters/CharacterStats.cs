using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(CharacterStats), menuName = nameof(ScriptableObject) + "/" + nameof(CharacterStats))]
public class CharacterStats : Stats
{
    [field: SerializeField] public float MovementSpeed { get; private set; }
    [field: SerializeField] public float JumpStrength { get; private set; }
}
