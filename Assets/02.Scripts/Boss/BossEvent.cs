using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BossEvent : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private BossObject _bossObject;
    [SerializeField] private GameObject _enemySpawners;

    [SerializeField] private int _killThreshold;
    private void Start()
    {
        _bossObject.gameObject.SetActive(false);
        _player.KillEvent += KillCountCheck;

        CommandInvoker.Instance.OnReplay += Replay;
    }

    private void KillCountCheck(int killCount)
    {
        if (!_bossObject.gameObject.activeInHierarchy && killCount >= _killThreshold)
        {
            // 보스 생성
            RecordCreate();
        }
    }

    private void RecordCreate()
    {
        ICommand command = new BossCreateCommand(this);
        CommandInvoker.Instance.ExecuteCommand(command);
    }

    public void CreateBoss()
    {
        StartCoroutine(BossAppearEvent());
    }

    private IEnumerator BossAppearEvent()
    {
        _enemySpawners.SetActive(false);
        UI_Game.Instance.ShowBossText();
        yield return new WaitForSeconds(3f);
        _bossObject.Initialize();
        _bossObject.gameObject.SetActive(true);
        _bossObject.OnDeath += DestroyBoss;
    }

    private void DestroyBoss()
    {
        StartCoroutine(DestroyBossEvent());
    }

    private IEnumerator DestroyBossEvent()
    {
        _player.AddScore(10000);
        for (int i = 0; i < 10; i++)
        {
            ParticlePool.Instance.Create(ParticleType.Explosion,
                (Vector2)_bossObject.transform.position + Random.insideUnitCircle * 2f);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }

        _enemySpawners.SetActive(true);
        _bossObject.gameObject.SetActive(false);
    }

    private void Replay()
    {
        UI_Game.Instance.DisableBossHealth();
    }
}
