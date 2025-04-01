using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class AttendanceRewardRecordWrapper
{
    public List<bool> isRewardedRecordList;
}

public class AttendanceManager : MonoBehaviour
{
    private const string ATTEDANCE_DATA_KEY = "AttendanceData";

    private const string LASTLOGINTIME_KEY = "LastLoginDateTime";

    private const string ATTENDANCE_COUNT_KEY = "AttendanceCount";

    // 관리자는 추가 삭제 조회 정렬만
    public static AttendanceManager instance;

    // 출석 데이터들
    public List<AttendanceDataSO> SoDatas;

    // 출석 객체들
    private List<Attendance> _attendances;

    // 조회
    public List<Attendance> Attendances => _attendances;

    // 출석 검증 데이터 Save, Load
    private DateTime _lastLoginDateTime = new DateTime(); // 마지막으로 로그인 한 날짜
    private int _attendanceCount = 0; // 현재까지 출석 횟수

    // 출석 이벤트
    public event Action OnDataChanged;

    private void Awake()
    {
        // 게임 시작시 싱글톤으로
        if (instance != null)
        {
            Destroy(gameObject);
        }

        // 출석 객체 생성
        _attendances = new List<Attendance>(SoDatas.Count); // 리스트 크기를 알기 때문에 미리 설정
        foreach (var data in SoDatas)
        {
            Attendance attendance = new Attendance(data, false);
            _attendances.Add(attendance);
        }

        instance = this;

        LoadAttendance();
        LoadLoginTime();
        LoadAttendanceCount();
    }

    private void Start()
    {
        AttendanceCheck();
    }

    private void AttendanceCheck()
    {
        //DateTime today = DateTime.Today;
        // TEST: 껏다 킬때마다 출석으로 확인
        DateTime now = DateTime.Now;

        if (now > _lastLoginDateTime) // 오늘이 마지막으로 로그인한 날짜가 오늘보다 크다면
        {
            _lastLoginDateTime = now;
            SaveLoginTime();
            _attendanceCount += 1;
            SaveAttendanceCount();
        }
    }

    private void LoadLoginTime()
    {
        string lastLoginDateTime = PlayerPrefs.GetString(LASTLOGINTIME_KEY, null);
        if (string.IsNullOrEmpty(lastLoginDateTime))
        {
            _lastLoginDateTime = new DateTime();
            //_lastLoginDateTime = DateTime.Today;
        }
        else
        {
            DateTime.TryParse(lastLoginDateTime, out _lastLoginDateTime);
        }

        Debug.Log("Last Login DateTime: " + _lastLoginDateTime);
    }

    private void LoadAttendance()
    {
        string attendanceData = PlayerPrefs.GetString(ATTEDANCE_DATA_KEY, null);

        if (string.IsNullOrEmpty(attendanceData)) return;

        // 데이터가 있으면 대입
        AttendanceRewardRecordWrapper rewardRecordWrapper =
            JsonUtility.FromJson<AttendanceRewardRecordWrapper>(attendanceData);
        var isRewardedList = rewardRecordWrapper.isRewardedRecordList;
        for (int i = 0; i < isRewardedList.Count; i++)
        {
            _attendances[i].SetRewarded(isRewardedList[i]);
        }
    }

    private void LoadAttendanceCount()
    {
        _attendanceCount = PlayerPrefs.GetInt(ATTENDANCE_COUNT_KEY, 0);
    }

    private void SaveLoginTime()
    {
        PlayerPrefs.SetString(LASTLOGINTIME_KEY, _lastLoginDateTime.ToString(CultureInfo.CurrentCulture));
    }

    private void SaveAttendance()
    {
        List<bool> isRewardedList = _attendances.Select(item => item.IsRewarded).ToList();

        // 래퍼 클래스에 대입
        AttendanceRewardRecordWrapper rewardRecordWrapper = new AttendanceRewardRecordWrapper
            { isRewardedRecordList = isRewardedList };
        // 제이슨 파일로 변환
        string attendanceJson = JsonUtility.ToJson(rewardRecordWrapper);
        PlayerPrefs.SetString(ATTEDANCE_DATA_KEY, attendanceJson);
    }

    private void SaveAttendanceCount()
    {
        PlayerPrefs.SetInt(ATTENDANCE_COUNT_KEY, _attendanceCount);
    }

    // 보상 받기
    public bool TryGetReward(Attendance attendance)
    {
        // 이미 보상을 받은 경우
        if (attendance.IsRewarded)
        {
            return false;
        }

        // 실제로 출석을 그만큼 했는지
        if (_attendanceCount < attendance.Data.Day)
        {
            Debug.Log("출석 불가");
            return false;
        }

        attendance.SetRewarded(true);
        CurrencyManager.instance.Add(attendance.Data.RewardCurrencyType, attendance.Data.RewardAmount);

        OnDataChanged?.Invoke();

        SaveAttendance();

        return true;
    }
}