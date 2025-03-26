using UnityEngine;

public class PlayerFireCommand: ICommand
{
    private PlayerFire _player;

    public PlayerFireCommand(PlayerFire player)
    {
        _player = player;
    }
    
    public void Execute()
    {
        _player.Fire();
    }
}