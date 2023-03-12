using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterStats : Stats
{
    [field: SerializeField] public Stat MovementSpeed { get; private set; }
    [field: SerializeField] public Stat JumpStrength { get; private set; }
}
