using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelTimerScript : MonoBehaviour {

    private float diffTime;
    private float currTime;
    private Text currentTime;
    private string minutes;
    private string seconds;

	// Use this for initialization
	void Start () {
        currentTime = GetComponentInChildren<Text>();
        diffTime = 0.0f;
	}
    void OnLevelWasLoaded(int level)
    {
        if (Time.deltaTime > 1.0f)
        diffTime = Time.deltaTime;
    }
	// Update is called once per frame
	void Update () {
        currTime += Time.deltaTime - diffTime;
        if (currTime >= 6000.0f)
        {
            minutes = Mathf.RoundToInt(Mathf.Floor(currTime / 60.0f)).ToString("000");
            seconds = Mathf.RoundToInt(Mathf.Floor(currTime % 60.0f)).ToString("00");
            currentTime.text = "Time : " + minutes + " : " + seconds;
        }
        else if (currTime >= 60000.0f)
        {
            currentTime.text = "Time : -- : --";
        }
        else
        {
            minutes = Mathf.RoundToInt(Mathf.Floor(currTime / 60.0f)).ToString("00");
            seconds = Mathf.RoundToInt(Mathf.Floor(currTime % 60.0f)).ToString("00");
            currentTime.text = "Time : " + minutes + " : " + seconds;
        }
	}
}
