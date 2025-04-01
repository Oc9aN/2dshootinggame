using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FollowerAttack : MonoBehaviour
{
    private GameObject _target;
    private List<Vector2> _points = new();

    public int SegmentCount = 30;
    private LineRenderer _lineRenderer;

    public float CoolTime = 2f;
    private float _coolTimer = 0f;
    public float AttackTime = 1f;
    private float _attackTimer = 0f;
    private bool _isAttacking = false;

    public float AttackDelay = 0.25f;
    public int Damage = 1;
    private float _attackDelayTimer = 0f;

    public GameObject EffectObject;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        // 쿨타임 중 리턴
        if (_coolTimer < CoolTime && !_isAttacking)
        {
            _coolTimer += Time.deltaTime;
            return;
        }
        Attack();
    }

    private void Findtarget()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var minDistance = Mathf.Infinity;
        foreach (var enemy in enemies)
        {
            if (!enemy.activeInHierarchy)
                continue;
            // 거리 체크
            var distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                _target = enemy;
            }
        }
    }

    private void DrawRazer()
    {
        // 제어점 설정

        // 시작점, 중간점 2개, 끝점
        if (_points.Count <= 0)
        {
            _points.Add(transform.position);
            _points.Add((Vector2)((transform.position + _target.transform.position) / 2) + Random.insideUnitCircle * 2f);
            _points.Add(_target.transform.position);
        }
        else
        {
            _points[0] = transform.position;
            _points[_points.Count - 1] = _target.transform.position;
        }
        
        _lineRenderer.positionCount = SegmentCount + 1;

        // 속도에 맞춰 시간 증가
        for (int i = 0; i <= SegmentCount; i++)
        {
            float t = i / (float)SegmentCount; // 0 ~ 1 사이 값
            var position = Bezier.GetBezierValue(t, _points);
            _lineRenderer.SetPosition(i, position);
        }
    }

    private void Attack()
    {
        // 타겟이 없거나 비활성이면 다른 타겟 탐색
        if (!_target || !_target.activeInHierarchy)
        {
            _target = null;
            _lineRenderer.positionCount = 0;
            AttackOff();
            Findtarget();
        }
        // 탐색 후에도 없거나 체력이 없으면 공격 안함
        if (!_target || _target.GetComponent<IDamagable>().Health <= 0)
        {
            AttackOff();
            return;
        }

        // 공격중
        EffectOn();
        
        _isAttacking = true;
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackTime)
        {
            // 공격 끝나면 coolTime 초기화
            AttackOff();
            return;
        }
        DrawRazer();
        
        _attackDelayTimer += Time.deltaTime;;
        if (_attackDelayTimer >= AttackDelay)
        {
            _attackDelayTimer = 0f;
            _target.GetComponent<IDamagable>().TakeDamage(new Damage(DamageType.Bullet, Damage, gameObject));
            _points[1] = (Vector2)((transform.position + _target.transform.position) / 2) + Random.insideUnitCircle * 2f;
            if (_target.GetComponent<IDamagable>().Health <= 0)
            {
                AttackOff();
            }
        }
    }

    private void EffectOn()
    {
        if (!_target) return;
        
        EffectObject.transform.position = _target.transform.position;
        
        // 꺼진 경우 활성화
        if (!EffectObject.activeSelf)
        {
            EffectObject.SetActive(true);
        }
    }

    private void AttackOff()
    {
        _isAttacking = false;
        _attackTimer = 0f;
        _coolTimer = 0f;
        CoolTime = Random.Range(0.8f, 1.6f);
        _lineRenderer.positionCount = 0;
        EffectObject.SetActive(false);
    }
}