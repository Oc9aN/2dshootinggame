using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro; // 텍스트 메시 프로는 현재 유니티에서 기본으로 쓰고있는 텍스트 컴포넌트
using UnityEngine;
using UnityEngine.UI;

// 특징
// UI_Game을 참조하는 코드가 점점 많아져서 귀찮다.
// 이것을 한방에 접근할 방법?
// UI_Game의 인스턴스는 게임 내에 딱 하나만 존재하는구나

// 싱글톤 패턴
// - 인스턴스가 단 하나임을 보장한다.
// - 전역적인 접근이 가능하다.
// 게임 개발에서는 관리자(manager) 클래스를 싱글톤 패턴으로 설계하는 것이 일반적인 관행이다.
// 인스턴스사 단 하나임을 보장하고 쉽게 접근할 수 있다.
// 전역 접근, 코드 단순화(해당 관리자를 찾기위한 복잡한 로직이 필요없다.)
// 메모리 및 리소스 관리가 용이하다.

public class UI_Game : MonoBehaviour
{
    public static UI_Game Instance = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("UI_Game Instance already exists!");
            Destroy(gameObject);
        }

        Instance = this;
    }

    // 목적: 필살기 갯수, 킬 카운트 등 UI를 담당하고 싶다.
    // 속성:
    // - 필살기 개수 UI
    public List<GameObject> Booms;

    // - 킬 카운트 UI
    public TextMeshProUGUI KillText;

    // - 킬 카운트 UI
    public TextMeshProUGUI ScoreText;

    // 보스 체력 UI
    public Slider BossHealthSlider;
    
    // 보스 경고
    public TextMeshProUGUI BossText;
    
    // 골드 텍스트
    public TextMeshProUGUI GoldText;

    private void Start()
    {
        CurrencyManager.instance.OnCurrencyChangedCallback += RefreshGoldText;

        RefreshGoldText();
    }

    // 기능: 새로고침
    public void RefreshBoomInfo(int boomCount)
    {
        // 필살기 수에 따라 ui를 키고 끈다
        for (int i = 0; i < 3; i++)
        {
            Booms[i].SetActive(i < boomCount);
        }
    }

    public void RefreshKillInfo(int killCount)
    {
        // 킬 횟수 텍스트 새로고침
        KillText.text = $"{killCount}";
    }

    public void RefreshScoreInfo(int score)
    {
        ScoreText.text = $"{score:D5}";
        ScoreText.transform.DOKill(); // 기존 애니메이션 중지
        ScoreText.transform.localScale = Vector3.one; // 원래 크기로 초기화
        ScoreText.transform.DOPunchScale(Vector3.one * 0.5f, 0.3f, 10, 0.5f);
    }
    
    public void RefreshPlayerData(PlayerData playerData)
    {
        RefreshBoomInfo(playerData.BoomCount);
        RefreshKillInfo(playerData.KillCount);
        RefreshScoreInfo(playerData.Score);
    }

    public void ActiveBossHealth(int maxHealth)
    {
        BossHealthSlider.maxValue = maxHealth;
        BossHealthSlider.value = maxHealth;
        BossHealthSlider.gameObject.SetActive(true);
    }

    public void RefreshBossHealth(int bossHealth)
    {
        BossHealthSlider.value = bossHealth;
    }

    public void ShowBossText()
    {
        BossText.gameObject.SetActive(true);
        BossText.DOFade(0, 1f)
            .SetLoops(3, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
    
    public void DisableBossHealth()
    {
        BossHealthSlider.gameObject.SetActive(false);
    }

    public void RefreshGoldText()
    {
        GoldText.text = $"{CurrencyManager.instance.Get(CurrencyType.Gold):N0}";
    }
}