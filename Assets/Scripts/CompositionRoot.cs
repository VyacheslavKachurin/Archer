using UnityEngine;

public class CompositionRoot : MonoBehaviour
{
  [Header("Dependencies")]
  [SerializeField] private InputController _inputController;
  [SerializeField] private PlayerController _playerController;
  [SerializeField] private TrajectoryProvider _trajectoryHandler;
  [SerializeField] private PlayerShooter _shooter;
  [SerializeField] private GunToAim _boneAim;
  [SerializeField] private LevelView _levelView;
  [SerializeField] private SceneObjects _sceneObjects;



  [SerializeField] private float _distanceThreshold = 0.2f;
  private IInputProvider _inputProvider;
  private SkinSwitch _skinSwitch;

  [Header("Game settings")]
  [SerializeField] private int _tracePositionsAmount;
  [SerializeField] private float _shootForce;

  private void Start()
  {
    _inputProvider = _inputController;

    _playerController.Inject(_inputProvider);

    _boneAim.Inject(_inputProvider);
    _boneAim.SetDistanceThreshold(_distanceThreshold);
    _playerController.OnGunShot += _shooter.Shoot;
    _shooter.ShootForce = _shootForce;
    _shooter.Inject(_inputProvider);

    InitTrajectoryHandler();

    _inputController.transform.position = _playerController.transform.position;
    _skinSwitch = new SkinSwitch(_playerController, _shooter);
    InitView();
  }

  private void InitView()
  {
    _levelView.OnSkinSwitchClicked += _skinSwitch.SwitchSkin;
    _levelView.OnToggleObstacleClicked += _sceneObjects.ToggleObstacle;
    _levelView.OnTargetsRestore += _sceneObjects.ResetTargets;
  }

  private void Update()
  {
    if (!_inputProvider.IsAiming) return;
    _trajectoryHandler.GunPoint = _playerController.GunPos;
  }

  private void InitTrajectoryHandler()
  {
    _trajectoryHandler.Inject(_inputProvider);
    _trajectoryHandler.CreatePoints(_tracePositionsAmount);
    _trajectoryHandler.SetDistanceThreshold(_distanceThreshold);
    _trajectoryHandler.SetShootForce(_shootForce);

  }
}