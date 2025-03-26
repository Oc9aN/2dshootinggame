using System;
using UnityEngine;

public class PlayerBoom : PlayerComponent
{
    // Boom 프리팹
    public Boom BoomObject;

    private void Start()
    {
        CommandInvoker.Instance.OnReplay += BoomObject.DisableBoom;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (_player.BoomCount <= 0)
                return;
            // 폭탄을 터트린다.
            RecordBoom();
        }
    }

    private void RecordBoom()
    {
        ICommand command = new PlayerBoomCommand(this);
        CommandInvoker.Instance.ExecuteCommand(command);
    }

    public void Boom()
    {
        BoomObject.EnableBoom();
        _player.BoomCount--;
    }
}