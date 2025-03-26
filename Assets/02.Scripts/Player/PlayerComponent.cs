using System;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    protected Player _player;

    protected virtual void Awake()
    {
        _player = GetComponent<Player>();
    }
}
