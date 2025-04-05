


using System;
using Spine;
using Spine.Unity;
using UnityEngine;


public class GunToAim : MonoBehaviour
{

    [SerializeField] private SkeletonAnimation _skeletonAnimation;

    [SpineBone(dataField: "skeletonAnimation")]
    [SerializeField]
    private string _boneName;
    [SerializeField] private Bone _bone;
    [SerializeField] private float _interpolalitonSpeed;


    private IInputProvider _inputProvider;
    private float _distanceThreshold;


    void Start()
    {
        _bone = _skeletonAnimation.Skeleton.FindBone(_boneName);
    }

    public void Inject(IInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
    }

    void Update()
    {
        if (!_inputProvider.IsAiming) return;
        Aim();
    }

    private void Aim()
    {
        var firstPos = _inputProvider.FirstPos;
        var inputPos = _inputProvider.InputPos;
        var distance = Vector2.Distance(firstPos, inputPos);

        if (distance < _distanceThreshold || firstPos.x < inputPos.x) return;

        _bone.Rotation = Mathf.LerpAngle(_bone.Rotation, _inputProvider.Angle, Time.deltaTime * _interpolalitonSpeed);

    }


    internal void SetDistanceThreshold(float distanceThreshold)
    {
        _distanceThreshold = distanceThreshold;
    }
}


