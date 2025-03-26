using System;
using System.Collections.Generic;
using UnityEngine;

public enum CurrencyType
{
    Gold,
    Diamond,

    Count // => 항상 마지막에
}

public class CurrencySaveData
{
    public List<int> Values = new(new int[(int)CurrencyType.Count]);
    
    // TODO: 저장 시간
}

public class CurrencyManager : MonoBehaviour
{
    // 싱글톤
    public static CurrencyManager instance;

    private CurrencySaveData _currencySaveData = new CurrencySaveData();
    private List<int> _values => _currencySaveData.Values;
    
    public event Action OnCurrencyChangedCallback;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;

        Load();
    }

    private void Start()
    {
        OnCurrencyChangedCallback?.Invoke();
    }

    // 재화량
    public int Get(CurrencyType currencyType)
    {
        return _values[(int)currencyType];
    }

    // 재화 추가
    public void Add(CurrencyType currencyType, int amount)
    {
        _values[(int)currencyType] += amount;

        Save();
        
        OnCurrencyChangedCallback?.Invoke();
    }

    // 재화 가지고 있니?
    public bool Have(CurrencyType currencyType, int amount)
    {
        return _values[(int)currencyType] >= amount;
    }

    public bool TryConsume(CurrencyType currencyType, int amount)
    {
        if (!Have(currencyType, amount))
        {
            return false;
        }

        _values[(int)currencyType] -= amount;

        Save();
        
        OnCurrencyChangedCallback?.Invoke();

        return true;
    }

    private const string SAVE_KEY = "Currency";

    private void Save()
    {
        string jsonData = JsonUtility.ToJson(_currencySaveData);
        PlayerPrefs.SetString(SAVE_KEY, jsonData);
    }

    private void Load()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY))
        {
            _currencySaveData = new CurrencySaveData();
            return;
        }
        
        string jsonData = PlayerPrefs.GetString(SAVE_KEY);
        _currencySaveData = JsonUtility.FromJson<CurrencySaveData>(jsonData);
    }
}