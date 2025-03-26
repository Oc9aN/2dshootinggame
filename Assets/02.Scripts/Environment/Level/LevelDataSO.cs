using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO", menuName = "Scriptable Objects/LevelDataSO")]
public class LevelDataSO : ScriptableObject
{
    public int Level = 1000;            // 기준이 되는 점수
    public float DamageFactor = 1f;     // 곱해줄 계수
    public float HealthFactor = 1f;     // 곱해줄 계수
    public float SpeedFactor = 1f;      // 곱해줄 계수
}
