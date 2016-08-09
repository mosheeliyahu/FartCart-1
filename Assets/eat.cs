using UnityEngine;
using System.Collections;

public class eat : MonoBehaviour
{

    // Use this for initialization
    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<AudioSource>().Play();
        Destroy(this.gameObject);
        other.GetComponent<steering>().speed += 2;
    }
}
