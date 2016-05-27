using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelNumberScript : MonoBehaviour {

    private Text currentLevel;
    private GameManager gm;

    void Awake ()
    {
		OnLevelWasLoaded ();
	}

	void OnLevelWasLoaded()
    {
        currentLevel = GetComponentInChildren<Text>();
        gm = (GameManager)(GameObject.FindObjectOfType(typeof(GameManager)));
		if (gm != null)
			currentLevel.text = "" + gm.getStratum ().ToString () + " - " + gm.getLevel ().ToString ();
		else
			currentLevel.text = "1 - 1";
	}

    public int GetStratum()
    {
		return gm.getStratum ();
    }
}
