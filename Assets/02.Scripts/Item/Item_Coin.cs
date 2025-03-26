public class Item_Coin : ItemObject
{
    protected override void ItemFunction(Player player)
    {
        // 효과 생성
        base.ItemFunction(player);
        
        _useSfx.Play();
        
        // 돈 추가
        CurrencyManager.instance.Add(CurrencyType.Gold, (int)ItemValue);
    }
}