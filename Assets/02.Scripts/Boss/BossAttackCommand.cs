public class BossAttackCommand : ICommand
{
    private BossObject _boss;
    private IBossAttackStrategy _strategy;

    public BossAttackCommand(BossObject boss, IBossAttackStrategy strategy)
    {
        _boss = boss;
        _strategy = strategy;
    }

    public void Execute()
    {
        _boss.Attack(_strategy);
    }
}