using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
    public const string VERSION = "v0.4";
    //public string room = "myroom";
    public string preFabPlayer;
    public Transform startPoint1;
    public Transform startPoint2;
    public GameObject[] load;

    // Use this for initialization
    void Start()
    {
        load = GameObject.FindGameObjectsWithTag("sceneLoad");
        foreach (GameObject go in load)
        {
            go.SetActive(false);
        }
        PhotonNetwork.ConnectUsingSettings(VERSION);
    }

    void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    void OnJoinedRoom()
    {
        int index= PhotonNetwork.playerList.Length - 1;
        Transform[] pos = {startPoint1, startPoint2};
        PhotonNetwork.Instantiate(preFabPlayer, pos[index].position, pos[index].rotation, 0);
        //if(temp.GetComponent<PhotonView>().isMine) temp.GetComponent<steering>().playerType = index;
    }

    void OnPhotonRandomJoinFailed()
    {
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(null, roomOptions, TypedLobby.Default); //maxPlayer limit can be any amount
    }

}
