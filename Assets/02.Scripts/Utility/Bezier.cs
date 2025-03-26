using System.Collections.Generic;
using UnityEngine;

public static class Bezier
{
    // n까지 수에서 r개의 조합의 수
    private static int GetBinomialCoefficient(int n, int r)
    {
        if (r == 0 || r == n) return 1;
        var result = 1;
        for (var i = 0; i < r; i++)
        {
            result *= n - i; // n!
            result /= i + 1; // i = n-r -> r번 반복 -> (n-r)*r
        }

        return result;
    }

    public static Vector2 GetBezierValue(float range, List<Vector2> points)
    {
        var n = points.Count - 1; // n차 베지에 곡선은 점 n-1개로 이루어짐
        var position = Vector2.zero;
        for (var r = 0; r <= n; r++)
        {
            var binomialCoefficient = GetBinomialCoefficient(n, r);
            var remainderRangeValue = Mathf.Pow(1 - range, n - r);
            var rangeValue = Mathf.Pow(range, r);
            var rValue = binomialCoefficient * remainderRangeValue * rangeValue; // r에 따른 값 -> 포인트 지점에 따른 값
            position += rValue * points[r];
        }

        return position;
    }
}