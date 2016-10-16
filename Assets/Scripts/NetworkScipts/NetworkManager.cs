using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {
    public const string VERSION = "v0.4";
    //public string room = "myroom";
    public string preFabPlayer;
    public Transform startPoint1;
    public Transform startPoint2;
    private GameObject[] gameParts;
    private GameObject UICamera, loadingCanvas, endGameCanvas;
    private state gameState;
    public Text finishText;

    public enum state
    {
        loading,
        game,
        endGame
    }

    // Use this for initialization
    void Start()
    {
        gameParts = GameObject.FindGameObjectsWithTag("inGameComponnents");
        UICamera = GameObject.FindGameObjectWithTag("2ndCamera");
        loadingCanvas = GameObject.FindGameObjectWithTag("loadingCanvas");
        endGameCanvas = GameObject.FindGameObjectWithTag("leaderboardPopup");
        setState(state.loading);
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
    
    public void setState(state newState,int place=0,float time=0)
    {
        switch (newState)
        {
            case state.loading:
                foreach (GameObject go in gameParts)
                {
                    go.SetActive(false);
                }
                endGameCanvas.SetActive(false);
                break;
            case state.game:
                UICamera.SetActive(false);
                loadingCanvas.SetActive(false);
                foreach (GameObject go in gameParts)
                {
                    go.SetActive(true);
                }
                break;
            case state.endGame:
                foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
                {
                    go.SetActive(false);
                }
                foreach (GameObject go in gameParts)
                {
                    go.SetActive(false);
                }
                UICamera.SetActive(true);
                endGameCanvas.SetActive(true);
                if (place == 1)
                {
                    finishText.text = "YOU WIN!\n";
                }
                else
                {
                    finishText.text = "YOU LOSE:(\n";
                }
                finishText.text += "Your time is: " + FormatTime(time);
                GameObject.FindGameObjectWithTag("leaderboardName").GetComponent<loadSceneOnClick>().score = time;
                PhotonNetwork.Disconnect();
                break;
        }
        gameState = newState;
    }

    string FormatTime(float time)
    {
        int minutes = (int)Mathf.Floor(time / 60.0f);
        int seconds = (int)Mathf.Floor(time - minutes * 60.0f);
        float milliseconds = time - Mathf.Floor(time);
        milliseconds = Mathf.Floor(milliseconds * 100.0f);


        string sMinutes = "00" + minutes.ToString();
        sMinutes = sMinutes.Substring(sMinutes.Length - 2);
        string sSeconds = "00" + seconds.ToString();
        sSeconds = sSeconds.Substring(sSeconds.Length - 2);
        string sMilliseconds = milliseconds.ToString();
        //sMilliseconds = sMilliseconds.Substring(sMilliseconds.Length - 3, sMilliseconds.Length - 1);

        return sMinutes + ":" + sSeconds + ":" + sMilliseconds;

    }
}
