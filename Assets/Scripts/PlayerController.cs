using System;
using Spine;
using Spine.Unity;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action<Vector2, float> OnGunShot;

    public Vector2 GunPos => _gunBone.GetWorldPosition(_skeletonAnimation.transform);

    [SerializeField] private SkeletonAnimation _skeletonAnimation;
    [SerializeField][SpineSkin] private string _skin1Name, _skin2Name;


    [SpineBone(dataField: "skeletonAnimation")]
    [SerializeField]
    private string _gunName, _arrowName;

    [SerializeField] private EventDataReferenceAsset _shootEvent;

    [Header("Animations")]

    [SpineAnimation]
    [SerializeField] private string _attackStart, _attackFinish, _aimToIdle;
    [SerializeField] private int _trackIndex = 0;
    [SerializeField] private float _cancelMixDurarion = 0.3f;


    private IInputProvider _inputProvider;
    private Spine.AnimationState _animState;
    private Bone _gunBone;
    private Bone _arrowBone;


    private void Start()
    {
        _animState = _skeletonAnimation.AnimationState;
        _gunBone = _skeletonAnimation.Skeleton.FindBone(_gunName);
        _arrowBone = _skeletonAnimation.Skeleton.FindBone(_arrowName);
        _skeletonAnimation.state.Event += HandleEvent;

    }

    private void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data == _shootEvent.EventData)
            OnGunShot?.Invoke(GunPos, _arrowBone.Rotation);
    }


    public void Inject(IInputProvider inputProvider)
    {
        _inputProvider = inputProvider;

        _inputProvider.OnInputStarted += AnimateAim;
        _inputProvider.OnInputStopped += AnimateShoot;
        _inputProvider.OnInputCancelled += PlayAimToIdle;

    }

    private void PlayAnimation(string animation, bool isLooping = false, float mixDuration = 0)
    {
        var trackEntry = _animState.SetAnimation(_trackIndex, animation, isLooping);
        trackEntry.MixDuration = mixDuration;
    }

    private void AnimateAim()
    {
        PlayAnimation(_attackStart);
    }

    [ContextMenu("Animate Shoot")]
    private void AnimateShoot()
    {
        PlayAnimation(_attackFinish);
    }

    [ContextMenu("PlayAimToIdle")]
    private void PlayAimToIdle()
    {
        PlayAnimation(_aimToIdle, true, _cancelMixDurarion);
    }

    public void SetSkin(int index)
    {
        var targetSkin = index == 0 ? _skin1Name : _skin2Name;
        _skeletonAnimation.Skeleton.SetSkin(targetSkin);
    }
}