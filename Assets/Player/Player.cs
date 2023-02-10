using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private Character _character;
    private Camera _camera;
    private float _mouseSensitivity = 3;
    private Vector3 _lookRotation;
    private bool _doJump;

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
        if (!IsOwner || _character is null) return;

        _lookRotation += new Vector3(-Input.GetAxisRaw("Mouse Y") * _mouseSensitivity, Input.GetAxisRaw("Mouse X") * _mouseSensitivity);

        _character.SetLookRotation(_lookRotation);

        _doJump = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        if (!IsOwner || _character is null) return;

        Vector3 moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) moveDir.z += 1;
        if (Input.GetKey(KeyCode.S)) moveDir.z -= 1;
        if (Input.GetKey(KeyCode.D)) moveDir.x += 1;
        if (Input.GetKey(KeyCode.A)) moveDir.x -= 1;

        _character.Move(moveDir);

        if (_doJump)
        {
            _doJump = false;
            _character.TryJump();
        }
    }

    [ServerRpc]
    private void AttachCharacterServerRpc()
    {
        _character = Instantiate(Characters.Basic, Vector3.up, Quaternion.identity);
        _character.NetworkObject.Spawn(true);
        _character.AttachPlayer(this);
        SendAttachedCharacterToClientsClientRpc(_character.NetworkObjectId);
    }

    [ClientRpc]
    private void SendAttachedCharacterToClientsClientRpc(ulong id)
    {
        _character = NetworkManager.SpawnManager.SpawnedObjects[id].GetComponent<Character>();
    }
}
