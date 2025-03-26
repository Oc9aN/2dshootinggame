using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    // 오브젝트 풀
    // 게임 오브젝트를 필요한 만큼 미리 생성해 두고 풀에 쌓아두는 기법이다.
    // 오브젝트를 매번 생성하고 삭제하는 것보다 메모리 사용량과 성능 저하를 줄일 수 있다.
    // 즉, 오버헤드를 줄인다.
    // 컴퓨터 과학에 pool은 사용할 때 획득한 메모리와 나중에 해제되는 메모리가 아닌 미리 할당된 메모리 덩어리를 말한다.
    
    // 목표: 총알 풀에 총알을 풀 사이즈만큼 미리 생성해서 등록하고 싶다.
    // 속성
    // - 생성할 총알 프리팹
    public List<Bullet> BulletPrefabs;
    // - 메인 탄
    // - 보조 탄
    
    // - 풀 사이즈
    public int PoolSize = 30;
    
    // - 총알을 관리할 풀 리스트
    private List<Bullet> _bullets;
    
    // BulletPool 싱글톤
    public static BulletPool Instance;
    
    // 기능
    // 1. 태어날 때
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        
        // 2. 총알 풀을 총알을 담을 수 있는 크기로 만든다.
        int bulletPrefabCount = BulletPrefabs.Count;    // 좋은 코드 구조. 항상 값을 명확하게 저장해서 사용 / 책에도 나온 내용
        _bullets =  new List<Bullet>(bulletPrefabCount * PoolSize);
        // 3. 풀 사이즈만큼 반복해서
        foreach (var bulletPrefab in BulletPrefabs)
        {
            for (int i = 0; i < PoolSize; i++)
            {
                // 총알 생성후 부모 하위에 저장
                Bullet bullet = Instantiate(bulletPrefab, transform, true);  // 똑같이 값을 저장하고 사용
                
                _bullets.Add(bullet);

                // 비활성화
                bullet.gameObject.SetActive(false);
            }
        }
        // 4. 총알 프리팹으로부터 총알을 생성한다.
        // 5. 생성한 총알을 풀에 추가한다.
    }

    // 응집도가 높은 구조를 위한 함수 -> 필요한 총알을 찾아준다.
    // 장점
    // 1. 응집도를 높혔다.
    // 2. 은닉화/캡슐화를 했다.
    // 3. 객체 생성 로직을 분리했다.
    // 싱글톤 + 오브젝트 풀링 + 팩토리 메서드
    public Bullet Create(BulletType bulletType, Vector3 position)
    {
        foreach (var bullet in _bullets)
        {
            if (bullet.BulletType == bulletType && bullet.gameObject.activeInHierarchy == false)
            {
                bullet.transform.position = position;
                    
                bullet.Initialize(StatManager.instance.GetValue(StatType.DamageFactor));
                    
                bullet.gameObject.SetActive(true);

                return bullet;
            }
        }
        return null;
    }
    
    public void AllDestroy()
    {
        foreach (var enemy in _bullets)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}
