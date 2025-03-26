using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemMove : MonoBehaviour
{
    // 플레이어와 가까워지면 이동
    public float Threshold = 3f;
    public float Speed = 3f;

    private Vector3[] _path;
    private GameObject _player;
    private readonly List<Vector2> _points = new();
    private Sequence _sequence = null;
    private bool _snap;

    // 베지어 이동 시간
    private float _time;
    
    // 삭제 타이머
    public float TimeOut = 5f;
    private float _timer = 0f;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // 아이템 동작 (이동)
    private void Update()
    {
        Move();
        Remove();
    }

    private void Remove()
    {
        _timer += Time.deltaTime;

        if (_timer >= TimeOut)
        {
            Destroy(gameObject);
        }
    }

    private void DefaultMove()
    {
        transform.Translate(Vector2.down * (Speed * Time.deltaTime * 0.5f));
    }

    private void Move()
    {
        // 혹시 플레이어가 없으면 return
        if (!_player) return;

        var distance = Vector2.Distance(transform.position, _player.transform.position);
        // 거리가 멀면 아래로 이동
        if (distance > Threshold && !_snap)
        {
            return;
        }

        // 가까우면 다가옴
        Snap();
    }

    public void Snap()
    {
        _player ??= GameObject.FindGameObjectWithTag("Player");
        // 베지어로 아이템 흡수
        if (_points.Count <= 0)
        {
            // 제어점 설정
            Vector2 direction = (transform.position - _player.transform.position).normalized;
            var backPoint = (Vector2)transform.position + direction * Speed;

            // 시작점, 중간점 2개, 끝점
            _points.Add(transform.position);
            _points.Add(backPoint);
            _points.Add(_player.transform.position);
        }

        _points[2] = _player.transform.position;
        // 속도에 맞춰 시간 증가
        _time += Time.deltaTime * Speed;
        _time = Mathf.Clamp01(_time);

        var newPosition = Bezier.GetBezierValue(_time, _points);
        // 위치 이동
        transform.position = newPosition;

        _snap = true;
    }
}