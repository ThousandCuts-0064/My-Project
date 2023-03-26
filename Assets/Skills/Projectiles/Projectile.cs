using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    private Rigidbody _rigidbody;
    private float _spawnTime;
    public StatusEffect StatusEffect { get; set; }
    public float LifeSeconds { get; set; } = 10;
    public float Damage { get; set; }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (!IsServer) return;

        _spawnTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (!IsServer) return;

        if (Time.time - _spawnTime >= LifeSeconds)
            NetworkObject.Despawn();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsServer 
            || !collision.gameObject.TryGetComponent(out Stats stats))
            return;

        stats.TakeDamage(Element.Fire, 10);
        stats.TryApplyStatusEffect(StatusEffect);
    }
}
