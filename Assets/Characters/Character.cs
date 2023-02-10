using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Character : NetworkBehaviour
{
    private List<Collider> _feetColliders;
    private Player _player;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private NetworkSlot _playerSlot;
    [SerializeField] private CharacterStats _stats;

    private void Awake()
    {
        _feetColliders = new List<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _playerSlot = transform.Find("PlayerSlot").GetComponent<NetworkSlot>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.GetContact(0).point.y < transform.position.y - _collider.bounds.extents.y / 10)
            _feetColliders.Add(collision.collider);
    }

    private void OnCollisionExit(Collision collision)
    {
        _feetColliders.Remove(collision.collider);
    }

    public void AttachPlayer(Player player)
    {
        _player = player;
        _player.transform.SetParent(_playerSlot.Slot.transform);
        _player.transform.localPosition = Vector3.zero;
        NetworkObject.ChangeOwnership(_player.OwnerClientId);
        name = $"{nameof(Character)} ({_player.OwnerClientId})";
    }

    public void SetLookRotation(Vector3 rotation)
    {
        Vector3 currSlotRot = _playerSlot.transform.rotation.eulerAngles;
        _playerSlot.transform.rotation = Quaternion.Euler(rotation.x, currSlotRot.y, rotation.z);

        Vector3 currRot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currRot.x, rotation.y, currRot.z);
    }

    public void Move(Vector3 offset)
    {
        _rigidbody.MovePosition(transform.position + Quaternion.AngleAxis(transform.rotation.eulerAngles.y, transform.up) * offset.normalized * Time.fixedDeltaTime);
    }

    public bool TryJump()
    {
        if (_feetColliders.Count == 0) return false;

        _feetColliders.Clear();
        _rigidbody.AddForce(transform.up * _stats.JumpStrength, ForceMode.Impulse);
        return true;
    }
}
