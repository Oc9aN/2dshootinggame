using UnityEngine;

public enum DamageType
{
    Bullet,
    Boom,
    Enemy,
}

public struct Damage
{
    public DamageType Type;
    public int DamageValue;
    public GameObject From;

    public Damage(DamageType type, int damageValue, GameObject from)
    {
        Type = type;
        DamageValue = damageValue;
        From = from;
    }
}
