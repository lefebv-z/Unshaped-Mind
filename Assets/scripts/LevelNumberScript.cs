using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelNumberScript : MonoBehaviour {

    private string currLevel;
    private Text currentLevel;
    public int lvlperStratum;
    private GameManager gm;
    public int lvl;

    void Awake ()
    {
        lvlperStratum = PlayerPrefs.GetInt("levelsPerStartum");
        lvl = 1;
        gm = (GameManager)(GameObject.FindObjectOfType(typeof(GameManager)));
        if (gm != null)
            lvl = gm.getLevel();
        currentLevel = GetComponentInChildren<Text>();
        currLevel = ((lvl - 1) / (lvlperStratum) + 1).ToString("0") + " - " + ((lvl - 1) % (lvlperStratum) + 1).ToString("0");
        currentLevel.text = "" + currLevel;
    }
	// Use this for initialization
	void Start () {
     
	}
    void OnLevelWasLoaded(int level)
    {
        lvl = 1;
        gm = (GameManager)(GameObject.FindObjectOfType(typeof(GameManager)));
        if (gm != null)
            lvl = gm.getLevel();
        currentLevel = GetComponentInChildren<Text>();
        currLevel = ((lvl - 1) / (lvlperStratum) + 1).ToString("0") + " - " + ((lvl - 1) % (lvlperStratum) + 1).ToString("0");
        currentLevel.text = "" + currLevel;
    }

    public int GetStratum()
    {
        return ((lvl - 1) / (lvlperStratum) + 1);
    }

	// Update is called once per frame
	void Update () {
	}
}
