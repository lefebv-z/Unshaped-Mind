using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditScript : MonoBehaviour {

    public float changeTime;
    private string initialText;
    private string otherText;

	private Image pressSpaceImage;
    private Text mt;

	// Use this for initialization
    void Start()
    {
        otherText = "Musics: \"Floating Cities\" \"Thunderbird\" \"Fairytale Waltz\" \"Orion 300XB\" Kevin MacLeod (incompetech.com)\nLicensed under Creative Commons: By Attribution 3.0 License \nhttp://creativecommons.org/licenses/by/3.0/";
        mt = this.GetComponent<Text>();
		pressSpaceImage = GameObject.Find("PressButtonInvit").GetComponentInChildren<Image>();
		changeTime *= 2.0f;
    }
	
	// Update is called once per frame
    void Update()
    {
		pressSpaceImage.color = new Color(pressSpaceImage.material.color.r,
		                                  pressSpaceImage.material.color.g,
	                                      pressSpaceImage.material.color.b,
		                                  Mathf.PingPong(Time.time * changeTime, 1.0f));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            initialText = mt.text;
            mt.text = otherText;
            otherText = initialText;
        }
    }
}
