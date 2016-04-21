using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelNumberScript : MonoBehaviour {

    private string currLevel;
    private Text currentLevel;
    public int lvlperStratum;


    void Awake ()
    {
        int lvl = Application.loadedLevel;
        currentLevel = GetComponentInChildren<Text>();
        currLevel = ((lvl - 1) / (lvlperStratum) + 1).ToString("0") + " - " + ((lvl - 1) % (lvlperStratum) + 1).ToString("0");
        currentLevel.text = "Current Level : " + currLevel;
    }
	// Use this for initialization
	void Start () {
     
	}
    void OnLevelWasLoaded(int level)
    {
        int lvl = level;
        currentLevel = GetComponentInChildren<Text>();
        currLevel = ((lvl - 1) / (lvlperStratum) + 1).ToString("0") + " - " + ((lvl - 1) % (lvlperStratum) + 1).ToString("0");
        currentLevel.text = "Current Level : " + currLevel;
    }

	// Update is called once per frame
	void Update () {
	}
}
