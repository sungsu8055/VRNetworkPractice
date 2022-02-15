using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ConnManager : MonoBehaviourPunCallbacks
{


    void Start()
    {
        PhotonNetwork.GameVersion = "0.1";

        // 게임에서 사용할 사용자의 이름을 랜덤으로 설정
        int num = Random.Range(0, 1000);
        PhotonNetwork.NickName = "Player" + num;

        // 게임 참여 시 마스터 클라이언트가 구성한 씬에 자동 동기화
        PhotonNetwork.AutomaticallySyncScene = true;

        // 설정에 따라 네임 서버로 접속 후 마스터 서버 자동 접속
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()  // 마스터 서버 접속 시 작동
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);    // 로비로 연결
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비 접속 완료!");
        RoomOptions ro = new RoomOptions() // 방 생성 옵션
        {
            IsVisible = true, // 로비 방 목록에 출력
            IsOpen = true,  // 사용자들이 방에 들어올 수 있는지 여부
            MaxPlayers = 8, // 최대 허용 인원 수
        };
        PhotonNetwork.JoinOrCreateRoom("NetTest", ro, TypedLobby.Default); // 들어갈 방이 없으면 방 생성
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("룸 입장!");

        // 반경 2m 이내에 Player 프리팹을 생성한다
        Vector2 originPos = Random.insideUnitCircle * 2.0f;
        // 파라미터 값 Prefab, Position, Rotation / Quaternion.identity는 회전값이 없음, 월드 좌표 축이나 부모 객체 축으로 정렬됨
        PhotonNetwork.Instantiate("Player", new Vector3(originPos.x, 0, originPos.y), Quaternion.identity);
    }

    void Update()
    {
        
    }

}
