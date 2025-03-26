using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticlePool : MonoBehaviour
{
    // 파티클 프리팹들
    [SerializeField] private List<Particle> _prticlePrefabs;
    // 풀 사이즈
    [SerializeField] private int _poolSize;
    // 풀
    private List<Particle> _pool;
    
    // 싱글톤
    public static ParticlePool Instance;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;

        _pool = ListPool<Particle>.Get();
        foreach (var prticlePrefab in _prticlePrefabs)
        {
            for (int i = 0; i < _poolSize; i++)
            {
                Particle particle = Instantiate(prticlePrefab, transform);
                
                _pool.Add(particle);
                
                // 비활성화
                particle.gameObject.SetActive(false);
            }
        }
    }
    
    public Particle Create(ParticleType type, Vector3 position)
    {
        foreach (var particle in _pool)
        {
            if (particle.Type == type && particle.gameObject.activeInHierarchy == false)
            {
                particle.transform.position = position;
                    
                //particle.Initialize();
                    
                particle.gameObject.SetActive(true);

                return particle;
            }
        }
        return null;
    }
    
    // 오브젝트가 제거될 때 리스트 반환
    private void OnDestroy()
    {
        ListPool<Particle>.Release(_pool);
    }
}
