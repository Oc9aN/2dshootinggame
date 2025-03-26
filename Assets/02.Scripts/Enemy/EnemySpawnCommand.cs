using UnityEngine;

public class EnemySpawnCommand : ICommand
{
    private EnemySpawner _spawner;
    private EnemyType _type;
    private Vector2 _position;
    
    private readonly Vector2 _startRandomPoint;
    private Vector2 _endRandomPoint;

    public EnemySpawnCommand(EnemySpawner spawner, EnemyType type)
    {
        _spawner = spawner;
        _type = type;
    }

    // 베지어 용 스폰 커멘드를 따로 만드는게 좋을지도?
    public EnemySpawnCommand(EnemySpawner spawner, EnemyType type, Vector3 position, Vector2 startRandomPoint, Vector2 endRandomPoint)
    {
        _spawner = spawner;
        _type = type;
        _position = position;
        _startRandomPoint = startRandomPoint;
        _endRandomPoint = endRandomPoint;
    }

    public void Execute()
    {
        GameObject enemy = _spawner.Spawn(_type);

        if (_type == EnemyType.Bezier)
        {
            // Debug.Log(_position);
            enemy.transform.position = _position;
            enemy.GetComponent<Enemy_Bezier>().StartRandomPoint = _startRandomPoint;
            enemy.GetComponent<Enemy_Bezier>().EndRandomPoint = _endRandomPoint;
        }
    }
}