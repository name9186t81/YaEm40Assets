using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplostionProjectile : MonoBehaviour
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private Explosion _explosion;

    private void Start()
    {
        _projectile.OnHit += Hit;
    }

    private void Hit(Transform arg1, Vector2 arg2, Vector2 arg3)
    {
        _explosion.Init(_projectile.Attackr);
    }
}
