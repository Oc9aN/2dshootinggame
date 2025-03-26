public class BossCreateCommand : ICommand
{
    private BossEvent _bossEvent;
    public BossCreateCommand(BossEvent bossEvent)
    {
        _bossEvent = bossEvent;
    }
    public void Execute()
    {
        _bossEvent.CreateBoss();
    }
}