using UnityEngine;

public class PlayerMoveCommand : ICommand
{
    private PlayerMove _player;
    private Vector2 _direction;

    public PlayerMoveCommand(PlayerMove player, Vector2 direction)
    {
        _player = player;
        _direction = direction;
    }
    
    public void Execute()
    {
        _player.Direction = _direction;
    }
}