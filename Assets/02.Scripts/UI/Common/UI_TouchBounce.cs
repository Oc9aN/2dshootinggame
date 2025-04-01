using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TouchBounce : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // 마우스 터치 시 크기 변경
    public float StartScale = 1f;
    public float EndScale = 1.2f;
    public float Duration = 0.5f;
    
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnDisable()
    {
        _rectTransform.localScale = new Vector3(StartScale, StartScale, 1f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _rectTransform.DOScale(new Vector3(EndScale, EndScale, 1f), Duration).SetUpdate(true);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        _rectTransform.DOScale(new Vector3(StartScale, StartScale, 1f), Duration).SetUpdate(true);
    }
}
