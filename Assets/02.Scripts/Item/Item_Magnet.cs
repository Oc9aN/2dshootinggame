using UnityEngine;

public class Item_Magnet : ItemObject
{
    protected override void ItemFunction(Player player)
    {
        Debug.Log("자석 효과 발동");
        var items = GameObject.FindGameObjectsWithTag("Item");
        foreach (var i in items)
        {
            i.GetComponent<ItemMove>().Snap();
        }
    }
}