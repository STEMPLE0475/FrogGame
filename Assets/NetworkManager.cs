using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using ExitGames.Client.Photon;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("ConnectPanel")]
    public TMP_InputField NickNameInput;
    public GameObject ConnectPanel;

    [Header("LobbyPanel")]
    public GameObject LobbyPanel;
    public TMP_InputField RoomInput;
    public TextMeshProUGUI WelcomeText;
    public TextMeshProUGUI LobbyInfoText;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public TextMeshProUGUI ListText;
    public TextMeshProUGUI RoomInfoText;
    public TextMeshProUGUI[] ChatText;
    public TMP_InputField ChatInput;
    public GameObject FrogCard;
    public GameObject TongueCard;
    public Button GameStartButton;
    public TextMeshProUGUI EnteredPlayerTMP; 

    [Header("ETC")]
    public TextMeshProUGUI StatusText;
    public PhotonView PV;

    [Header("GameScene")]
    public GameObject PlayerPrefab;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;



    #region 방리스트 갱신
    // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }

    void MyListRenewal()
    {
        // 최대페이지
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // 이전, 다음버튼
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }
    #endregion


    #region 서버연결
    void Awake() => Screen.SetResolution(1920, 1080, false);

    void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";
    }

    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        LobbyPanel.SetActive(true);
        RoomPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        WelcomeText.text = PhotonNetwork.LocalPlayer.NickName + "님 환영합니다";
        myList.Clear();
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause)
    {
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(false);
    }
    #endregion


    #region 방
    public void CreateRoom() => PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 2 });
    public override void OnCreatedRoom()
    {
        ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable()
        {
            { "FrogPlayer", null },
            { "TonguePlayer", null }
        };
        Debug.Log(roomProperties);
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom()
    {
        ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        string userID = PhotonNetwork.LocalPlayer.UserId.ToString();
        if (userID.Equals(roomProperties["FrogPlayer"]))
        {
            roomProperties["FrogPlayer"] = null;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
            Debug.Log("개구리 선택해제");
            // OnRoomPropertiesUpdate() 호출
        }

        if (userID.Equals(roomProperties["TonguePlayer"]))
        {
            roomProperties["TonguePlayer"] = null;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
            Debug.Log("혀 선택해제");
            // OnRoomPropertiesUpdate() 호출
        }

        PhotonNetwork.LeaveRoom();
        //방을 나가면 방을 참조하기 힘드므로, 방을 나가기 전에 정보를 갱신하고 나감. -> OnLeaveRoom()사용 X
    }

    public override void OnJoinedRoom()
    {
        RoomPanel.SetActive(true);
        RoomRenewal();
        ChatInput.text = "";
        for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
    }

    public override void OnCreateRoomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnJoinRandomFailed(short returnCode, string message) { RoomInput.text = ""; CreateRoom(); }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }

    void RoomRenewal()
    {
        ListText.text = "";
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            ListText.text += PhotonNetwork.PlayerList[i].NickName + ((i + 1 == PhotonNetwork.PlayerList.Length) ? "" : ", ");
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "명 / " + PhotonNetwork.CurrentRoom.MaxPlayers + "최대";

        UpdateCharactorSelectionUI();
    }

    #endregion

    #region 캐릭터 선택
    void UpdateCharactorSelectionUI()
    {
        if (PhotonNetwork.InRoom)
        {
            ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
            string playerID = PhotonNetwork.LocalPlayer.UserId.ToString();
            if (roomProperties["FrogPlayer"] != null && playerID.Equals(roomProperties["FrogPlayer"]))
            {
                FrogCard.GetComponent<CharactorCard>().EnableCard_Cancel();
            }
            if (roomProperties["FrogPlayer"] != null && !playerID.Equals(roomProperties["FrogPlayer"]))
            {
                FrogCard.GetComponent<CharactorCard>().DisableCard();
            }
            if (roomProperties["TonguePlayer"] != null && playerID.Equals(roomProperties["TonguePlayer"]))
            {
                TongueCard.GetComponent<CharactorCard>().EnableCard_Cancel();
            }
            if (roomProperties["TonguePlayer"] != null && !playerID.Equals(roomProperties["TonguePlayer"]))
            {
                TongueCard.GetComponent<CharactorCard>().DisableCard();
            }
            if (roomProperties["FrogPlayer"] == null)
            {
                FrogCard.GetComponent<CharactorCard>().EnableCard_Select();
            }
            if (roomProperties["TonguePlayer"] == null)
            {
                TongueCard.GetComponent<CharactorCard>().EnableCard_Select();
            }

            if (roomProperties["TonguePlayer"] != null && roomProperties["FrogPlayer"] != null)
            {
                GameStartButton.interactable = true;
            }
            else
            {
                GameStartButton.interactable = false;
            }
        }
        
    }

    public void SelectCharactor(int number)
    {
        ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        if (roomProperties["FrogPlayer"] == null && number == 0)
        {
            if (roomProperties["TonguePlayer"] != null)
            {
                if (roomProperties["TonguePlayer"].Equals(PhotonNetwork.LocalPlayer.UserId.ToString()))
                {
                    roomProperties["TonguePlayer"] = null;
                }
            }
            
            roomProperties["FrogPlayer"] = PhotonNetwork.LocalPlayer.UserId.ToString();
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
            Debug.Log("개구리를 선택했습니다");
            // OnRoomPropertiesUpdate() 호출
        }

        if (roomProperties["TonguePlayer"] == null && number == 1)
        {
            if (roomProperties["FrogPlayer"] != null)
            {
                if (roomProperties["FrogPlayer"].Equals(PhotonNetwork.LocalPlayer.UserId.ToString()))
                {
                    roomProperties["FrogPlayer"] = null;
                }
            }
            
            roomProperties["TonguePlayer"] = PhotonNetwork.LocalPlayer.UserId.ToString();
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
            Debug.Log("혀를 선택했습니다");
            // OnRoomPropertiesUpdate() 호출
        }
    }

    public void CancelCharactor(int number)
    {
        ExitGames.Client.Photon.Hashtable roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        string userID = PhotonNetwork.LocalPlayer.UserId.ToString();
        if (userID.Equals(roomProperties["FrogPlayer"]) && number == 0)
        {
            roomProperties["FrogPlayer"] = null;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
            Debug.Log("개구리 선택해제");
            // OnRoomPropertiesUpdate() 호출
        }

        if (userID.Equals(roomProperties["TonguePlayer"]) && number == 1)
        {
            roomProperties["TonguePlayer"] = null;
            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
            Debug.Log("혀 선택해제");
            // OnRoomPropertiesUpdate() 호출
        }
    }

    public void StartGame()
    {
        PV.RPC("StartGameRPC", RpcTarget.All);
    }


    [PunRPC]
    void StartGameRPC()
    {
        
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(false);
        ConnectPanel.SetActive(false);
        PhotonNetwork.Instantiate("Player", Vector3.up * 5f, Quaternion.identity);
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        Debug.Log("방 속성 변경");
        RoomRenewal();
    }

    #endregion

    #region 채팅
    public void Send()
    {
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
        ChatInput.text = "";
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
    }
    #endregion
}