using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PoolInfo<T>
{
    public T Prefab;
    public int PoolSize;
}
public class BossBulletPool : MonoBehaviour
{
    // // 몬스터 프리팹들
    // [SerializeField] private List<BossBullet> _bossBulletsPrefabs;
    // // 풀 사이즈
    // [SerializeField] private List<int> _poolSize;
    [SerializeField] private List<PoolInfo<BossBullet>> _poolInfos;
    // 풀
    private List<BossBullet> _pool;
    
    // 싱글톤
    public static BossBulletPool Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        int poolSize = _poolInfos.Sum(n => n.PoolSize);
        _pool = new List<BossBullet>(poolSize);
        for (int i = 0; i < _poolInfos.Count(); i++)
        {
            for (int j = 0; j < _poolInfos[i].PoolSize; j++)
            {
                BossBullet enemy = Instantiate(_poolInfos[i].Prefab, transform);
                
                _pool.Add(enemy);
                
                // 비활성화
                enemy.gameObject.SetActive(false);
            }
        }
    }
    
    public BossBullet Create(BossBulletType type, Vector3 position)
    {
        foreach (var bullet in _pool)
        {
            if (bullet.Type == type && bullet.gameObject.activeInHierarchy == false)
            {
                bullet.transform.position = position;
                    
                //bullet.Initialize();
                    
                bullet.gameObject.SetActive(true);

                return bullet;
            }
        }
        return null;
    }

    public void AllDestroy()
    {
        foreach (var bullet in _pool)
        {
            bullet.gameObject.SetActive(false);
        }
    }
}