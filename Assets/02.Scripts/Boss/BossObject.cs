using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class PatternSet
{
    public List<BossAttackPatternCricleData> PatternDatas;
}

public class BossObject : MonoBehaviour, IDamagable
{
    [SerializeField] private int _fullHealth = 1000;
    private int _health = 1000;
    public int Health => _health;

    private float _attackSpeed = 3f; // 공격 속도
    private float _attackTimer = 0f;

    [SerializeField] private List<PatternSet> _patternSets;

    [SerializeField] private List<BossAttackPatternData> _patternDatas = new();
    [SerializeField] private List<BossAttackPatternCricleData> _patternAngleDatas = new();

    private Animator _animator;
    
    public event Action OnDeath;

    // 총알 갯수
    // 각도 (최대, 최소)
    // 계산 공식
    // 총알 생성 딜레이

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        CommandInvoker.Instance.OnReplay += Replay;
    }

    public void Initialize()
    {
        _health = _fullHealth;
        
        UI_Game.Instance.ActiveBossHealth(_health);
    }

    private void Update()
    {
        if (_health <= 0 || CommandInvoker.Instance.IsReplaying) return;
        
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _attackSpeed)
        {
            RecordAttack();
            _attackTimer = 0f;
            float hpPercent = _health / (float)_fullHealth;
            if (hpPercent <= 0.3f)
            {
                _attackSpeed = Random.Range(1f, 2f);
            }
        }
    }

    [ContextMenu("치트")]
    private void Cheat()
    {
        _health = _fullHealth / 2;
    }

    private void RecordAttack()
    {
        float hpPercent = _health / (float)_fullHealth;
        if (hpPercent >= 0.7f)
        {
            BossAttackStrategy_Circle attack = new BossAttackStrategy_Circle(_patternAngleDatas[0]);
            AttackCommand(attack);
        }
        else if (hpPercent >= 0.3f)
        {
            // 패턴대로 행동
            foreach (var pattern in _patternSets[Random.Range(0, _patternSets.Count)].PatternDatas)
            {
                BossAttackStrategy_Circle attack = new BossAttackStrategy_Circle(pattern);
                AttackCommand(attack);
            }
        }
        else
        {
            // 타겟으로 하는 패턴과 같이
            BossAttackStrategy_Circle attack1 =
                new BossAttackStrategy_Circle(_patternAngleDatas[Random.Range(0, _patternAngleDatas.Count)]);
            AttackCommand(attack1);

            BossAttackStrategy_Target attack2 =
                new BossAttackStrategy_Target(_patternDatas[Random.Range(0, _patternDatas.Count)]);
            AttackCommand(attack2);
        }
    }

    private void AttackCommand(IBossAttackStrategy strategy)
    {
        ICommand command = new BossAttackCommand(this, strategy);
        CommandInvoker.Instance.ExecuteCommand(command);
    }
    
    public void Attack(IBossAttackStrategy strategy)
    {
        StartCoroutine(strategy.CreateBullet(transform.position));
    }

    public void TakeDamage(Damage damage)
    {
        if (_health <= 0) return;
        
        _health -= damage.DamageValue;

        UI_Game.Instance.RefreshBossHealth(_health);

        _animator.SetTrigger("Hit");

        if (_health <= 0)
        {
            Debug.Log("사망");
            StopAllCoroutines();
            // 죽음 이벤트
            OnDeath?.Invoke();
        }
    }

    private void Replay()
    {
        StopAllCoroutines();
        BossBulletPool.Instance.AllDestroy();
        
        gameObject.SetActive(false);
    }
}