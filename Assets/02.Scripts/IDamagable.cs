using UnityEngine;

public interface IDamagable
{
    public int Health { get; }
    public void TakeDamage(Damage damage);
}
