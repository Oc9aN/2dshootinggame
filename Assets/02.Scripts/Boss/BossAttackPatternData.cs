using UnityEngine;

[CreateAssetMenu(fileName = "BossAttackPatternData", menuName = "Scriptable Objects/BossAttackPatternData")]
public class BossAttackPatternData : ScriptableObject
{
    public GameObject BulletPrefab;
    public int MaxBullets;
    public float BulletSpeed;
    public float Delay;
}
