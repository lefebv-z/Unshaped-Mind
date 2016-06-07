using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditScript : MonoBehaviour {

    public float changeTime;
    private float currTime;
    private string initialText;
    private string otherText;

    public Text pressButton;
    private string pressButtonText;
    private string pressButtonOldText;
    private Text mt;

	// Use this for initialization
    void Start()
    {
        otherText = "Musics: \"Floating Cities\" \"Thunderbird\" \"Fairytale Waltz\" \"Orion 300XB\" Kevin MacLeod (incompetech.com)\nLicensed under Creative Commons: By Attribution 3.0 License \nhttp://creativecommons.org/licenses/by/3.0/";
        currTime = 0.0f;
        mt = this.GetComponent<Text>();
        pressButtonText = "";
    }
	
	// Update is called once per frame
    void Update()
    {
        if (currTime >= changeTime)
        {
            currTime = 0.0f;
            pressButtonOldText = pressButton.text;
            pressButton.text = pressButtonText;
            pressButtonText = pressButtonOldText;
        }
        currTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            initialText = mt.text;
            mt.text = otherText;
            otherText = initialText;
        }
    }
}
