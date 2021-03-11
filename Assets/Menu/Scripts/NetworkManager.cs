using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject panelConnecting = null;
    [SerializeField] GameObject panelRooms = null;
    [SerializeField] GameObject panelErrorRoom = null;

    public static NetworkManager networkManager;

    public bool isConnected = false;

    public int activeRoom = 0;

    void Awake()
    {
        networkManager = this;
    }

    public void ConnectToServer()
    {
        PhotonNetwork.GameVersion = "2021.0311";
        PhotonNetwork.ConnectUsingSettings();
        
        panelConnecting.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        isConnected = true;

        panelConnecting.SetActive(false);
        panelRooms.SetActive(true);
    }

    public void DisconnectFromServer()
    {
        activeRoom = 0;
        isConnected = false;

        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        activeRoom = 0;
        isConnected = false;
    }

    public void JoinRoom(int roomNumber)
    {
        activeRoom = roomNumber;
        PhotonNetwork.JoinOrCreateRoom(roomNumber.ToString(), new RoomOptions {MaxPlayers = 2}, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        panelRooms.SetActive(false);
    }

    public void CloseRoomError()
    {
        panelErrorRoom.SetActive(false);
        panelRooms.SetActive(true);
    }
}
