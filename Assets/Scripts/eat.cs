using UnityEngine;
using System.Collections;

public class eat : MonoBehaviour
{
    public int type;
    // Use this for initialization

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PhotonView>().isMine)
            {
                other.GetComponent<steering>().addFood(type);
                gameObject.SetActive(false);
            }
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }

    }

    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 5, 0));
    }
}
