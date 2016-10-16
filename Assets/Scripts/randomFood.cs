using UnityEngine;
using System.Collections;
using System.Linq;


public class randomFood : MonoBehaviour {

    public static readonly string[] foodName = { "Cake", "Donuts", "Hambuger", "HamEgg" ,"IceCream", "Milk", "Waffle"};
    MeshFilter[] mfs;
    Vector3[] vec;

    float time;
    public float timeForFood;
    GameObject track,food;

    void Start() {
        mfs = gameObject.GetComponentsInChildren<MeshFilter>();
        food = GameObject.FindGameObjectWithTag("food");
        time = 0;
    }

	
	// Update is called once per frame
	void FixedUpdate () {
        if (PhotonNetwork.isMasterClient)
        {
            if (time <= 0)
            {
                time = timeForFood;
                int i = Random.Range(0, transform.childCount);
                track = transform.GetChild(i).gameObject;
                vec = track.GetComponent<MeshFilter>().mesh.vertices;

                Vector3 pos1 = track.transform.TransformPoint(vec[Random.Range(0, vec.Length)]);
                Vector3 pos2 = track.transform.TransformPoint(vec[Random.Range(0, vec.Length)]);
                Vector3 pos = new Vector3((pos1.x + pos2.x) / 2, 0, (pos1.z + pos2.z) / 2);
                PhotonNetwork.InstantiateSceneObject(foodName[Random.Range(0, foodName.Length)], pos, Quaternion.identity, 0,null).transform.SetParent(food.transform);
            }
            time -= Time.fixedDeltaTime;
        }
    }



}
