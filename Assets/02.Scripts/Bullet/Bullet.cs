using System;
using UnityEngine;

public enum BulletType
{
    Main,
    Sub
}

public class Bullet : MonoBehaviour
{
    // 목표: 위로 계속 이동하고 싶다.

    // 필요 속성
    // - 이동 속도
    [SerializeField] private PlayerBulletDataSO _playerBulletData;
    public BulletType BulletType => _playerBulletData.BulletType;

    private float _damageFactor;

    public void Initialize(float damageFactor)
    {
        // 초기화 코드
        _damageFactor = damageFactor;
    }

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 총알과 충돌:
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<IDamagable>();
            enemy.TakeDamage(new Damage(DamageType.Bullet, (int)(_playerBulletData.Damage * _damageFactor), gameObject));

            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }


    // 기능
    // - 계속 위로 이동
    private void Move()
    {
        // 구현 순서
        // 1. 방향을 정한다.
        var dir = Vector2.up;

        // 2. 이동한다.
        transform.Translate(dir * (_playerBulletData.Speed * Time.deltaTime));
    }
}