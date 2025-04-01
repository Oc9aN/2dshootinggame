using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Attendance : MonoBehaviour
{
    public Transform Content;
    public UI_AttendanceItem AttendanceItemPrefab;
    private List<UI_AttendanceItem> _buttonList;

    private UI_PopUp _popUp;

    private void Awake()
    {
        _popUp = GetComponent<UI_PopUp>();
    }

    public void Start()
    {
        AttendanceManager.instance.OnDataChanged += Refresh;

        var attendances = AttendanceManager.instance.Attendances;
        _buttonList = new List<UI_AttendanceItem>(attendances.Count);
        for (int i = 0; i < attendances.Count; i++)
        {
            _buttonList.Add(Instantiate(AttendanceItemPrefab, Content));
        }

        for (int i = 0; i < _buttonList.Count; i++)
        {
            var attendanceData = AttendanceManager.instance.Attendances[i];
            Debug.Log(attendanceData.IsRewarded);
            var currencyType = attendanceData.Data.RewardCurrencyType;
            Debug.Log(CurrencyManager.instance.CurrencyIconList[(int)currencyType]);
            _buttonList[i].Initialized(attendanceData, CurrencyManager.instance.CurrencyIconList[(int)currencyType]);
        }

        Refresh();

        gameObject.SetActive(false);
    }

    private void Refresh()
    {
        foreach (var button in _buttonList)
        {
            button.Refresh();
        }
    }

    public void SetActiveToggle()
    {
        if (gameObject.activeSelf)
        {
            _popUp.InactivePopUp();
        }
        else
        {
            _popUp.ActivePopUp();
        }
    }
}