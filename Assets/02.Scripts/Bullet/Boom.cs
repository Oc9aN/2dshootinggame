using System;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public float BoomTime = 2f;
    private int _damage = int.MaxValue;

    private float _timer = 0f;

    public void EnableBoom()
    {
        _timer = 0f;
        gameObject.SetActive(true);
    }

    public void DisableBoom()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= BoomTime)
        {
            // BoomTime초 후 제거
            DisableBoom();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 총알과 충돌:
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            enemy.TakeDamage(new Damage(DamageType.Boom, _damage, gameObject));
        }
    }
}