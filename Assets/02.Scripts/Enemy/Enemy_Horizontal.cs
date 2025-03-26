using UnityEngine;

public class Enemy_Horizontal : Enemy
{
    public override void StartAction()
    {
        _direction = transform.position.x < 0 ? Vector2.right : Vector2.left;
    }

    public override void UpdateAction()
    {
        transform.Translate(_direction * (Speed * Time.deltaTime));
    }

    public override void SetType()
    {
        _data.Type = EnemyType.Horizontal;
    }
}