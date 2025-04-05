using System;
using System.Collections.Generic;
using System.Text;
using Spine;
using UnityEngine;

public class TrajectoryProvider : MonoBehaviour
{
    public Vector2 GunPoint { set => _startPoint = value; }

    [SerializeField] private GameObject _pointPrefab;

    [SerializeField] private float _pointStartSize;
    [SerializeField] private float _sizeStep = 0.05f;
    [SerializeField] private float _timeOffset;
    [SerializeField] private float _timeStep;

    private IInputProvider _inputProvider;
    private Vector2 _startPoint;
    private List<GameObject> _points = new List<GameObject>();


    private Vector2 _shootForce;
    private float _minDistance;



    public void CreatePoints(int amount)
    {
        var currentPointSize = _pointStartSize;
        for (int i = 0; i < amount; i++)
        {
            var point = Instantiate(_pointPrefab);
            point.SetActive(false);
            point.transform.parent = transform;
            point.transform.localPosition = new Vector3(0, 0, 0);
            currentPointSize -= _sizeStep;
            var scale = Vector3.one * currentPointSize;
            point.transform.localScale = scale;
            _points.Add(point);
        }
    }

    private void Update()
    {
        if (!_inputProvider.IsAiming) return;
        if (_inputProvider.Distance < _minDistance || _inputProvider.FirstPos.x < _inputProvider.InputPos.x)
            HideTrajectory();
        else
            ShowTrajectory();
    }

    private void ShowTrajectory()
    {
        var tracePositions = CalculateTrajectory(_startPoint, _inputProvider.Direction);
        DisplayPoints(tracePositions);
    }


    private List<Vector2> CalculateTrajectory(Vector2 startPos, Vector2 velocity)
    {
        var tracePositions = new List<Vector2>();
        for (int i = 0; i < _points.Count; i++)
        {
            float t = (i + _timeOffset) * _timeStep;
            Vector2 pos = CalculatePositionAtTime(t, startPos, velocity * _shootForce);
            tracePositions.Add(pos);
        }
        return tracePositions;
    }

    private Vector2 CalculatePositionAtTime(float t, Vector2 startPos, Vector2 velocity)
    {
        float x = startPos.x + velocity.x * t;
        float y = startPos.y + velocity.y * t - 0.5f * Physics2D.gravity.magnitude * t * t;
        return new Vector2(x, y);
    }

    public void HideTrajectory()
    {
        foreach (var point in _points)
        {
            point.SetActive(false);
        }
    }


    public void DisplayPoints(List<Vector2> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            var point = _points[i];
            point.transform.position = positions[i];
            point.SetActive(true);
        }

    }

    internal void Inject(IInputProvider inputProvider)
    {
        _inputProvider = inputProvider;
        _inputProvider.OnInputStopped += HideTrajectory;
        _inputProvider.OnInputCancelled += HideTrajectory;
    }


    public void SetDistanceThreshold(float minDistance)
    {
        _minDistance = minDistance;
    }

    internal void SetShootForce(float shootForce)
    {
        _shootForce = new Vector2(shootForce, shootForce);
    }


}
