using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    public BoxCollider weaponColl;

    void Start()
    {
        // 무기의 충돌 영역 비활성
        DeActivateCollider();
    }

    void Update()
    {
        
    }

    // 콜라이더 활성 함수
    public void ActivateCollider()
    {
        weaponColl.enabled = true;
    }

    // 콜라이더 비활성 함수
    public void DeActivateCollider()
    {
        weaponColl.enabled = false;
    }
}
