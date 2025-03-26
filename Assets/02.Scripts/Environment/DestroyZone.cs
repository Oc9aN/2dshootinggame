using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 레이어로 충돌 연산 없이 서로 충돌 해결
        other.gameObject.SetActive(false);
        // if (other.gameObject.CompareTag("Bullet"))
        // {
        //     other.gameObject.SetActive(false);
        // }
        // else
        // {
        //     Destroy(other.gameObject);
        // }
    }
}