using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Character : NetworkBehaviour
{
    private Player _player;
    private Rigidbody _rigidbody;
    private NetworkSlot _playerSlot;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerSlot = transform.Find("PlayerSlot").GetComponent<NetworkSlot>();
    }

    public void AttachPlayer(Player player)
    {
        _player = player;
        _player.transform.SetParent(_playerSlot.Slot.transform);
        _player.transform.localPosition = Vector3.zero;
        name = $"{nameof(Character)} ({_player.OwnerClientId})";
    }

    public void Rotate(Vector3 offset) 
    {
        _rigidbody.MoveRotation(Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, offset.y, 0)));
        _playerSlot.transform.Rotate(new Vector3(offset.x, 0, 0));
    }

    public void Move(Vector3 offset)
    {
        _rigidbody.MovePosition(transform.position + offset * Time.fixedDeltaTime);
    }
}
