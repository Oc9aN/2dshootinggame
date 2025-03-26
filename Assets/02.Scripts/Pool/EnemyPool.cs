using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    // 몬스터 프리팹들
    [SerializeField] private List<Enemy> _enemyPrefabs;
    // 풀 사이즈
    [SerializeField] private int _poolSize;
    // 풀
    private List<Enemy> _pool;
    
    // 싱글톤
    public static EnemyPool Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        int enemyPrefabCount = _enemyPrefabs.Count;
        _pool = new List<Enemy>(enemyPrefabCount * _poolSize);
        foreach (var enemyPrefab in _enemyPrefabs)
        {
            for (int i = 0; i < _poolSize; i++)
            {
                Enemy enemy = Instantiate(enemyPrefab, transform);
                
                _pool.Add(enemy);
                
                // 비활성화
                enemy.gameObject.SetActive(false);
            }
        }
    }
    
    public Enemy Create(EnemyType enemyType, Vector3 position)
    {
        foreach (var enemy in _pool)
        {
            if (enemy.Type == enemyType && enemy.gameObject.activeInHierarchy == false)
            {
                enemy.transform.position = position;
                    
                enemy.Initialize();
                    
                enemy.gameObject.SetActive(true);

                return enemy;
            }
        }
        return null;
    }

    public void AllDestroy()
    {
        foreach (var enemy in _pool)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}
