using System;
using UnityEngine;

public enum PlayMode
{
    Auto,
    Mannual,
}

public class Player : MonoBehaviour, IDamagable
{
    // 플레이어 스텟
    [SerializeField] private float _moveSpeed = 3f;
    public float MoveSpeed => _moveSpeed;
    [SerializeField] private float _attackCoolTime = 0.6f;
    public float AttackCoolTime => _attackCoolTime;
    [SerializeField] private int _health = 100;
    public int Health => _health;

    // 초기 스텟
    private float _defaultMoveSpeed;
    private float _defaultAttackCoolTime;
    private int _defaultHealth;

    // 암호화
    private AESCrypto _aesCrypto;

    // Boom count
    private const int MAXBOOMCOUNT = 3; // 최대 폭탄 갯수
    private const int MAXKILLCOUNT = 20; // 폭탄 채우기 위해 죽여야하는 유닛 수

    public event Action<int> KillEvent;

    // 플레이어 데이터
    [SerializeField] private PlayerData _playerData;
    public int Score => _playerData.Score;

    public int BoomCount
    {
        get => _playerData.BoomCount;
        set
        {
            _playerData.BoomCount = value;
            UI_Game.Instance.RefreshBoomInfo(_playerData.BoomCount);
            Save();
        }
    }


    public float Defence = 0.2f;

    // - 모드(자동, 수동)
    public PlayMode PlayMode = PlayMode.Mannual;

    // 타겟 탐색 방식
    public TargetMode TargetMode = TargetMode.Closest;

    private void Awake()
    {
        // 초기값 저장
        _defaultMoveSpeed = _moveSpeed;
        _defaultHealth = _health;
        _defaultAttackCoolTime = _attackCoolTime;

        PlayerPrefs.DeleteAll();
        _aesCrypto = new AESCrypto();
    }

    private void Start()
    {
        Load();
        UI_Game.Instance.RefreshPlayerData(_playerData);
        CommandInvoker.Instance.OnReplay += Replay;

        StatManager.instance.OnDataChangedCallback += (type) =>
        {
            if (type == StatType.MaxHealth)
            {
                Heal((int)StatManager.instance.GetValue(StatType.MaxHealth));
            }
        };
    }

    public void KillEnemy()
    {
        _playerData.TotalKill++;
        _playerData.KillCount++;
        UI_Game.Instance.RefreshKillInfo(_playerData.KillCount);
        if (_playerData.KillCount >= MAXKILLCOUNT)
        {
            BoomCount = Mathf.Min(MAXBOOMCOUNT, _playerData.BoomCount + 1);
            _playerData.KillCount = 0;
        }

        KillEvent?.Invoke(_playerData.KillCount);

        Save();
    }

    private void Save()
    {
        string data = JsonUtility.ToJson(_playerData);
        // Debug.Log($"암호화 이전: {data}");
        data = _aesCrypto.EncryptString(data);
        // Debug.Log($"암호화 이후: {data}");
        PlayerPrefs.SetString("PlayerData", data);
    }

    private void Load()
    {
        string data = PlayerPrefs.GetString("PlayerData", null);
        if (string.IsNullOrEmpty(data))
        {
            _playerData = new PlayerData();
            return;
        }

        // Debug.Log($"복호화 이전: {data}");
        data = _aesCrypto.DecryptString(data);
        // Debug.Log($"복호화 이후: {data}");
        _playerData = JsonUtility.FromJson<PlayerData>(data);
        UI_Game.Instance.RefreshPlayerData(_playerData);
    }

    private void Update()
    {
        // 키 입력 검사
        if (Input.GetKeyDown(KeyCode.Alpha1))
            PlayMode = PlayMode.Auto;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) PlayMode = PlayMode.Mannual;
    }

    public void TakeDamage(Damage damage)
    {
        _health -= (int)(damage.DamageValue * Defence);

        CameraShake.Instance.Shake(0.2f, 0.2f);

        if (_health <= 0)
        {
            Debug.Log("플레이어 죽음");
            Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        _health = Mathf.Min((int)StatManager.instance.GetValue(StatType.MaxHealth), _health + amount);
    }

    public void CoolTimeChange(float value)
    {
        _attackCoolTime = Mathf.Max(0.1f, _attackCoolTime - value);
    }

    public void SpeedIncrease(float increase)
    {
        _moveSpeed = Mathf.Min(10f, _moveSpeed + increase);
    }

    public void AddScore(int amount)
    {
        _playerData.Score += amount;
        UI_Game.Instance.RefreshScoreInfo(_playerData.Score);
        Save();
    }

    public void Replay()
    {
        // 아이템 스텟 초기화
        _health = _defaultHealth;
        _attackCoolTime = _defaultAttackCoolTime;
        _moveSpeed = _defaultMoveSpeed;
    }
}