using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviourPun
{
    public Animator anim;
    // 최대 체력
    public float maxHP = 10f;
    // 공격력
    public float attackPower = 2f;
    // HP 슬라이더
    public Slider hpSlider;
    // 무기 콜라이더
    public BoxCollider weaponColl;

    // 현재 체력
    float currentHP;

    void Start()
    {
        // 현재 체력을 최대 체력으로 초기화
        currentHP = maxHP;
        hpSlider.value = currentHP / maxHP;
    }

    void Update()
    {
        // 트리거 버튼 입력 시 RPC 함수 실행하여 공격 진행
        if(ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
        {
            // 자신의 캐릭터만 공격 함수 실행
            if (photonView.IsMine)
            {
                // 포톤뷰의 컴포넌트인 RPC를 통해 AttackAnimation 간접 호출
                photonView.RPC("AttackAnimation", RpcTarget.AllBuffered);
            }
        }
    }

    // 공격 애니메이션 호출 함수 + RPC 애트리뷰트
    [PunRPC]
    public void AttackAnimation()
    {
        anim.SetTrigger("Attack");
    }

    [PunRPC]
    public void Damaged(float pow)
    {
        // 0이 최소 값, 현재 체력에서 공격력 만큼 체력 감소
        currentHP = Mathf.Max(0, currentHP - pow); 
        // 매 호출 마다 현재 체력이 업데이트 됨으로 두 수중 최대값을 뽑는 함수를 써도 체력 감소 효과 있음
        // 1타 10-2=8 / 2타 8-2=6 / ~... 체력이 모두 떨어진 경우 최대값 0을 출력

        // hp 바에 현재 체력 출력
        hpSlider.value = currentHP / maxHP;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 자기 자신의 캐릭터이면서 무기에 닿은 대상이 Player일 경우
        if(photonView.IsMine && other.gameObject.name.Contains("Player"))
        {
            // 무기에 닿은 대상 포톤뷰에서 데미지 처리 함수를 RPC로 호출
            PhotonView pv = other.GetComponent<PhotonView>();
            pv.RPC("Damaged", RpcTarget.AllBuffered, attackPower);

            weaponColl.enabled = false;
        }
    }
}
