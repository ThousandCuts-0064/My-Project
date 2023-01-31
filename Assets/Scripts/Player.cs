using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private Camera _camera;

    [SerializeField] private Character _character;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Instantiate(_character, transform)
            .AttachPlayer(this);
    }
}
