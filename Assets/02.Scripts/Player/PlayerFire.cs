using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerFire : PlayerComponent
{
    // 목표: 총알을 만들어서 발사하고 싶다.

    // 필요 속성:

    // - 총구들
    public GameObject[] Muzzles;
    public GameObject[] SubMuzzles;

    // - 쿨타임 / 쿨타이머
    public float Cooltimer;

    private void Start()
    {
        CommandInvoker.Instance.OnReplay += Replay;
    }

    // 필요 기능:
    // - 발사하다.
    private void Update()
    {
        Cooltimer -= Time.deltaTime;

        // 쿨타임이 아직 안됐으면 종료
        if (Cooltimer > 0) return;

        // 자동 모드 이거나 "Fire1" 버튼이 입력되면..
        if (_player.PlayMode == PlayMode.Auto || Input.GetButtonDown("Fire1"))
        {
            RecordFire();

            Cooltimer = _player.AttackCoolTime;
        }
    }

    private void RecordFire()
    {
        ICommand command = new PlayerFireCommand(this);
        CommandInvoker.Instance.ExecuteCommand(command);
    }

    public void Fire()
    {
        // 오브젝트 풀을 활용한 총알 생성
        foreach (var muzzle in Muzzles)
        {
            BulletPool.Instance.Create(BulletType.Main, muzzle.transform.position);
        }

        foreach (var subMuzzle in SubMuzzles)
        {
            BulletPool.Instance.Create(BulletType.Sub, subMuzzle.transform.position);
        }
    }

    public void Replay()
    {
        // 총알을 다 지워준다.
        BulletPool.Instance.AllDestroy();
    }
}