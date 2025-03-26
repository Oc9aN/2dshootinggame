using UnityEngine;

public class Enemy_Basic : Enemy
{
    public override void StartAction()
    {
        _direction = Vector2.down;
    }

    public override void UpdateAction()
    {
        transform.Translate(_direction * (Speed * Time.deltaTime));
    }

    public override void SetType()
    {
        _data.Type = EnemyType.Basic;
    }
}