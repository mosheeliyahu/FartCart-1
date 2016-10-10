using UnityEngine;
using System.Collections;

public class location : MonoBehaviour
{
    public int trackPos;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PhotonView>().isMine)
        {
            other.GetComponent<steering>().location(trackPos);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PhotonView>().isMine)
        {
            other.GetComponent<steering>().exitLocation(trackPos);
        }
    }

}
