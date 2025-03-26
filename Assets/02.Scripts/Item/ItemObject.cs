using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ItemObject : MonoBehaviour
{
    // 플레이어와 충돌하면 1초 후 아이템 효과 실행
    // 아이템 효과
    public float ItemThreshold = 1f;
    public float ItemValue;
    
    protected AudioSource _useSfx;

    private float _timer;

    public event Action OnDeath;

    private void Awake()
    {
        _useSfx = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) _timer = 0f; // 타이머 초기화
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _timer += Time.deltaTime;

            if (_timer >= ItemThreshold)
            {
                ItemFunction(other.GetComponent<Player>());
                Destroy(gameObject);
            }
        }
    }

    protected virtual void ItemFunction(Player player)
    {
        ParticlePool.Instance.Create(ParticleType.Item, transform.position);
    }

    private void OnDestroy()
    {
        OnDeath?.Invoke();
    }
}