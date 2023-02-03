using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private Camera _camera;

    private Character _character;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        if (!IsOwner) return;

        AttachCharacterServerRpc();
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        Vector3 moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) moveDir.z += 1;
        if (Input.GetKey(KeyCode.S)) moveDir.z -= 1;
        if (Input.GetKey(KeyCode.D)) moveDir.x += 1;
        if (Input.GetKey(KeyCode.A)) moveDir.x -= 1;

        MoveCharacterServerRpc(moveDir);
    }

    [ServerRpc]
    private void AttachCharacterServerRpc()
    {
        _character = Instantiate(Characters.Basic, Vector3.up, Quaternion.identity);
        _character.NetworkObject.Spawn(true);
        _character.AttachPlayer(this);
    }

    [ServerRpc]
    private void MoveCharacterServerRpc(Vector3 moveDir)
    {
        _character.Move(moveDir);
    }
}
