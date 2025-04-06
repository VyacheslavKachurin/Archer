
using System;
using Spine;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SkeletonAnimation _skeleton;
    [SerializeField][SpineSkin] private string _skin1Name, _skin2Name;


    [SpineAnimation]
    [SerializeField]
    private string _attackAnimation;
    private float _rotationOffset;

    private float _forceRadius;
    private float _explosionForce;

    void Start()
    {
        // _rotationOffset = _renderer.transform.rotation.eulerAngles.z;

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
        // _renderer.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _skeleton.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        _rb.velocity = Vector2.zero;
        _rb.simulated = false;
        _collider.enabled = false;
        Explode();

        _skeleton.AnimationState.SetAnimation(0, _attackAnimation, false);
        _skeleton.state.End += OnAnimationEnd;
    }

    private void Explode()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, _forceRadius);
        foreach (var hit in hits)
        {
            Rigidbody2D rb = hit.attachedRigidbody;
            if (rb != null)
            {
                Vector2 dir = (rb.position - (Vector2)transform.position).normalized;
                float dist = Vector2.Distance(rb.position, transform.position);
                float force = Mathf.Lerp(_explosionForce, 0, dist / _forceRadius);
                rb.AddForce(dir * force, ForceMode2D.Impulse);
            }
        }
    }

    private void OnAnimationEnd(TrackEntry trackEntry)
    {

        Destroy(gameObject);
    }

    internal void SetParams(float forceRadius, float explosionForce)
    {
        _forceRadius = forceRadius;
        _explosionForce = explosionForce;
    }

    public void SetSkin(int index)
    {
        var targetskin = index == 0 ? _skin1Name : _skin2Name;
        _skeleton.Skeleton.SetSkin(targetskin);
    }
}