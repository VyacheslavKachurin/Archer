
using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class Projectile : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _renderer;

    private float _rotationOffset;

    void Start()
    {
        _rotationOffset = _renderer.transform.rotation.eulerAngles.z;
    }

    public void ApplyForce(Vector2 force)
    {
        _rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (_rb.velocity == Vector2.zero) return;
        RotateTowardsFalling();
    }

    private void RotateTowardsFalling()
    {
        float angle = Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg;
        angle += _rotationOffset;
        _renderer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

}