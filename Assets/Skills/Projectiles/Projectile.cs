using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    private Rigidbody _rigidbody;
    private float _spawnTime;
    public float LifeSeconds { get; private set; } = float.MaxValue;
    public float Damage { get; private set; } = float.MaxValue;

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
        if (!IsServer) return;

        if (collision.gameObject.TryGetComponent(out Stats stats))
            stats.TakeDamage(Element.Fire, 10);
    }

    public void Config(float lifeSeconds, float damage)
    {
        LifeSeconds = lifeSeconds;
        Damage = damage;
    }
}
