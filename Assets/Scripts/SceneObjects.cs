using UnityEngine;

public class SceneObjects : MonoBehaviour
{
    [SerializeField] private GameObject _obstacle;
    [SerializeField] private GameObject _targetsPrefab;
    [SerializeField] private GameObject _sceneTargets;
    private Vector2 _targetsPos;


    private void Start()
    {
        _targetsPos = _sceneTargets.transform.position;
    }

    public void ResetTargets()
    {
        Destroy(_sceneTargets);
        _sceneTargets = Instantiate(_targetsPrefab, _targetsPos, Quaternion.identity);
    }

    public void ToggleObstacle()
    {
        _obstacle.SetActive(!_obstacle.activeSelf);
    }
}