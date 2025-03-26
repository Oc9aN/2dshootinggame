using System.Collections;
using UnityEngine;

public class BossAttackStrategy_Target : IBossAttackStrategy
{
    private GameObject _target;
    private BossAttackPatternData _patternData;

    public BossAttackStrategy_Target(BossAttackPatternData patternData)
    {
        _patternData = patternData;
    }

    public IEnumerator CreateBullet(Vector2 pivotPosition)
    {
        for (int i = 0; i < _patternData.MaxBullets; i++)
        {
            BossBullet bullet = BossBulletPool.Instance.Create(BossBulletType.Normal, pivotPosition);
            bullet.Speed = _patternData.BulletSpeed;
            _target ??= GameObject.FindGameObjectWithTag("Player");
            Vector2 direction = bullet.transform.position - _target.transform.position;
            var rotation = Utility.GetDirectionDegAngle(direction);
            bullet.transform.rotation = Quaternion.Euler(0, 0, rotation + 90f);
            yield return new WaitForSeconds(_patternData.Delay);
        }
        yield break;
    }
}