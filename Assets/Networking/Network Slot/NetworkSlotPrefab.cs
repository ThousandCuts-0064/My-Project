using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkSlotPrefab : MonoBehaviour
{
    [SerializeField] private NetworkObject _networkSlot;

    public static NetworkObject Get { get; private set; }

    private void Awake()
    {
        Get = _networkSlot;
    }
}
