using System;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager instance;
    
    private List<Stat> _stats = new();
    public List<Stat> Stats => _stats;

    public List<StatDataSO> StatDataList;
    
    public event Action<StatType> OnDataChangedCallback;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;

        for (int i = 0; i < StatDataList.Count; i++)
        {
            _stats.Add(new Stat((StatType)i, 0, StatDataList[i]));
        }
    }

    public bool TryLevelUp(StatType statType)
    {
        bool result = _stats[(int)statType].TryUpgrade();
        if (result)
        {
            OnDataChangedCallback?.Invoke(statType);
        }
        return result;
    }

    public float GetValue(StatType statType)
    {
        return _stats[(int)statType].Value;
    }
}
