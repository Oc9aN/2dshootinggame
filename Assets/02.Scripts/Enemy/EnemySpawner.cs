using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    // 목표: 일정 시간마다 적을 내 위치에 생성하고 싶다.
    public enum SpawnerType
    {
        Horizontal,
        Vertical
    }

    // 필요 속성:
    // 일정 시간
    public float IntervalTime = 1f;

    // 스포너 타입
    public SpawnerType spawnerType;

    // 현재 시간
    private float _currentTimer;

    private void Start()
    {
        CommandInvoker.Instance.OnReplay += Replay;
    }

    private void Update()
    {
        // 1. 시간이 흐르다가
        _currentTimer += Time.deltaTime;

        // 2. 만약 스폰 타임이 된다면
        if (_currentTimer >= IntervalTime)
        {
            _currentTimer = 0f;

            // IntervalTime = 1 ~ 3;
            IntervalTime = Random.Range(0.5f, 12f / Mathf.Min(4f, Time.time));
            // 의사난수알고리즘
            // 1) 시드가 같아야한다.
            // 2) 알고리즘이 같아야한다.   Random.Range, C#, C++ STL 여러 가지 랜덤 엔진
            // 3) 호출 횟수가 같아야한다.

            // 3. 스폰을 한다.
            RecordSpawn();
        }
    }

    private void RecordSpawn()
    {
        EnemyType type = EnemyType.Basic;
        var percent = Random.Range(0f, 1f);
        switch (spawnerType)
        {
            case SpawnerType.Horizontal:
            {
                type = EnemyType.Horizontal;
                break;
            }

            case SpawnerType.Vertical:
            {
                if (percent <= 0.3f) // 30%
                {
                    type = EnemyType.Basic;
                }
                else if (percent <= 0.55f) // 25%
                {
                    type = EnemyType.Target;
                }
                else if (percent <= 0.80f) // 25%
                {
                    type = EnemyType.Follow;
                }
                else
                {
                    type = EnemyType.BasicBezier;
                }
                break;
            }
        }
        ICommand command = new EnemySpawnCommand(this, type);
        CommandInvoker.Instance.ExecuteCommand(command);
    }

    public GameObject Spawn(EnemyType type)
    {
        Enemy enemyComponent = EnemyPool.Instance.Create(type, transform.position);
        GameObject enemy = enemyComponent.gameObject;

        // 이벤트 등록
        // 베지어 생성 이벤트 등록
        if (type == EnemyType.BasicBezier)
        {
            enemyComponent.CreateBezierEvent += () =>
            {
                if (CommandInvoker.Instance.IsReplaying)
                    return;
                // 3마리 생성
                CreateBezier(enemyComponent.transform.position);
            };
        }

        return enemy;
    }

    private void CreateBezier(Vector2 position)
    {
        // 3마리 생성
        for (int i = 0; i < 3; i++)
        {
            var startRandomPoint = (Vector2)transform.position + Random.Range(-5f, 5f) * Vector2.right +
                                   Random.Range(-3f, -2f) * Vector2.up;
            var player = GameObject.FindWithTag("Player");
            var endRandomPoint = (Vector2)player.transform.position + Random.Range(-2f, 2f) * Vector2.right +
                                 Random.Range(1f, 2f) * Vector2.up;
            ICommand command = new EnemySpawnCommand(this, EnemyType.Bezier,  position
            , startRandomPoint, endRandomPoint);
            CommandInvoker.Instance.ExecuteCommand(command);
        }
    }

    private void Replay()
    {
        EnemyPool.Instance.AllDestroy();

        gameObject.SetActive(false);
    }
}