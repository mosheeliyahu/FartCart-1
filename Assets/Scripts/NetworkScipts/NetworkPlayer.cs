using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkPlayer : Photon.MonoBehaviour {
    public Color green, blue;
    public GameObject myCamera;
    public float lerpSmothing = 10.0f;
    bool isAlive = false , start=false;
    public Vector3 position;
    public Quaternion rotation;
    public float countdownTime = 5.49f;
    Text heading;
    public int score;
    public GameObject frontAxle, rearAxle;
    //public GameObject leaderInput;

    void Start()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("sceneUnload"))
        {
            go.SetActive(false);
        }

        foreach (GameObject go in GameObject.FindGameObjectWithTag("manager").GetComponent<NetworkManager>().load)
        {
            go.SetActive(true);
        }
        heading = GameObject.FindGameObjectWithTag("heading").GetComponent<Text>();
        if (photonView.isMine) {
            myCamera.SetActive(true);
            GetComponent<steering>().enabled = true;
            GetComponent<BoxCollider>().enabled = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            GetComponent<Rigidbody>().useGravity = true;
            if (PhotonNetwork.isMasterClient)
            {
                GetComponent<steering>().playerType = 1;
                frontAxle.GetComponent<Renderer>().material.color = blue;
                rearAxle.GetComponent<Renderer>().material.color = blue;
            }
        }
        else {
            isAlive = true;
            StartCoroutine("Alive");
            if (!PhotonNetwork.isMasterClient)
            {
                frontAxle.GetComponent<Renderer>().material.color = blue;
                rearAxle.GetComponent<Renderer>().material.color = blue;
            }

        }
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        if (PhotonNetwork.playerList.Length == 2)
        {
            photonView.RPC("startCountdown", PhotonTargets.All, PhotonNetwork.time);
        }
    }

    [PunRPC]
    void startCountdown(double cur)
    {
        countdownTime -= (float)(PhotonNetwork.time - cur);
        start = true; 
    }


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if (stream.isWriting){
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(score);
        }
        else if (stream.isReading){
            position = (Vector3)stream.ReceiveNext();
            rotation = (Quaternion)stream.ReceiveNext();
            score = (int)stream.ReceiveNext();
        }
    }

    IEnumerator Alive(){
        while (isAlive)
        {
           try {
                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * lerpSmothing);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * lerpSmothing);
            }
            catch
            {
                //Debug.Log(rotation);
            }
            yield return null;
        } 
    }

    void FixedUpdate()
    {
        if (start)
        {
            heading.text = Mathf.RoundToInt(countdownTime).ToString();
            countdownTime -= Time.fixedDeltaTime;
            if (countdownTime <= 0)
            {
                start = false;
                heading.text = "START!";
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                for (int i=0; i < players.Length; i++)
                {
                    players[i].GetComponent<steering>().speed = 10;
                    players[i].GetComponent<steering>().other = players[1-i];
                }
            }
        }

    }


}
