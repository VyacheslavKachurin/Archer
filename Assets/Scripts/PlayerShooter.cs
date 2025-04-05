using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Projectile _arrowPrefab;
    private float _shootForce;

    public float ShootForce { set => _shootForce = value; }
    private IInputProvider _inputProvider;

    public void Inject(IInputProvider inputProvider) => _inputProvider = inputProvider;


    public void Shoot(Vector2 origin, float rotation)
    {
        var projectile = Instantiate(_arrowPrefab, origin, Quaternion.Euler(0, 0, rotation));
        projectile.ApplyForce(_inputProvider.Direction * _shootForce);

    }
}