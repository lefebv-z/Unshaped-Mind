using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditScript : MonoBehaviour {

    public float changeTime;
    private float currTime;
    private string initialText;
    private string otherText;
    private Text mt;

	// Use this for initialization
    void Start()
    {
        otherText = "Musics\n \"Floating Cities\" \"Thunderbird\" \"Fairytale Waltz\" \"Orion 300XB\" Kevin MacLeod (incompetech.com)\nLicensed under Creative Commons: By Attribution 3.0 License \nhttp://creativecommons.org/licenses/by/3.0/";
        currTime = 0.0f;
        mt = this.GetComponent<Text>();
    }
	
	// Update is called once per frame
    void Update()
    {
        if (currTime >= changeTime)
        {
            currTime = 0.0f;
            initialText = mt.text;
            mt.text = otherText;
            otherText = initialText;
        }
        currTime += Time.deltaTime;
    }
}
