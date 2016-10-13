using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class loadSeanOnClick : MonoBehaviour {
    dreamloLeaderBoard dl;
    public void loadByIndex(int index){
		SceneManager.LoadScene(index);
	}

    public void sendData(int index,string name,float score)
    {
        dl= dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        dl.AddScore(name, Mathf.FloorToInt(score));
        loadByIndex(0);
    }

}
