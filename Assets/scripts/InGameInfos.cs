using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InGameInfos : MonoBehaviour {

	private Shape		playerShape;
	private GameManager	gameManager;
	private Text		remaining;
	private List<GameObject>	availableShapes;
	private List<GameObject>	unavailableShapes;

	// Use this for initialization
	void Start () {
		playerShape = GameObject.Find ("PlayerShape").GetComponent<Shape> ();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		remaining = GetComponentInChildren<Text>();

		availableShapes = new List<GameObject> ();
		unavailableShapes = new List<GameObject> ();

		int i;
		for (i = 0; i < (int)ShapeType.ShapeTypeCount; i++) {
			availableShapes.Add(GameObject.Find("Img_" + ((ShapeType)i).ToString() + "_Available"));
			unavailableShapes.Add(GameObject.Find("Img_" + ((ShapeType)i).ToString() + "_Unavailable"));
		}

		i = 0;
		foreach (bool available in playerShape.shapeAvailable) {
			availableShapes[i].gameObject.SetActive(available);
			unavailableShapes[i].gameObject.SetActive(!available);
			i++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//TODO: faire ça ailleurs que dans l'update parce que c'est degueulasse ?
		remaining.text = "Remaining transformations: " + gameManager.remainingTransformation.ToString();
	}
}
