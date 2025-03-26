using System.Collections;
using UnityEngine;

public interface IBossAttackStrategy
{
    public IEnumerator CreateBullet(Vector2 pivotPosition);
}