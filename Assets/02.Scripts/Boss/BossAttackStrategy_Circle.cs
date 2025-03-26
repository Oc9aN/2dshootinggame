using System.Collections;
using UnityEngine;

public class BossAttackStrategy_Circle : IBossAttackStrategy
{
    private BossAttackPatternCricleData _patternCricleData;

    public BossAttackStrategy_Circle(BossAttackPatternCricleData patternData)
    {
        _patternCricleData = patternData;
    }

    public IEnumerator CreateBullet(Vector2 pivotPosition)
    {
        if (_patternCricleData.Direction >= 0)
        {
            for (int i = 0; i < _patternCricleData.MaxBullets; i++)
            {
                InstantiateBullet(i, pivotPosition);
                // 딜레이 후 생성
                if (_patternCricleData.Delay > 0)
                {
                    yield return new WaitForSeconds(_patternCricleData.Delay);
                }
                else
                {
                    yield return null;
                }
            }
        }
        else
        {
            for (int i = _patternCricleData.MaxBullets; i > 0; i--)
            {
                InstantiateBullet(i, pivotPosition);
                // 딜레이 후 생성
                if (_patternCricleData.Delay > 0)
                {
                    yield return new WaitForSeconds(_patternCricleData.Delay);
                }
                else
                {
                    yield return null;
                }
            }
        }

        yield break;
    }

    private void InstantiateBullet(int index, Vector2 pivotPosition)
    {
        float fullAngle = _patternCricleData.MaxAngle - _patternCricleData.MinAngle;
        float angle = _patternCricleData.MinAngle + index * (fullAngle / _patternCricleData.MaxBullets);
        Vector2 position = Utility.MakeCircle(angle) + (Vector2)pivotPosition;
        BossBullet bullet = BossBulletPool.Instance.Create(BossBulletType.Normal, position);
        bullet.Speed = _patternCricleData.BulletSpeed;
        Vector2 direction = pivotPosition - (Vector2)bullet.transform.position;
        var rotation = Utility.GetDirectionDegAngle(direction);
        bullet.transform.rotation = Quaternion.Euler(0, 0, rotation + 90f);
    }
}