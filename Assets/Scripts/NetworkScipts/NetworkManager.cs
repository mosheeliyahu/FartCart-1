using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
    public const string VERSION = "v0.3";
    public string room = "myroom";
    public string preFabPlayer;
    public Transform startPoint1;
    public Transform startPoint2;

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(VERSION);
    }

    void OnJoinedLobby()
    {
        RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom(room, roomOptions, TypedLobby.Default);
    }

    void OnJoinedRoom()
    {
        int index= PhotonNetwork.playerList.Length - 1;
        Transform[] pos = {startPoint1, startPoint2};
        PhotonNetwork.Instantiate(preFabPlayer, pos[index].position, pos[index].rotation, 0);
        //if(temp.GetComponent<PhotonView>().isMine) temp.GetComponent<steering>().playerType = index;
    }

}
