using UnityEngine;

public interface IAutoTargetFindStrategy
{
    /// <summary>
    ///     타겟 탐색
    /// </summary>
    /// <param name="referencePoint">기준 위치</param>
    /// <param name="target">타겟</param>
    public void TargetSearch(Vector3 referencePoint, ref GameObject target);
}