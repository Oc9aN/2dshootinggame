using System;
using UnityEngine;

public class ReplayHandler : MonoBehaviour
{
    private void Start()
    {
        // 기록
        CommandInvoker.Instance.Record();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 리플레이
            PlayReplay();
        }
    }

    private void PlayReplay()
    {
        CommandInvoker.Instance.Replay();
    }
}