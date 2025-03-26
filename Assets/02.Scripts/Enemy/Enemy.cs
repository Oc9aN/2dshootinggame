using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum EnemyType
{
    Basic = 0,
    Target = 1,
    Follow = 2,
    Horizontal = 3,
    Bezier = 4,
    BasicBezier = 5
}

public abstract class Enemy : MonoBehaviour, IDamagable
{
    public event Action CreateBezierEvent;

    [SerializeField] protected EnemyDataSO _data;
    public float Speed;
    public int MaxHealth;
    public int Damage;
    
    public EnemyType Type => _data.Type;
    
    private int _health = 100;
    protected Vector2 _direction;
    public int Health => _health;

    protected GameObject _player;
    private Animator _animator;


    public abstract void StartAction();
    public abstract void UpdateAction();
    public abstract void SetType();

    public virtual void Initialize()
    {
        LevelDataSO levelData = LevelManager.instance.GetLevelData();
        Damage *= (int)levelData.DamageFactor;
        MaxHealth *= (int)levelData.HealthFactor;
        Speed *= (int)levelData.SpeedFactor;
        
        _health = MaxHealth;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        SetType();
    }

    private void Start()
    {
        StartAction();
    }

    // 매프레임마다 자동으로 호출되는 함수
    private void Update()
    {
        UpdateAction();
    }

    // 충돌 이벤트 함수
    // - Trigger 이벤트    : 물리 연산을 무시하지만, 충돌 이벤트를 받겠다.
    // - Collision 이벤트  : 물리 연산도 하고, 충돌 이벤트도 받겠다.

    // 충돌 시작, 충돌 중, 충돌 끝

    // 다른 콜라이더와 충돌이 일어났을때 자동으로 호출되는 함수
    private void OnTriggerEnter2D(Collider2D other) // Stay(충돌중), Exit(충돌끝)
    {
        // 플레이어와 충돌:
        if (!other.CompareTag("Player")) return;
        // 플레이어 체력이 0 이하일때만 죽인다.
        var player = other.GetComponent<Player>(); // 게임 오브젝트의 컴포넌트를 가져온다.

        // 묻지말고 시켜라!
        player.TakeDamage(new Damage(DamageType.Enemy, Damage, gameObject));

        // 나죽자
        gameObject.SetActive(false);
    }
    
    protected void SetDirection()
    {
        _player ??= GameObject.FindGameObjectWithTag("Player"); // 업데이트가 아니면 성능상 문제가 없다
        // 방향을 구한다. (target - me)
        _direction = _player.transform.position - transform.position;
        _direction.Normalize(); // 정규화
        
        var angle = Utility.GetDirectionDegAngle(_direction);
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }

    public void TakeDamage(Damage damage)
    {
        _health -= damage.DamageValue;
        
        _animator.SetTrigger("Hit");

        if (_health <= 0)
        {
            OnDeath(damage.Type);
            gameObject.SetActive(false);
        }
    }

    private void OnDeath(DamageType damageType)
    {
        CreateBezierEvent?.Invoke();
        CreateBezierEvent = null;

        Particle p = ParticlePool.Instance.Create(ParticleType.Explosion, transform.position);
        
        _player?.GetComponent<Player>().AddScore(_data.Score);
        
        if (damageType == DamageType.Bullet)
        {
            _player ??= GameObject.FindGameObjectWithTag("Player"); // 업데이트가 아니면 성능상 문제가 없다
            _player.GetComponent<Player>().KillEnemy();
        }

        // 리플레이의 경우 주어진 아이템을 생성하기 때문에 스킵
        if (CommandInvoker.Instance.IsReplaying) return;
        
        CreateItem();
    }

    // 아이템 생성은 Enemy에서 하는게 맞는지 고민해보기 (수정)
    // -> 30%확률로 ItemSpawner에게 요청
    private void CreateItem()
    {
        var percent = Random.Range(0f, 1f);
        if (percent <= 1f)
        {
            ItemSpawner.instance.RecordItemSpawn(transform.position);
        }
    }
}