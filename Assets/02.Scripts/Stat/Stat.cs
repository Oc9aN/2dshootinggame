using UnityEngine;

public enum StatType
{
    DamageFactor,
    MaxHealth,
    MoveSpeedFactor,
    
    Count,
}
// 속성:
// (Enum)   스텟 이름
// (int)    레벨
// (float)  현재 수치
// (int)    비용
// (float)  업그레이드 비용 증가 값
// (float)  업그레이드 수치 증가 값
    
// 메소드:
// 업그레이드 - public bool TryUpgrade()
// 레벨로 수치 계산 - private void Calculate()

// 스텟은 계산되어 나오는 값 -> 개방폐쇄의 흔한 예시인 도형의 넓이 계산과 같은 경우
public class Stat
{
    private StatDataSO _data;
    
    private StatType _statType;
    public StatType StatType => _statType;
    
    private int _level;
    private float _value;
    public float Value => _value;
    
    private int _cost;
    public int Cost => _cost;
    
    public Stat(StatType statType, int level, StatDataSO data)
    {
        _statType = statType;
        
        _level = level;

        _data = data;
        
        Calculate();
    }

    public bool TryUpgrade()
    {
        // 1. 돈이 충분한가?
        if (!CurrencyManager.instance.TryConsume(CurrencyType.Gold, _cost))
        {
            return false;
        }

        // 돈이 충분하면 레벨업
        _level += 1;
        
        // 레벨업 후 다시 수치 계산
        Calculate();

        return true;
    }

    private void Calculate()
    {
        _value = _data.DefaultValue + _level * _data.UpgradeAddValue;
        _cost = (int)(_data.DefaultCost + _level * _data.UpgradeAddCost);
    }

    public string GetValueString()
    {
        if (_data.Unit == StatUnit.Percent)
            return $"{_value * 100:N0}%";
        return $"{_value}"; 
    }
}
