using UnityEngine;
using UnityEngine.Serialization;

public class FollowerMove : MonoBehaviour
{
    // 캐릭터를 따라 다니는 움직임
    public Transform Player;
    public float SmoothTime = 0.3f;
    
    private Vector3 _velocity = Vector3.zero;

    void Update()
    {
        if (!Player) return;
        transform.position = Vector3.SmoothDamp(transform.position, Player.position, ref _velocity, SmoothTime);
    }
}