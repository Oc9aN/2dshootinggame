using UnityEngine;

public class ItemCreateCommand : ICommand
{
    private ItemSpawner _itemSpawner;
    private int _type;
    private Vector2 _position;

    public ItemCreateCommand(ItemSpawner itemSpawner, int type, Vector2 position)
    {
        _itemSpawner = itemSpawner;
        _type = type;
        _position = position;
    }
    public void Execute()
    {
        _itemSpawner.ItemSpawn(_type, _position);
    }
}