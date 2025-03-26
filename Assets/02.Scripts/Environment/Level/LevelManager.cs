using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField] private List<LevelDataSO> _levelDatas;

    [SerializeField] private Player _player;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    public LevelDataSO GetLevelData()
    {
        int score = _player.Score;

        foreach (var levelData in _levelDatas)
        {
            if (score < levelData.Level)
                return levelData;
        }

        return _levelDatas[^1];
    }
}
