using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameInfos : MonoBehaviour {

	//private Shape		playerShape;
	private GameManager	gameManager;
	private Text		remaining;

	// Use this for initialization
	void Start () {
		//playerShape = GameObject.Find ("PlayerShape").GetComponent<Shape> ();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		remaining = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		//TODO: faire ça ailleurs que dans l'update parce que c'est degueulasse ?
		remaining.text = "Shapes left: " + gameManager.remainingTransformation.ToString();
	}
}
