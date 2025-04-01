using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AttendanceItem : MonoBehaviour
{
    private Attendance _attendance;

    public TextMeshProUGUI DayText;
    public Image RewardImage;
    public TextMeshProUGUI RewardAmountText;
    public Button AttendButton;

    public void Initialized(Attendance attendance, Sprite icon)
    {
        _attendance = attendance;
        RewardImage.sprite = icon;
        DayText.text = $"Day {_attendance.Data.Day}";
        RewardAmountText.text = $"{attendance.Data.RewardAmount}";
        AttendButton.onClick.AddListener(() => AttendanceManager.instance.TryGetReward(_attendance));
    }

    public void Refresh()
    {
        if (_attendance.IsRewarded)
        {
            // 리워드 받은 상태면 비활성화
            AttendButton.interactable = false;
        }
        else
        {
            // 아니면 활성화
            AttendButton.interactable = true;
        }
    }
}
