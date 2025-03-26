public class PlayerBoomCommand : ICommand
{
    private PlayerBoom _player;

    public PlayerBoomCommand(PlayerBoom player)
    {
        _player = player;
    }
    public void Execute()
    {
        _player.Boom();
    }
}