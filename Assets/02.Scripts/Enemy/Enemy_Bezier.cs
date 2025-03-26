using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bezier : Enemy
{
    // TODO: 리팩토링 필요
    public Vector2 StartRandomPoint;
    public Vector2 EndRandomPoint;
    
    private readonly List<Vector2> _points = new();
    private float _time;

    public override void Initialize()
    {
        base.Initialize();
        _time = 0f;
    }

    public override void StartAction()
    {
        if (!CommandInvoker.Instance.IsReplaying)
        {
            if (_player == null) _player = GameObject.FindGameObjectWithTag("Player"); // 업데이트가 아니면 성능상 문제가 없다
            // 제어점 설정
        
            // 시작점, 중간점 2개, 끝점
            _points.Add(transform.position);
            _points.Add(StartRandomPoint);
            _points.Add(EndRandomPoint);
            _points.Add(_player.transform.position);
        
            // 속도에 맞춰 시간 증가
            _time += Time.deltaTime * Speed;
            _time = Mathf.Clamp01(_time);
        
            var newPosition = Bezier.GetBezierValue(_time, _points);
        
            var direction = (newPosition - (Vector2)transform.position).normalized;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 라디안을 도로 변환
            transform.rotation = Quaternion.Euler(0, 0, angle + 90f); // Z축 회전 적용
            // 위치 이동
            transform.position = newPosition;
        }
        else
        {
            _player ??= GameObject.FindGameObjectWithTag("Player");
            // 자동으로 셋팅된 위치로 이동
            // 시작점, 중간점 2개, 끝점
            _points.Add(transform.position);
            _points.Add(StartRandomPoint);
            _points.Add(EndRandomPoint);
            _points.Add(_player.transform.position);
        }
    }

    public override void UpdateAction()
    {
        _player ??= GameObject.FindGameObjectWithTag("Player"); // 업데이트가 아니면 성능상 문제가 없다
        _points[3] = _player.transform.position;
        _time += Time.deltaTime * Speed;
        _time = Mathf.Clamp01(_time);
        var newPosition = Bezier.GetBezierValue(_time, _points);
        var direction = (newPosition - (Vector2)transform.position).normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 라디안을 도로 변환
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
        // 위치 이동
        transform.position = newPosition;
    }

    public override void SetType()
    {
        _data.Type = EnemyType.Bezier;
    }
}