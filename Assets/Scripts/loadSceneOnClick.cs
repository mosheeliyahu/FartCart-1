using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadSceneOnClick : MonoBehaviour {
    public Text username;
    public float score;
    dreamloLeaderBoard dl;

    public void loadByIndex(int index){
        if (index == 0) PhotonNetwork.Disconnect();
		SceneManager.LoadScene(index);
	}

    public void sendData(int index)
    {
        if (username.text.Length == 0) return;
        dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        dl.AddScore(username.text, Mathf.FloorToInt(score*100));
        DontDestroyOnLoad(dl);
        loadByIndex(0);
    }

}
