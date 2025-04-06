using UnityEngine;

public class SceneObjects : MonoBehaviour
{
    [SerializeField] private GameObject _obstacle;

    public void ToggleObstacle()
    {
        _obstacle.SetActive(!_obstacle.activeSelf);
    }
}