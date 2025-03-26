using System;
using UnityEngine;

public enum BossBulletType
{
    Normal,
    TargetBig,
    TargetSmall,
}

public class BossBullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private BossBulletType _type;
    public BossBulletType Type => _type;

    public float Speed
    {
        private get { return _speed; }
        set { _speed = value; }
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        transform.Translate(Vector2.up * (_speed * Time.deltaTime));
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 총알과 충돌:
        if (other.CompareTag("Player"))
        {
            var enemy = other.GetComponent<IDamagable>();
            enemy.TakeDamage(new Damage(DamageType.Bullet, _damage, gameObject));

            gameObject.SetActive(false);
        }
    }
}