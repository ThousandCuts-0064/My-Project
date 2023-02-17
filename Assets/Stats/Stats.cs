using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(Stats), menuName = nameof(ScriptableObject) + "/" + nameof(Stats) + "/" + nameof(Stats))]
public class Stats : ScriptableObject, IReadOnlyStats
{
    [field: SerializeReference] public List<Resource> Resources { get; private set; }
    IReadOnlyList<IReadOnlyResource> IReadOnlyStats.Resources => Resources;

    private void Awake()
    {
        for (int i = 0; i < Resources.Count; i++)
            Resources[i] = Instantiate(Resources[i]);
    }
}
