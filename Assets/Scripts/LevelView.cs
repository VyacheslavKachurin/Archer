using System;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelView : MonoBehaviour
{
    private VisualElement _root;
    private Button _obstacleBtn, _skinBtn;

    public event Action OnSkinSwitchClicked;
    public event Action OnToggleObstacleClicked;

    public void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _skinBtn = _root.Q<Button>("skin-btn");
        _obstacleBtn = _root.Q<Button>("obstacle-btn");

        _skinBtn.clicked += () => OnSkinSwitchClicked?.Invoke();
        _obstacleBtn.clicked += () => OnToggleObstacleClicked?.Invoke();

    }
}