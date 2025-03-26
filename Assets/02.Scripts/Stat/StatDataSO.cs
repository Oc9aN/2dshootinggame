using UnityEngine;

public enum StatUnit
{
    Default,
    Percent,
}

[CreateAssetMenu(fileName = "StatDataSO", menuName = "Scriptable Objects/StatDataSO")]
public class StatDataSO : ScriptableObject
{
    public StatUnit Unit;

    public float DefaultValue;
    public float UpgradeAddValue;

    public int DefaultCost;
    public float UpgradeAddCost;
}