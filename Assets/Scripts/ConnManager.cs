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

        // ���ӿ��� ����� ������� �̸��� �������� ����
        int num = Random.Range(0, 1000);
        PhotonNetwork.NickName = "Player" + num;

        // ���� ���� �� ������ Ŭ���̾�Ʈ�� ������ ���� �ڵ� ����ȭ
        PhotonNetwork.AutomaticallySyncScene = true;

        // ������ ���� ���� ������ ���� �� ������ ���� �ڵ� ����
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()  // ������ ���� ���� �� �۵�
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);    // �κ�� ����
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ���� �Ϸ�!");
        RoomOptions ro = new RoomOptions() // �� ���� �ɼ�
        {
            IsVisible = true, // �κ� �� ��Ͽ� ���
            IsOpen = true,  // ����ڵ��� �濡 ���� �� �ִ��� ����
            MaxPlayers = 8, // �ִ� ��� �ο� ��
        };
        PhotonNetwork.JoinOrCreateRoom("NetTest", ro, TypedLobby.Default); // �� ���� ������ �� ����
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("�� ����!");

        // �ݰ� 2m �̳��� Player �������� �����Ѵ�
        Vector2 originPos = Random.insideUnitCircle * 2.0f;
        // �Ķ���� �� Prefab, Position, Rotation / Quaternion.identity�� ȸ������ ����, ���� ��ǥ ���̳� �θ� ��ü ������ ���ĵ�
        PhotonNetwork.Instantiate("Player", new Vector3(originPos.x, 0, originPos.y), Quaternion.identity);
    }

    void Update()
    {
        
    }

}
