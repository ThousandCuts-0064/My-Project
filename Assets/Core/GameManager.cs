using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action UpdateEvent;
    public static event Action FixedUpdateEvent;

    [field: SerializeField] public GameObject[] ForceAwakes { get; private set; }

    private void Awake()
    {
        for (int i = 0; i < ForceAwakes.Length; i++)
            ForceAwakes[i].SetActive(true);
    }

    private void Update()
    {
        UpdateEvent?.Invoke();
    }

    private void FixedUpdate()
    {
        FixedUpdateEvent?.Invoke();
    }
}
