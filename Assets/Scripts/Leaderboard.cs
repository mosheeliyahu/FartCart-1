using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour {
    dreamloLeaderBoard dl;
    List<dreamloLeaderBoard.Score> scoreList;
    GameObject fillRect;
    Text t1;
    void Start()
    {
        dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        dl.LoadScores();
        fillRect = GameObject.FindGameObjectWithTag("leaderboardFill");
        t1 = GameObject.FindGameObjectWithTag("leaderboardFill").GetComponent<Text>();
    }

    void OnGUI()
    {
        //GUILayout.BeginArea(fillRect.GetComponent<RectTransform>().rect);
        GUILayout.Label("High Scores:");
        scoreList = dl.ToListLowToHigh();

        if (scoreList == null)
        {
            GUILayout.Label("(loading...)");
        }
        else
        {
            int maxToDisplay = 5;
            int count = 0;
            string str = "";

            foreach (dreamloLeaderBoard.Score currentScore in scoreList)
            {
                count++;
                str+=currentScore.playerName + ":    ";
                str += currentScore.score.ToString() + " sec \n";
                if (count >= maxToDisplay) break;
            }
            t1.text = str;
        }
    }
}

