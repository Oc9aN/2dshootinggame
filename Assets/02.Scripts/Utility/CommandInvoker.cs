using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    public static CommandInvoker Instance;
    
    private bool _isRecording;
    private bool _isReplaying;
    private float _replayTime;

    private float _recordingTime;

    // 시간순으로 정렬 된 명령어
    private SortedList<float, List<ICommand>> _recordedCommands = new SortedList<float, List<ICommand>>();
    
    // 리플레이 시작 시
    public event Action OnReplay;
    // 리플레이 종료 후
    public event Action OnEndReplay;
    
    public bool IsReplaying => _isReplaying;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();
        
        if (_isRecording)
        {
            if (_recordedCommands.ContainsKey(_recordingTime))
            {
                _recordedCommands[_recordingTime].Add(command);
            }
            else
            {
                _recordedCommands.Add(_recordingTime, new List<ICommand>(){ command });
            }
        }

        // Debug.Log($"record Time : {_recordingTime}");
        // Debug.Log($"record command : {command}");
    }

    public void Record()
    {
        _isRecording = true;
        _recordingTime = 0f;
        _isReplaying = false;
    }

    [ContextMenu("Replay")]
    public void Replay()
    {
        OnReplay?.Invoke();
        _isReplaying = true;
        _replayTime = 0f;
        _isRecording = false;
    }

    private void InitReplay()
    {
        _isRecording = true;
        _isReplaying = false;
        _recordingTime = 0f;
        _replayTime = 0f;
    }

    private void Update()
    {
        if (_isRecording)
            _recordingTime += Time.deltaTime;
        
        if (_isReplaying)
        {
            _replayTime += Time.deltaTime;
            if (_recordedCommands.Count > 0)
            {
                // queue처럼 실행 -> 맨 처음 명령부터 해야 리플레이
                float commandTime = _recordedCommands.Keys[0];

                if (_replayTime >= commandTime)
                {
                    foreach (var c in _recordedCommands.Values[0])
                    {
                        c.Execute();
                    }
                    _recordedCommands.RemoveAt(0);
                }
            }
            else
            {
                Debug.Log("Replay Over");
                OnEndReplay?.Invoke();
                InitReplay();
            }
        }
    }
}