using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBulletDataSO", menuName = "Scriptable Objects/PlayerBulletDataSO")]
public class PlayerBulletDataSO : ScriptableObject
{
    public float Speed;
    public BulletType BulletType;
    public int Damage;
}
