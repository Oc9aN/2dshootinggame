using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"OhterCollider: {other.otherCollider.GetType()}, Collider: {other.collider.name}");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var overlaps = new List<Collider2D>();
        other.Overlap(overlaps);
        foreach (var c in overlaps)
        {
            Debug.Log($"{c.GetType()}");
        }
    }
}