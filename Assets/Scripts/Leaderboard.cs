using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour {

    public Text t1, t2, t3;
    public int maxToDisplay = 5;

    dreamloLeaderBoard dl;
    List<dreamloLeaderBoard.Score> scoreList;


    void Start()
    {
        dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        dl.LoadScores();
    }

    void OnGUI()
    {
        //GUILayout.BeginArea(fillRect.GetComponent<RectTransform>().rect);
        scoreList = dl.ToListLowToHigh();

        if (scoreList == null)
        {
            t2.text="loading...";
        }
        else
        {
            int count = 0;
            t1.text = "";
            t2.text = "";
            t3.text = "";
            foreach (dreamloLeaderBoard.Score currentScore in scoreList)
            {
                count++;
                t1.text += count + ".\n";
                t2.text += currentScore.playerName + "\n";
                float time = (float)currentScore.score / 100;
                t3.text += FormatTime(time)+ "\n";
                if (count >= maxToDisplay) break;
            }
        }
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

