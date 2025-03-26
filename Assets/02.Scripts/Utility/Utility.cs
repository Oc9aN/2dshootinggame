using UnityEngine;

public static class Utility
{
    // 유틸리티 함수로 빼기
    public static Vector2 MakeCircle(float angle)
    {
        float x = Mathf.Sin(angle * Mathf.Deg2Rad);
        float y = Mathf.Cos(angle * Mathf.Deg2Rad);
        return new Vector2(x, y);
    }

    public static float GetDirectionDegAngle(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}