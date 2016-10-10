using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class steering : MonoBehaviour
{
    public bool test;
    public float turnSpeed, speed,fartReduce, blinkSpeed=2;
    public int playerType;
    public AudioSource fartAudio, barpAudio, hiccupAudio;
    public Transform centerOfMass;
    public GameObject other, body;
    Quaternion lastGoodRotate;
    ProgressBar.ProgressRadialBehaviour bar;
    Text lapText,timeText, heading;
    NetworkPlayer np;
    GameObject[] track;

    bool worngWay = false, respan = false, finish = false;
    int curPos = 16;
    int lap = 0;
    int score = 0;
    float startTime ,alphaSpeed, worngWayTime ,respanTime;

    void Start()
    {
        alphaSpeed = 1;
        bar = GameObject.FindGameObjectWithTag("bar").GetComponent<ProgressBar.ProgressRadialBehaviour>();
        GetComponent<Rigidbody>().centerOfMass = centerOfMass.localPosition;
        heading = GameObject.FindGameObjectWithTag("heading").GetComponent<Text>();
        lapText = GameObject.FindGameObjectWithTag("Lap").GetComponent<Text>();
        timeText = GameObject.FindGameObjectWithTag("Time").GetComponent<Text>();
        np = GetComponent<NetworkPlayer>();
        track = GameObject.FindGameObjectsWithTag("track");
        track = track.OrderBy(x => x.GetComponent<location>().trackPos).ToArray<GameObject>();  
    }

    public void addFood(int foodPick)
    {
        if (foodPick < 0)
        {
            alphaSpeed = 1;
            barpAudio.Play();
        }
        else
        {
            float power = foodPick;
            if (foodPick % 2 == 1) power += 1;
            power /= 5;
            if (foodPick % 2 == playerType)
            {
                alphaSpeed += power;
                fartAudio.pitch = 1.2f + power;
                fartAudio.Play();
            }
            else
            {
                if (alphaSpeed > 2 ) alphaSpeed = 2;
                alphaSpeed -= (power/10);
                hiccupAudio.Play();
                if (alphaSpeed < 1) alphaSpeed = 1;
            }
        }
    }

    public void location(int trackPos)
    {
        Debug.Log("enter:" + trackPos);
        if (trackPos == 1 && curPos == 16)
        {
            lap++;
            curPos = 1;
            setLapText();
            worngWayOff();
        }
        else
        {
            if (curPos + 1 == trackPos)
            {
                lastGoodRotate = transform.rotation;
                curPos = trackPos;
                worngWayOff();
            }
            else if (!worngWay && lap>0)
            {
                worngWayOn();
            }
        }
        if (lap == 4)
        {
            lap = -1;
            finish = true;
        }
    }

    public void exitLocation(int trackPos)
    {
        Debug.Log("exit:" + trackPos);
        if ( trackPos== curPos)
        {
            worngWayOn();
        }
    }

    void setLapText()
    {
        switch (lap)
        {
            case 1:
                lapText.text = "1st LAP";
                startTime = Time.time;
                break;
            case 2:
                lapText.text = "2nd LAP";
                break;
            case 3:
                lapText.text = "LAST LAP";
                //lapText.color= new Color("#55555");
                break;
            case 4:
                lapText.text = "FINISH!";
                break;
        }
    }

    void FixedUpdate()
    {
        if (finish)
        {
            speed -= 4 * Time.fixedDeltaTime;
            bar.Value -= 4 * Time.fixedDeltaTime;
            if (speed < 0) speed = 0;

            Color temp = heading.color;
            temp.a = Mathf.Round(Mathf.PingPong(Time.time * blinkSpeed, 1.0f));
            heading.color = temp;
        }
        if(speed > 0 || respan)
        {
            float steer;
            #if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
                    steer = Input.acceleration.x;
            #else
                        var steer= Input.GetAxis("Horizontal"); 
            #endif
              if (test) steer = Input.GetAxis("Horizontal");
             transform.Rotate(new Vector3(0, steer * turnSpeed * Time.fixedDeltaTime, 0));
        }
        if (!respan)
        {
            float betaSpeed = alphaSpeed;
            if (alphaSpeed > 2) betaSpeed = 2;
            transform.Translate(Vector3.forward * Time.fixedDeltaTime * speed * betaSpeed);
            bar.Value = Mathf.Round((betaSpeed - 1) * 100);
            if (alphaSpeed > 1) alphaSpeed *= fartReduce;
        }
        else
        {
            float diff = Time.time - respanTime;
            if (diff > 1.5f)
            {
                respan=false;
                body.SetActive(true);
            }
            else
            {
                body.SetActive(Mathf.RoundToInt(Mathf.PingPong(Time.time * blinkSpeed*2, 1.0f))>0?true:false);
            }
        }
        if (lap > 0) timeText.text = FormatTime(Time.time - startTime);

        if (worngWay)
        {
            float diff = Time.time - worngWayTime;
            if (diff > 3) {
                // worngWayOff(); not working on time.. why??
                worngWayOff();

                transform.position = track[(curPos - 1) % track.Length].transform.position;
                transform.rotation = lastGoodRotate;
                respan = true;
                respanTime = Time.time;
            }
            else
            {
                Color temp = heading.color;
                temp.a= Mathf.Round(Mathf.PingPong(Time.time * blinkSpeed, 1.0f));
                heading.color = temp;
            }
        }
        if(!finish) updateRank();
    }

    string FormatTime(float time)
    {
        int  minutes = (int)Mathf.Floor(time / 60.0f);
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


    void worngWayOn()
    {
        worngWay = true;
        worngWayTime = Time.time;
        heading.text = "WORNG WAY!";
    }

    void worngWayOff()
    {
        worngWay = false;
        Color temp = heading.color;
        temp.a = 1;
        heading.color = temp;
        
    }

    void updateRank()
    {
        score = 100000 * lap + 1000 * curPos - distScore();
        np.score = score;
        if (other != null && !worngWay)
        {
            //Debug.Log("this: " + score + " other:" + other.GetComponent<NetworkPlayer>().score);
            if (score > other.GetComponent<NetworkPlayer>().score)
            {
                heading.text = "1st";
            }
            else
            {
                heading.text = "2nd";
            }
        }

    }
    int distScore() { 
        return Mathf.RoundToInt(Mathf.Abs(Vector3.Distance(transform.position, track[curPos % track.Length].transform.position)));
    }
}

