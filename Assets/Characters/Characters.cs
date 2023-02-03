using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    public static Characters _characters;

    [SerializeField] private Character _basic;

    public static Character Basic => _characters._basic;

    private void Awake()
    {
        _characters = this;
    }
}
