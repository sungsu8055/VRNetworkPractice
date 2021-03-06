using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    // 이동 속도
    public float moveSpeed = 3.0f;
    // 회전 속도
    public float rotSpeed = 200.0f;
    // 카메라 오브젝트
    public GameObject cameraRig;
    // 캐릭터
    public Transform myCharacter;
    // 애니메이터
    public Animator anim;
    // 플레이어 이름
    public Text nameText;

    // 서버에서 받은 데이터를 저장할 변수
    Vector3 setPos;
    Quaternion setRot;
    float dir_speed = 0;

    void Start()
    {
        cameraRig.SetActive(photonView.IsMine);

        // 각 접속자의 닉네임 출력
        nameText.text = photonView.Owner.NickName;

        // 자신의 이름은 녹색 다른 사람은 빨간색
        if (photonView.IsMine)
        {
            nameText.color = Color.green;
        }
        else
        {
            nameText.color = Color.red;
        }
    }

    void Update()
    {
        Move();
        Rotate();
    }

    // 함수 호출 순서로 인해 제일 마지막으로 배치 했을 경우 데이터 송수신이 안 되는 것으로 보임
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position); // Player 포지션(카메라, 모델링) 
            stream.SendNext(myCharacter.rotation); // 모델링의 로테이션
            stream.SendNext(anim.GetFloat("Speed"));
        }
        else if (stream.IsReading)
        {
            setPos = (Vector3)stream.ReceiveNext();
            setRot = (Quaternion)stream.ReceiveNext();
            dir_speed = (float)stream.ReceiveNext();
        }
    }

    void Move()
    {
        if (photonView.IsMine)
        {
            // 왼손 썸스틱 방향 값을 가져와 캐릭터 이동 방향 설정
            float stickPosX = ARAVRInput.GetAxis("Horizontal");
            float stickPosY = ARAVRInput.GetAxis("Vertical");
            Vector3 dir = new Vector3(stickPosX, 0, stickPosY);
            dir.Normalize();
            // 캐릭터의 이동 방향 벡터를 카메라가 바라보는 방향으로 설정
            dir = cameraRig.transform.TransformDirection(dir);
            transform.position += dir * moveSpeed * Time.deltaTime;
            // 왼손 썸스틱 회전 시 해당 방향으로 캐릭터 회전
            float magnitude = dir.magnitude;

            if (magnitude > 0)
            {
                myCharacter.rotation = Quaternion.LookRotation(dir);
            }
            // 애니메이터 브랜드 트리 변수에 벡터 크기 전달
            anim.SetFloat("Speed", magnitude);
        }
        else
        {
            // 전체 오브젝트 위치 값과 캐릭터 회전 값을 서버에서 전달 받은 값으로 동기화
            transform.position = Vector3.Lerp(transform.position, setPos, Time.deltaTime * 20.0f);
            myCharacter.transform.rotation = Quaternion.Lerp(myCharacter.rotation, setRot, Time.deltaTime * 20.0f);

            // 서버에서 전달 받은 애니메이터 파라미터 값 동기화
            anim.SetFloat("Speed", dir_speed);
        }
    }

    void Rotate()
    {
        if (photonView.IsMine)
        {
            // 오른손 방향 값에서 좌우 기울기 누적
            float rotH = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch).x;
            // 카메라 회전
            cameraRig.transform.eulerAngles += new Vector3(0, rotH, 0) * rotSpeed * Time.deltaTime;
        }           
    }    
}
