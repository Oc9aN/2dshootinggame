using System;
using UnityEngine;

public class Background : MonoBehaviour
{
// 배경 스크롤링: 배경 이미지를 일정한 속도로 움직여 캐릭터, 몬스터 등의 움직임을
// 더 동적으로 만들어주는 기술
// 캐릭터는 그대로 두고 배경만 움직이는 일종의 눈속임

// 필요속성
// - 머터리얼
// - 스크롤 속도
    private SpriteRenderer _mySpriteRenderer;
    private Material _material;

    private float _offsetY = 0f;

    // 스크롤 속도
    public float ScrollSpeed = 1f;

    private void Awake()
    {
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        _material = _mySpriteRenderer.sharedMaterial;
    }

    private void Update()
    {
        // 방향을 구해서 방향으로 스크롤
        Vector2 direction = Vector2.up;

        // 방향으로 스크롤
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        Vector4 currentST = _material.GetVector(Shader.PropertyToID("_MainTex_ST"));
        
        _offsetY += (ScrollSpeed * Time.deltaTime);
        
        propertyBlock.SetVector(Shader.PropertyToID("_MainTex_ST"),
            new Vector4(currentST.x, currentST.y, currentST.z, _offsetY));
        _mySpriteRenderer.SetPropertyBlock(propertyBlock);
        //_material.mainTextureOffset += direction * (ScrollSpeed * Time.deltaTime);
    }
}