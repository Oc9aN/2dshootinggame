using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AttendanceSO", menuName = "Scriptable Objects/AttendanceSO")]
public class AttendanceDataSO : ScriptableObject
{
    public int Day = 0;
    public CurrencyType RewardCurrencyType;
    public int RewardAmount;
}
