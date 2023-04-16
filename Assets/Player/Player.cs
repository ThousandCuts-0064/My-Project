using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[DisallowMultipleComponent]
public class Player : NetworkBehaviour
{
    private Character _character;
    private Camera _camera;
    private UIStats _uiCharacter;
    private Vector3 _lookRotation;
    private bool _doJump;
    private bool _doFire1;
    [SerializeField, Range(0, 5)]private float _mouseSensitivity = 3;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _uiCharacter = FindObjectOfType<Canvas>().GetComponentInChildren<UIStats>(true);
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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                _uiCharacter.PanelsSetVisible(true);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                _uiCharacter.PanelsSetVisible(false);
            }
        }

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            _lookRotation += new Vector3(-Input.GetAxisRaw("Mouse Y") * _mouseSensitivity, Input.GetAxisRaw("Mouse X") * _mouseSensitivity);

            _character.SetLookRotation(_lookRotation);

            _doJump = Input.GetKey(KeyCode.Space);

            if (Input.GetKeyDown(KeyCode.Mouse0))
                _doFire1 = true;
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner || _character is null) return;

        Vector3 moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) moveDir.z += 1;
        if (Input.GetKey(KeyCode.S)) moveDir.z -= 1;
        if (Input.GetKey(KeyCode.D)) moveDir.x += 1;
        if (Input.GetKey(KeyCode.A)) moveDir.x -= 1;

        _character.TryMove(moveDir);

        if (_doJump)
        {
            _doJump = false;
            _character.TryJump();
        }

        if (_doFire1)
        {
            _doFire1 = false;
            _character.Fire1ServerRPC(_character.NetworkObjectId);
        }
    }

    [ServerRpc]
    private void AttachCharacterServerRpc()
    {
        _character = Instantiate(Characters.Basic, Vector3.up, Quaternion.identity);
        _character.SetUI(_uiCharacter);

        _character.NetworkObject.Spawn(true);
        _character.AttachPlayer(this);
        SendAttachedCharacterToClientsClientRpc(_character.NetworkObjectId);
    }

    [ClientRpc]
    private void SendAttachedCharacterToClientsClientRpc(ulong id)
    {
        _character = NetworkManager.SpawnManager.SpawnedObjects[id].GetComponent<Character>();
        _character.SetUI(_uiCharacter);
    }
}
