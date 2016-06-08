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
	int _oldTransformation;
	bool _blinkIn = false;
	bool _blinkOut = false;
	float _timeBlink = 0.25f;
	float _currentBlink = 0.0f;
	float _blinkScale = 0.125f;
	Color _originColor;

	// Use this for initialization
	void Start () {
		playerShape = GameObject.Find ("PlayerShape").GetComponent<Shape> ();
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
		remaining = GetComponentInChildren<Text>();
		_oldTransformation = gameManager.remainingTransformation;
		_originColor = remaining.color;

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
	
	void Update () {
		RemainingTransformationColorUpdate();
	}

	void RemainingTransformationColorUpdate() {
		if (_oldTransformation != gameManager.remainingTransformation) {
            Debug.Log(remaining.transform.localScale);
			_oldTransformation = gameManager.remainingTransformation;
			remaining.text = gameManager.remainingTransformation.ToString();
			_blinkIn = true;
			_currentBlink = 0.0f;
            remaining.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        if (_blinkIn) {
			_currentBlink += Time.deltaTime;
			float percentageLerp = _currentBlink / _timeBlink;
			remaining.color = new Color(Mathf.Lerp(_originColor.r, Color.red.r, percentageLerp),
			            Mathf.Lerp(_originColor.g, Color.red.g, percentageLerp),
			            Mathf.Lerp(_originColor.b, Color.red.b, percentageLerp),
			            Mathf.Lerp(_originColor.a, Color.red.a, percentageLerp));
			remaining.transform.localScale += new Vector3(_blinkScale * percentageLerp, _blinkScale * percentageLerp);
			if (_currentBlink > _timeBlink) {
				_blinkIn = false;
                _blinkOut = true;
				_currentBlink = 0.0f;
			}
		} else if (_blinkOut) {
			_currentBlink += Time.deltaTime;
			float percentageLerp = _currentBlink / _timeBlink;
			remaining.color = new Color(Mathf.Lerp(Color.red.r, _originColor.r, percentageLerp),
			                            Mathf.Lerp(Color.red.g, _originColor.g, percentageLerp),
			                            Mathf.Lerp(Color.red.b, _originColor.b, percentageLerp),
			                            Mathf.Lerp(Color.red.a, _originColor.a, percentageLerp));
			remaining.transform.localScale -= new Vector3(_blinkScale * percentageLerp, _blinkScale * percentageLerp);
			if (_currentBlink > _timeBlink) {
				_blinkOut = false;
                remaining.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                remaining.color = _originColor;
                if (gameManager.remainingTransformation <= 0) {
                    remaining.color = Color.red;
                }
			}
		}
	}
}
