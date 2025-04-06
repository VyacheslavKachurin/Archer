using System;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelView : MonoBehaviour
{
    public event Action OnSkinSwitchClicked, OnToggleObstacleClicked, OnTargetsRestore;

    private VisualElement _root;
    private Button _obstacleBtn, _skinBtn, _targetsBtn;


    public void Awake()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _skinBtn = _root.Q<Button>("skin-btn");
        _obstacleBtn = _root.Q<Button>("obstacle-btn");
        _targetsBtn = _root.Q<Button>("targets-btn");

        _skinBtn.clicked += () => OnSkinSwitchClicked?.Invoke();
        _obstacleBtn.clicked += () => OnToggleObstacleClicked?.Invoke();
        _targetsBtn.clicked += () => OnTargetsRestore?.Invoke();

    }
}