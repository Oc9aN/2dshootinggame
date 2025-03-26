using UnityEngine;

public class Enemy_Follow : Enemy
{
    public override void StartAction()
    {
        SetDirection();
    }

    public override void UpdateAction()
    {
        SetDirection();
        transform.Translate(_direction * (Speed * Time.deltaTime));
    }

    public override void SetType()
    {
        _data.Type = EnemyType.Follow;
    }
}