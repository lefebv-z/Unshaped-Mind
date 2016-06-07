using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelNumberEndScript : MonoBehaviour {

    private Text currentLevel;
    private GameManager gm;

	void Start()
    {
        currentLevel = GetComponentInChildren<Text>();
        gm = (GameManager)(GameObject.FindObjectOfType(typeof(GameManager)));
		if (gm != null)
			currentLevel.text = "" + gm.getLevel ().ToString () + " / " + gm.getLevelsInCurrentStratum();
		else
			currentLevel.text = "1 / 1";
	}
}

