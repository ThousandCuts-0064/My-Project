using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Character : NetworkBehaviour
{
    private Player _player;
    private Rigidbody _rigidbody;
    private NetworkObject _playerSlot;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void AttachPlayer(Player player)
    {
        _player = player;
        _player.transform.SetParent(_playerSlot.transform);
        _player.transform.localPosition = Vector3.zero;
    }

    public void Move(Vector3 offset)
    {
        _rigidbody.MovePosition(_rigidbody.position + offset * Time.fixedDeltaTime);
    }
}
