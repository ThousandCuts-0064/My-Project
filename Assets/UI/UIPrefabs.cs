using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPrefabs : MonoBehaviour
{
    public static Bar Bar { get; private set; }
    [SerializeField] private Bar _bar;

    private void Awake()
    {
        Bar = _bar;
    }
}
