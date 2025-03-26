using System;
using UnityEngine;

public enum TargetMode
{
    Closest,
    Farthest,
    HealthLow
}

public class PlayerMove : PlayerComponent
{
    private const float THRESHOLD = 0.05f;

    public float MinX, MaxX;
    public float MinY, MaxY;

    private Animator _myAnimator;

    // 자동 모드 타겟
    private GameObject _target;

    // 타겟 탐색 방식
    private AutoTargetFindStrategy findStrategy;

    // 리플레이 Direction
    private Vector2 _direction = Vector2.zero;  // 현재 이동방향
    private Vector2 _defalutPosition = Vector2.zero;

    public Vector2 Direction
    {
        get => _direction;
        set => _direction = value;
    }

    // Start보다 먼저 호출되며 인스턴스화 된 직후 호출
    protected override void Awake()
    {
        base.Awake();
        _myAnimator = GetComponent<Animator>();
        
        _defalutPosition = transform.position;
    }

    private void Start()
    {
        // 리플레이 리셋 이벤트 등록
        CommandInvoker.Instance.OnReplay += Replay;
        // 타겟 탐색 방식 셋팅
        findStrategy = new AutoTargetFindStrategy();
        switch (_player.TargetMode)
        {
            case TargetMode.Closest: // 가장 가까운 적
            {
                findStrategy.SetStrategy(new ClosestTargetFindStrategy());
                break;
            }
            case TargetMode.Farthest: // 가장 먼 적
            {
                findStrategy.SetStrategy(new FarthestTargetFindStrategy());
                break;
            }
            case TargetMode.HealthLow: // 체력이 적은 적
            {
                findStrategy.SetStrategy(new HealthLowTargetFindStrategy());
                break;
            }
        }
    }


    private void Update()
    {
        // 키 입력 검사
        if (Input.GetKeyDown(KeyCode.Alpha1))
            _player.PlayMode = PlayMode.Auto;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) _player.PlayMode = PlayMode.Mannual;


        //SpeedCheck();

        if (_player.PlayMode == PlayMode.Mannual)
            ManualMove();
        else if (_player.PlayMode == PlayMode.Auto)
            AutoMove();

        // 리플레이 모드이면 방향에 따라 업데이트로 계속 움직임
        // 위치 제한을 하면서 움직임
        MoveByDirection();
    }


    private void AutoMove()
    {
        findStrategy.ExecuteStrategy(transform.position, ref _target);
        // 타겟이 없거나 비활성화인 경우 return
        if (!_target || !_target.activeInHierarchy)
        {
            _direction = Vector2.zero;
            return;
        }

        // 적 위치에 따라 방향 계산
        var direction = Vector2.zero;
        // x축 계산
        if (Mathf.Abs(_target.transform.position.x - transform.position.x) > THRESHOLD)
        {
            if (_target.transform.position.x < transform.position.x)
                direction += Vector2.left;
            else
                direction += Vector2.right;
        }

        // y축 계산
        var targetDistance = Vector2.Distance(transform.position, _target.transform.position);
        if (targetDistance < 3f)
            direction += Vector2.down;
        else if (targetDistance > 5f) direction += Vector2.up;

        RecordMovement(direction);  // 저장된 방향 값 사용
    }

    private void ManualMove()
    {
        // 키보드, 마우스, 터치, 조이스틱 등 외부에서 들어오는
        // 입력 소스는 모오오두 'Input' 클래스를 통해 관리할 수 있다.
        //float h = Input.GetAxis("Horizontal"); // 수평 키 : -1f ~ 1f
        var h = Input.GetAxisRaw("Horizontal"); // 수평 키 : -1, 0, 1

        //float v = Input.GetAxis("Vertical");  // 수직 키: -1f ~ 1f
        var v = Input.GetAxisRaw("Vertical"); // 수직 키: -1, 0, 1

        // 방향 만들기
        var direction = new Vector2(h, v);
        // 벡터로부터 방향만 가져오는 것을: 정규화
        direction = direction.normalized;
        direction.Normalize();


        // transform.Translate(direction * Speed * Time.deltaTime);

        RecordMovement(direction);  // 저장된 방향 값 사용
    }

    private void MoveByDirection()
    {
        PlayAnimation(_direction);
        // 이동 위치 계산
        var newPosition = transform.position + (Vector3)(_direction * _player.MoveSpeed) * Time.deltaTime;
        
        newPosition.y = Math.Clamp(newPosition.y, MinY, MaxY);
        // // 이동 위치 제한
        if (newPosition.x < MinX)
            newPosition.x = MinX;
        else if (newPosition.x > MaxX) newPosition.x = MaxX;
        
        // // 넘어가면 반대로
        // if (newPosition.x < MinX)
        //     newPosition.x = MaxX;
        // else if (newPosition.x > MaxX) newPosition.x = MinX;

        transform.position = newPosition;
    }

    private void PlayAnimation(Vector2 direction)
    {
        _myAnimator.SetInteger("x", Math.Sign(direction.x));
    }

    private void RecordMovement(Vector2 direction)
    {
        if (direction != _direction)    // 방향이 바뀌면 저장 및 기록
        {
            // 커맨드 등록 및 실행
            ICommand command = new PlayerMoveCommand(this, direction);
            CommandInvoker.Instance.ExecuteCommand(command);
        }
    }

    private void Replay()
    {
        Direction = Vector2.zero;
        transform.position = _defalutPosition;
    }
}