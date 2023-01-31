using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Player _player;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void AttachPlayer(Player player)
    {
        _player = player;
        _player.transform.parent = transform.Find("PlayerSlot");
    }
}
