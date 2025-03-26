using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UI_StatButton : MonoBehaviour
{
    private Stat _stat;

    public Stat Stat
    {
        private get => _stat;
        set => _stat = value;
    }

    public TextMeshProUGUI NameTextUI;
    public TextMeshProUGUI ValueTextUI;
    public TextMeshProUGUI CostTextUI;
    
    public void Refresh()
    {
        NameTextUI.text = _stat.StatType.ToString();
        ValueTextUI.text = _stat.GetValueString();
        CostTextUI.text = $"{_stat.Cost:N0}";

        if (CurrencyManager.instance.Have(CurrencyType.Gold, _stat.Cost))
        {
            CostTextUI.color = Color.green;
        }
        else
        {
            CostTextUI.color = Color.red;
        }
    }

    public void OnClickLevelUp()
    {
        if (StatManager.instance.TryLevelUp(_stat.StatType))
        {
            // 업그레이드 성공
            transform.DOPunchPosition(Vector3.up * 20f, 0.3f, 5, 0.5f);
            transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f);
        }
        else
        {
            // 업그레이드 실패
            transform.DOPunchPosition(Vector3.right * 15f, 0.3f, 10, 1f);
            transform.DOPunchScale(Vector3.one * -0.1f, 0.3f, 5, 0.5f);
        }
    }
}