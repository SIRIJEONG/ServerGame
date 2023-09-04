using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "2";

    public Text connectionInfoText;
    public Button joinButton;

    // Start is called before the first frame update
    void Start()
    {
        //���ӿ� �ʿ��� ���� ����
        PhotonNetwork.GameVersion = gameVersion;
        //������ ������ ������ ���� ���� �õ�
        PhotonNetwork.ConnectUsingSettings();
        //�� ���� ��ư ��� ��Ȱ��ȭ
        joinButton.interactable = false;
        //���� �õ������� �ؽ�Ʈ�� ǥ��
        connectionInfoText.text = "Connect master server";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ������ ���� ���� ������ �ڵ� ����
    public override void OnConnectedToMaster()
    {
        // �� ���� ��ư ��Ȱ��ȭ
        joinButton.interactable = true;
        //���� ���� ǥ��
        connectionInfoText.text = "Online : Connected to master server succeed";
    }

    // ������ ���� ���� ���н� �ڵ� ����
    public override void OnDisconnected(DisconnectCause cause)
    {
        //�� ���� ��ư ��Ȱ��ȭ 
        joinButton.interactable = false;
        //���� ���� ǥ��
        connectionInfoText.text = string.Format("{0}\n{1}", "offline : DisConnected : to master server", "Retry connect now...");
        //������ �������� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // �� ���� �õ�
    public void Connect()
    {
        //�ߺ� ���� �õ��� ���� ���� ���� ��ư ��� ��Ȱ��ȭ
        joinButton.interactable = false;
        //������ ������ ���� ���̶��
        if (PhotonNetwork.IsConnected)
        {
            //�뿡 ����
            connectionInfoText.text = "connected to Room";

            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //������ ������ ���� ���� �ƴ϶�� ������ ������ ���� �õ�
            connectionInfoText.text = string.Format("{0}\n{1}", "offline : DisConnected : to master server", "Retry connect now...");
            //������ �������� ������ �õ� 
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // (�� ���� ����)���� �� ������ ������ ��� �ڵ� ����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //���� ���� ǥ��
        connectionInfoText.text = "Nothing to empty room , creat new room...";
        //�ִ� 4���� ���� ������ �� �� ����
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    // �뿡 ���� �Ϸ�� ��� �ڵ� ����
    public override void OnJoinedRoom()
    {
        //���� ���� ǥ�� 
        connectionInfoText.text = "Successes joined room";
        //��� �� ������ Main ���� �ε��ϰ� �� 
        PhotonNetwork.LoadLevel("PlayScene");
    }
}
