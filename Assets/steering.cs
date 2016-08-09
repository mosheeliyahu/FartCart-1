using UnityEngine;
using System.Collections;

public class steering : MonoBehaviour
{
    public float turnSpeed, speed;

    void Start()
    {

    }

    void Update()
    {
        var steer = -Input.GetAxis("Horizontal");
        var gas = -Input.GetAxis("Vertical");


        if (gas != 0)
        {
            float moveDist = gas * speed * Time.deltaTime;
            float turnAngle = steer * turnSpeed * Time.deltaTime * gas;
            //now apply 'em, starting with the turn
            // transform.rotation.eulerAngles.y += turnAngle;
            //and now move forward by moveVect
            transform.Rotate(new Vector3(0,turnAngle, 0));
            transform.Translate(Vector3.forward * moveDist);
        }
    }
}