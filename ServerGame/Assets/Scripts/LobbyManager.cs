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
        //접속에 필요한 정보 설정
        PhotonNetwork.GameVersion = gameVersion;
        //설정한 정보로 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();
        //룸 접속 버튼 잠시 비활성화
        joinButton.interactable = false;
        //접속 시도중임을 텍스트로 표시
        connectionInfoText.text = "Connect master server";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 마스터 서버 접속 성공시 자동 실행
    public override void OnConnectedToMaster()
    {
        // 룸 접속 버튼 비활성화
        joinButton.interactable = true;
        //접속 정보 표시
        connectionInfoText.text = "Online : Connected to master server succeed";
    }

    // 마스터 서버 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        //룸 접속 버튼 비활성화 
        joinButton.interactable = false;
        //접속 정보 표시
        connectionInfoText.text = string.Format("{0}\n{1}", "offline : DisConnected : to master server", "Retry connect now...");
        //마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 룸 접속 시도
    public void Connect()
    {
        //중복 접속 시도를 막기 위해 접속 버튼 잠시 비활성화
        joinButton.interactable = false;
        //마스터 서버에 접속 중이라면
        if (PhotonNetwork.IsConnected)
        {
            //룸에 접속
            connectionInfoText.text = "connected to Room";

            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //마스터 서버에 접속 중이 아니라면 마스터 서버에 접속 시도
            connectionInfoText.text = string.Format("{0}\n{1}", "offline : DisConnected : to master server", "Retry connect now...");
            //마스터 서버로의 재접속 시도 
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // (빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //접속 상태 표시
        connectionInfoText.text = "Nothing to empty room , creat new room...";
        //최대 4명을 수용 가능한 빈 방 생성
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        //접속 상태 표시 
        connectionInfoText.text = "Successes joined room";
        //모든 룸 참가자 Main 씬을 로드하게 함 
        PhotonNetwork.LoadLevel("PlayScene");
    }
}
