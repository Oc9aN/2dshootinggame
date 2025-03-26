using UnityEngine;

public class Enemy_Target : Enemy
{
    public override void StartAction()
    {
        SetDirection();
    }

    public override void UpdateAction()
    {
        transform.Translate(_direction * (Speed * Time.deltaTime));
    }

    public override void SetType()
    {
        _data.Type = EnemyType.Target;
    }
}