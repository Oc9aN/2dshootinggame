using UnityEngine;

public class Item_AttackSpeed : ItemObject
{
    protected override void ItemFunction(Player player)
    {
        // 효과 생성
        base.ItemFunction(player);
        
        _useSfx.Play();
        
        player.CoolTimeChange(ItemValue);
    }
}