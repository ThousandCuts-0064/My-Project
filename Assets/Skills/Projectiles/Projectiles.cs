using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    private static Projectiles _projectiles;

    [SerializeField] private Projectile _basic;

    public static Projectile Basic => _projectiles._basic;

    private void Awake()
    {
        _projectiles = this;
    }
}
