using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private Character _character;
    private Camera _camera;
    private float _mouseSensitivity = 5;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        if (!IsOwner) return;

        Cursor.lockState = CursorLockMode.Locked;
        name = $"{nameof(Player)} ({OwnerClientId})";
        _camera.enabled = true;
        AttachCharacterServerRpc();
    }

    private void Update()
    {
        if (!IsOwner) return;

        RotateCharacterServerRpc(new Vector3(-Input.GetAxis("Mouse Y") * _mouseSensitivity, Input.GetAxis("Mouse X") * _mouseSensitivity));
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
    private void RotateCharacterServerRpc(Vector3 rotDir)
    {
        _character.Rotate(rotDir);
    }

    [ServerRpc]
    private void MoveCharacterServerRpc(Vector3 moveDir)
    {
        _character.Move(moveDir);
    }
}
