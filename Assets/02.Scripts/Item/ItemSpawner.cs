using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    // 랜덤하게 아이템을 생성해줌
    // 위치를 받아서.
    public List<GameObject> ItemPrefabs = new List<GameObject>();
    private List<GameObject> _items = new List<GameObject>();
    
    public static ItemSpawner instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    private void Start()
    {
        CommandInvoker.Instance.OnReplay += Replay;
    }

    public void RecordItemSpawn(Vector2 position)
    {
        int type = Random.Range(0, ItemPrefabs.Count);
        ICommand command = new ItemCreateCommand(this, type, position);
        CommandInvoker.Instance.ExecuteCommand(command);
    }

    public void ItemSpawn(int type, Vector2 position)
    {
        // Debug.Log("Spawning " + type);
        var item = Instantiate(ItemPrefabs[type]);
        _items.Add(item);
        item.transform.position = position;
        item.GetComponent<ItemObject>().OnDeath += () => _items.Remove(item);
    }

    private void Replay()
    {
        // 아이템 제거
        foreach (var i in _items)
        {
            Destroy(i);
        }
    }
}