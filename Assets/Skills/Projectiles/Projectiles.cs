using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    private static Projectiles _projectiles;

    [SerializeField] private Projectile _projectile;

    public static Projectile Projectile => _projectiles._projectile;

    private void Awake()
    {
        _projectiles = this;
    }
}
