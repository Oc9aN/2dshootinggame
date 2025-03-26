using UnityEngine;

public class AutoTargetFindStrategy
{
    private IAutoTargetFindStrategy _autoMoveStrategy;

    public void SetStrategy(IAutoTargetFindStrategy strategy)
    {
        _autoMoveStrategy = strategy;
    }

    public void ExecuteStrategy(Vector3 referencePoint, ref GameObject target)
    {
        _autoMoveStrategy.TargetSearch(referencePoint, ref target);
    }
}

public class ClosestTargetFindStrategy : IAutoTargetFindStrategy
{
    public void TargetSearch(Vector3 referencePoint, ref GameObject target)
    {
        // 타겟이 있는데 활성화인 경우만 return
        if (target && target.activeInHierarchy) return;

        target = null;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var minDistance = Mathf.Infinity;
        foreach (var enemy in enemies)
        {
            // 풀에서 비활성화 된 적은 무시
            if (!enemy.activeInHierarchy) continue;
            // 아래 내려간 적은 무시
            if (enemy.transform.position.y < referencePoint.y) continue;
            // 거리 체크
            var distance = Vector2.Distance(referencePoint, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                target = enemy;
            }
        }
    }
}

public class FarthestTargetFindStrategy : IAutoTargetFindStrategy
{
    public void TargetSearch(Vector3 referencePoint, ref GameObject target)
    {
        // 타겟이 있는데 활성화인 경우만 return
        if (target && target.activeInHierarchy) return;
        
        target = null;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var maxDistance = 0f;
        foreach (var enemy in enemies)
        {
            // 풀에서 비활성화 된 적은 무시
            if (!enemy.activeInHierarchy) continue;
            // 아래 내려간 적은 무시
            if (enemy.transform.position.y < referencePoint.y) continue;
            
            var distance = Vector2.Distance(referencePoint, enemy.transform.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                target = enemy;
            }
        }
    }
}

public class HealthLowTargetFindStrategy : IAutoTargetFindStrategy
{
    public void TargetSearch(Vector3 referencePoint, ref GameObject target)
    {
        // 타겟이 있는데 활성화인 경우만 return
        if (target && target.activeInHierarchy) return;
        
        target = null;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        var minHealth = int.MaxValue;
        foreach (var enemy in enemies)
        {
            // 풀에서 비활성화 된 적은 무시
            if (!enemy.activeInHierarchy) continue;
            // 아래 내려간 적은 무시
            if (enemy.transform.position.y < referencePoint.y) continue;
            
            var health = enemy.GetComponent<Enemy>().Health;
            if (health < minHealth)
            {
                minHealth = health;
                target = enemy;
            }
        }
    }
}