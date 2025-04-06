using UnityEngine;

public class SkinSwitch
{
    private PlayerController _player;
    private PlayerShooter _shooter;

    private int _currentSkin = 0;

    public void SwitchSkin()
    {
        _currentSkin = _currentSkin == 0 ? 1 : 0;
        _player.SetSkin(_currentSkin);
        _shooter.SetSkin(_currentSkin);
    }

    public SkinSwitch(PlayerController player, PlayerShooter shooter)
    {
        _player = player;
        _shooter = shooter;
    }

}