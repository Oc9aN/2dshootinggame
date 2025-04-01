using System;
using UnityEngine;

public class Attendance
{
    public readonly AttendanceDataSO Data;
    
    // 보상 수령 여부
    private bool _rewarded;
    public bool IsRewarded => _rewarded;

    // 생성자
    public Attendance(AttendanceDataSO data, bool rewarded)
    {
        Data = data;
        _rewarded = rewarded;
    }

    public void SetRewarded(bool rewarded)
    {
        _rewarded = rewarded;
    }
}
