using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    // 스크립터블 오브젝트는 주로 데이터를 저장하는 '데이터 컨테이너'이다.
    // 게임 오브젝트의 기능이나 transform없이 단순히 데이터만 저장할 수 있다.
    // 공유된 데이터 개념이므로 이 방식은 플라이 웨이트 패턴이라고 할 수 있다.
    // 메모리 소모를 줄일 수 있다.
    // 테스트가 용이하다.
    
    public EnemyType  Type;
    public int Score;
}
