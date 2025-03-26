using System.Collections.Generic;
using UnityEngine;

public class UI_Stat : MonoBehaviour
{
    public List<UI_StatButton> ButtonList;

    private void Start()
    {
        // 스텟 메니저로부터 스텟을 가져와 리스트를 초기화
        for (int i = 0; i < ButtonList.Count; i++)
        {
            ButtonList[i].Stat = StatManager.instance.Stats[i];
        }
        
        // 스텟변화 Event Handling
        StatManager.instance.OnDataChangedCallback += (_) => Refresh();
        
        // 돈 변화
        CurrencyManager.instance.OnCurrencyChangedCallback += Refresh;

        Refresh();
    }

    // 새로고침
    private void Refresh()
    {
        foreach (var button in ButtonList)
        {
            button.Refresh();
        }
    }
}
