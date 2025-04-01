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

    public GameObject EnableVFXObject;
    
    private bool isAnimating = false;
    
    public void Refresh()
    {
        NameTextUI.text = _stat.StatType.ToString();
        ValueTextUI.text = _stat.GetValueString();
        CostTextUI.text = $"{_stat.Cost:N0}";

        if (CurrencyManager.instance.Have(CurrencyType.Gold, _stat.Cost))
        {
            CostTextUI.color = Color.green;
            EnableVFXObject.SetActive(true);
        }
        else
        {
            CostTextUI.color = Color.red;
            EnableVFXObject.SetActive(false);
        }
    }

    public void OnClickLevelUp()
    {
        if (isAnimating) return;  // 애니메이션 중에는 클릭을 무시

        isAnimating = true;
        transform.localScale = Vector3.one;

        bool upgradeSuccess = StatManager.instance.TryLevelUp(_stat.StatType);

        if (upgradeSuccess)
        {
            // 업그레이드 성공
            transform.DOPunchPosition(Vector3.up * 20f, 0.3f, 5, 0.5f)
                .OnKill(() => isAnimating = false); 
            transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 5, 0.5f)
                .OnKill(() => isAnimating = false); 
        }
        else
        {
            // 업그레이드 실패
            transform.DOPunchPosition(Vector3.right * 15f, 0.3f, 10, 1f)
                .OnKill(() => isAnimating = false); 
            transform.DOPunchScale(Vector3.one * -0.1f, 0.3f, 5, 0.5f)
                .OnKill(() => isAnimating = false); 
        }
    }
}