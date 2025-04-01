using System;
using DG.Tweening;
using UnityEngine;

public class UI_PopUp : MonoBehaviour
{
    public float Duration;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void ActivePopUp()
    {
        _rectTransform.DOKill();
        _rectTransform.localScale = Vector3.zero;
        gameObject.SetActive(true);
        _rectTransform.DOScale(new Vector3(1f, 1f, 1f), Duration).SetUpdate(true).SetEase(Ease.Unset);
    }

    public void InactivePopUp()
    {
        _rectTransform.DOKill();
        _rectTransform.DOScale(new Vector3(0f, 0f, 1f), Duration).SetUpdate(true).SetEase(Ease.Unset)
            .OnComplete(() => gameObject.SetActive(false));
    }
}