using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PhotonStudy : MonoBehaviourPunCallbacks
{
    /*[SerializeField] private TextMeshProUGUI StatusText;
    public TMP_InputField roomInput, NickNameInput;
    [SerializeField] private TextMeshProUGUI UserCountTMP;
    [SerializeField] private GameObject ConnectPanel;
    [SerializeField] private GameObject LobbyPanel;
    [SerializeField] private GameObject RoomPanel;
    [SerializeField] private TextMeshProUGUI WelcomeText;

    #region 서버 연결
    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
    }

    private void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString(); // 실행상태를 알리는 메서드
        UserCountTMP.text = PhotonNetwork.CountOfPlayers.ToString();
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();  // Photon서버에 연결. -> OnConnectedToMaster()로 연결
    public override void OnConnectedToMaster() // 서버 연결시 실행되는 콜백 함수
    {
        Debug.Log("서버 접속 완료");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby() {
        LobbyPanel.SetActive(true);
        ConnectPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
        //myList.Clear();
    }
    #endregion

    #region 방 관련
    public void CreateRoom() => PhotonNetwork.CreateRoom(roomInput.text == "" ? "Room" + Random.Range(0, 100) : roomInput.text, new RoomOptions { MaxPlayers = 2 });
    //public override void OnCreatedRoom() => Debug.Log("Photon 방 생성 완료!"); // PhotonNetwork.CreateRoom이 완료되었을 때 실행
    //public override void OnCreateRoomFailed(short returnCode, string message) => Debug.Log("방 만들기 실패");
    public override void OnJoinedRoom()
    {
        RoomPanel.SetActive(true);
    }
     

    #endregion
    public void JoinRoom() => PhotonNetwork.JoinRoom(roomInput.text);
    public override void OnJoinRoomFailed(short returnCode, string message) => Debug.Log("방 참가 실패");

    

    public void JoinOrCreateRoom() => PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 }, null);
    //방이 없으면 만들고 있으면 참가.

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    public override void OnJoinRandomFailed(short returnCode, string message) => Debug.Log("방 랜덤 참가 실패");

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();


    [ContextMenu("정보")] // Componenet를 우클릭하면 함수를 실행하는 듯.
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("현재 방 인원 수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.Log("현재 방 최대 인원 수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            Debug.Log(playerStr);
        }
        else
        {
            Debug.Log("접속한 인원 수  : " + PhotonNetwork.CountOfPlayers);
            Debug.Log("방 개수  : " + PhotonNetwork.CountOfPlayers);
            Debug.Log("모든 방에 있는 인원 수  : " + PhotonNetwork.CountOfPlayersInRooms);
            Debug.Log("로비에 있는지?  : " + PhotonNetwork.InLobby);
            Debug.Log("연결됐는지?  : " + PhotonNetwork.IsConnected);
        }
    }*/
    
}
