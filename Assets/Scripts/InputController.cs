using System;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class InputController : MonoBehaviour, IInputProvider
{
    public event Action OnInputStarted;
    public event Action OnInputStopped;
    public event Action OnInputCancelled;


    [SerializeField] private Camera _cam;
    [SerializeField] private float _minDistance = 0.3f;
    [SerializeField] private float _maxLength = 5f;

    public Vector2 InputPos => _inputPos;
    public Vector2 FirstPos => _firstPos;
    public float Distance => _distance;
    public Vector2 Direction => Vector2.ClampMagnitude(_direction, _maxLength);
    public float Angle => _angle;
    public bool IsAiming => _isPressed;

    private Vector2 _inputPos;
    private Vector2 _direction;
    private Vector2 _firstPos;
    private bool _isPressed;
    private float _distance;
    private float _angle;


    private void OnMouseDown()
    {
        _isPressed = true;
        _firstPos = _cam.ScreenToWorldPoint(Input.mousePosition);
        OnInputStarted?.Invoke();
    }

    private void Update()
    {
        if (!_isPressed) return;
        _inputPos = _cam.ScreenToWorldPoint(Input.mousePosition);
        _direction = _firstPos - _inputPos;
        _distance = Vector2.Distance(_firstPos, _inputPos);
        _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
    }

    private void OnMouseUp()
    {
        _isPressed = false;
        if (_distance > _minDistance && _firstPos.x > _inputPos.x) OnInputStopped?.Invoke();
        else OnInputCancelled?.Invoke();

    }
}

public interface IInputProvider
{
    event Action OnInputStarted;
    event Action OnInputStopped;
    event Action OnInputCancelled;


    Vector2 InputPos { get; }
    Vector2 FirstPos { get; }
    float Distance { get; }
    Vector2 Direction { get; }
    float Angle { get; }
    bool IsAiming { get; }
}