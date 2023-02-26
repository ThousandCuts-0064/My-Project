using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Character : NetworkBehaviour
{
    private List<Collider> _feetColliders;
    private Player _player;
    private UICharacter _uiCharacter;
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
        _stats = Instantiate(_stats);
        _stats.Resources[0].CurrentChanged += DeathOnDepletion;
        _stats.Resources[0].MaxChanged += DeathOnDepletion;
    }

    private void FixedUpdate()
    {
        foreach (var resource in _stats.Resources)
            resource.Current += resource.Generation * Time.fixedDeltaTime;
        GameObject.Find("Canvas").transform.Find("TextDebug").GetComponent<TMPro.TMP_Text>().text = _stats.Resources[0].Current.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) return;

        if (collision.GetContact(0).point.y < transform.position.y - _collider.bounds.extents.y / 10)
            _feetColliders.Add(collision.collider);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!IsOwner) return;

        _feetColliders.Remove(collision.collider);
    }

    public void AttachPlayer(Player player)
    {
        _player = player;
        _player.transform.SetParent(_playerSlot.Slot.transform);
        _player.transform.localPosition = Vector3.zero;
        if (NetworkObject.OwnerClientId != _player.OwnerClientId)
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

    public bool TryMove(Vector3 direction)
    {
        if (_feetColliders.Count == 0) return false;

        if (new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.z).sqrMagnitude > _stats.MovementSpeed * _stats.MovementSpeed) return false;

        Vector3 relativeDir = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, transform.up) * direction.normalized * _stats.MovementSpeed;
        _rigidbody.velocity = new Vector3(relativeDir.x, _rigidbody.velocity.y, relativeDir.z);
        return true;
    }

    public bool TryJump()
    {
        if (_feetColliders.Count == 0) return false;

        Vector3 newVelocity = _rigidbody.velocity;
        newVelocity.y = Math.Max(newVelocity.y, _stats.JumpStrength);
        _rigidbody.velocity = newVelocity;
        return true;
    }

    public void SetUI(UICharacter uiCharacter)
    {
        _uiCharacter = uiCharacter;
        _uiCharacter.gameObject.SetActive(true);
        _uiCharacter.CharacterStats = _stats;
    }

    private void DeathOnDepletion(float value)
    {
        if (value < 0.001)
            NetworkObject.Despawn();
    }
}
